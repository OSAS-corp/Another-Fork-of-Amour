using System;
using System.Collections.Generic;
using System.Linq;
using Content.Shared._Amour.Jukebox;
using Content.Shared.Audio.Jukebox;
using Content.Shared.GameTicking;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.Interaction;
using Content.Shared.Verbs;
using Robust.Server.GameObjects;
using Robust.Server.GameStates;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Containers;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Server._Amour.Jukebox;

public sealed class AmourJukeboxSystem : EntitySystem
{
    [Dependency] private readonly SharedContainerSystem _containerSystem = default!;
    [Dependency] private readonly SharedHandsSystem _handsSystem = default!;
    [Dependency] private readonly PvsOverrideSystem _pvsOverrideSystem = default!;
    [Dependency] private readonly UserInterfaceSystem _uiSystem = default!;
    [Dependency] private readonly SharedInteractionSystem _interaction = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly IPrototypeManager _proto = default!;

    private readonly List<AmourJukeboxComponent> _playingJukeboxes = new();
    private const float UpdateTimerDefaultTime = 1f;
    private float _updateTimer;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeNetworkEvent<AmourJukeboxRequestSongPlay>(OnSongRequestPlay);
        SubscribeLocalEvent<AmourJukeboxComponent, InteractUsingEvent>(OnInteract);
        SubscribeLocalEvent<AmourJukeboxComponent, AmourJukeboxStopRequest>(OnRequestStop);
        SubscribeLocalEvent<AmourJukeboxComponent, AmourJukeboxRepeatToggled>(OnRepeatToggled);
        SubscribeLocalEvent<AmourJukeboxComponent, AmourJukeboxEjectRequest>(OnEjectRequest);
        SubscribeLocalEvent<AmourJukeboxComponent, AmourJukeboxSetPlaybackPosition>(OnSetPlaybackPosition);
        SubscribeLocalEvent<AmourJukeboxComponent, AmourJukeboxSetVolume>(OnSetVolume);
        SubscribeLocalEvent<AmourJukeboxComponent, GetVerbsEvent<Verb>>(OnGetVerb);
        SubscribeLocalEvent<AmourJukeboxComponent, ComponentInit>(OnInit);
        SubscribeLocalEvent<RoundRestartCleanupEvent>(OnRoundRestart);
    }

    private void OnInit(EntityUid uid, AmourJukeboxComponent component, ComponentInit args)
    {
        _pvsOverrideSystem.AddGlobalOverride(uid);

        component.TapeContainer = _containerSystem.EnsureContainer<Container>(uid, AmourJukeboxComponent.JukeboxContainerName);
    }

    private void OnRoundRestart(RoundRestartCleanupEvent ev)
    {
        _playingJukeboxes.Clear();
    }

    private void OnEjectRequest(EntityUid uid, AmourJukeboxComponent component, AmourJukeboxEjectRequest args)
    {
        if (component.PlayingSongData != null)
            return;

        if (component.TapeContainer.ContainedEntities.Count > 0)
        {
            _containerSystem.EmptyContainer(component.TapeContainer, true);
            _uiSystem.CloseUi(uid, AmourJukeboxUIKey.Key);
        }
    }

    private void OnGetVerb(EntityUid uid, AmourJukeboxComponent jukeboxComponent, GetVerbsEvent<Verb> ev)    {
        if (ev.Hands == null)
            return;
        if (jukeboxComponent.PlayingSongData != null)
            return;
        if (jukeboxComponent.TapeContainer.ContainedEntities.Count == 0)
            return;

        var removeTapeVerb = new Verb
        {
            Text = "Вытащить кассету",
            Priority = 10000,
            Icon = new SpriteSpecifier.Texture(new ResPath("/Textures/Interface/VerbIcons/remove_tape.png")),
            Act = () =>
            {
                var tapes = jukeboxComponent.TapeContainer.ContainedEntities.ToList();
                _containerSystem.EmptyContainer(jukeboxComponent.TapeContainer, true);

                foreach (var tape in tapes)
                {
                    _handsSystem.PickupOrDrop(ev.User, tape);
                }

                _uiSystem.CloseUi(uid, AmourJukeboxUIKey.Key);
            }
        };
        ev.Verbs.Add(removeTapeVerb);
    }

    private void OnRepeatToggled(EntityUid uid, AmourJukeboxComponent component, AmourJukeboxRepeatToggled args)
    {
        component.Playing = args.NewState;
        Dirty(uid, component);
    }

    private void OnSetPlaybackPosition(EntityUid uid, AmourJukeboxComponent component, AmourJukeboxSetPlaybackPosition args)
    {
        if (component.PlayingSongData == null)
            return;

        component.PlayingSongData.PlaybackPosition = Math.Clamp(args.PlaybackPosition,
            0f,
            component.PlayingSongData.ActualSongLengthSeconds);

        if (!_playingJukeboxes.Contains(component))
            _playingJukeboxes.Add(component);

        Dirty(uid, component);
    }

    private void OnSetVolume(EntityUid uid, AmourJukeboxComponent component, AmourJukeboxSetVolume args)
    {
        component.Volume = Math.Clamp(args.Volume, 0f, 1f);
        Dirty(uid, component);
    }

    private void OnRequestStop(EntityUid uid, AmourJukeboxComponent component, AmourJukeboxStopRequest args)
    {
        component.PlayingSongData = null;
        Dirty(uid, component);
    }

    private void OnInteract(EntityUid uid, AmourJukeboxComponent component, InteractUsingEvent args)
    {
        if (component.PlayingSongData != null)
            return;

        if (!HasComp<AmourTapeComponent>(args.Used))
            return;

        var containedEntities = component.TapeContainer.ContainedEntities;

        if (containedEntities.Count >= 1)
        {
            var removedTapes = _containerSystem.EmptyContainer(component.TapeContainer, true).ToList();

            foreach (var tapeUid in removedTapes)
            {
                _handsSystem.PickupOrDrop(args.User, tapeUid);
            }
        }

        _containerSystem.Insert(args.Used, component.TapeContainer);

        _uiSystem.CloseUi(uid, AmourJukeboxUIKey.Key);
    }

    private void OnSongRequestPlay(AmourJukeboxRequestSongPlay msg, EntitySessionEventArgs args)
    {
        if (msg.Jukebox == null || msg.SongPath == null)
            return;

        var entity = GetEntity(msg.Jukebox.Value);
        if (!Exists(entity) || !TryComp<AmourJukeboxComponent>(entity, out var jukebox))
            return;

        var session = args.SenderSession;
        if (session.AttachedEntity is not { } sender || !Exists(sender))
            return;

        if (!_uiSystem.IsUiOpen(entity, AmourJukeboxUIKey.Key, sender))
            return;

        if (!_interaction.InRangeUnobstructed(sender, entity))
            return;

        if (!TryResolveSongDuration(msg.SongPath.Value, out var duration))
            return;

        jukebox.Playing = true;

        jukebox.PlayingSongData = new AmourPlayingSongData
        {
            SongName = msg.SongName,
            SongPath = msg.SongPath,
            ActualSongLengthSeconds = duration,
            PlaybackPosition = 0f
        };

        if (!_playingJukeboxes.Contains(jukebox))
            _playingJukeboxes.Add(jukebox);

        Dirty(entity, jukebox);
    }

    private bool TryResolveSongDuration(ResPath songPath, out float duration)
    {
        duration = 0f;

        foreach (var proto in _proto.EnumeratePrototypes<JukeboxPrototype>())
        {
            if (proto.Path.Path != songPath)
                continue;

            try
            {
                duration = (float) _audio.GetAudioLength(songPath.ToString()).TotalSeconds;
            }
            catch
            {
                duration = 0f;
            }
            return true;
        }

        var query = EntityQueryEnumerator<AmourTapeComponent>();
        while (query.MoveNext(out _, out var tape))
        {
            foreach (var song in tape.Songs)
            {
                if (song.SongPath != songPath)
                    continue;

                duration = song.SongDurationSeconds;
                return true;
            }
        }

        return false;
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        if (_updateTimer <= UpdateTimerDefaultTime)
        {
            _updateTimer += frameTime;
            return;
        }

        ProcessPlayingJukeboxes();
    }

    private void ProcessPlayingJukeboxes()
    {
        for (var i = _playingJukeboxes.Count - 1; i >= 0; i--)
        {
            var playingJukeboxData = _playingJukeboxes[i];

            if (playingJukeboxData.PlayingSongData == null)
            {
                _playingJukeboxes.RemoveAt(i);
                continue;
            }

            playingJukeboxData.PlayingSongData.PlaybackPosition += _updateTimer;

            if (playingJukeboxData.PlayingSongData.PlaybackPosition >=
                playingJukeboxData.PlayingSongData.ActualSongLengthSeconds)
            {
                if (playingJukeboxData.Playing)
                {
                    playingJukeboxData.PlayingSongData.PlaybackPosition = 0;
                }
                else
                {
                    playingJukeboxData.PlayingSongData = null;
                    _playingJukeboxes.RemoveAt(i);

                    RaiseNetworkEvent(new AmourJukeboxStopPlaying
                    {
                        JukeboxUid = GetNetEntity(playingJukeboxData.Owner)
                    });

                    Dirty(playingJukeboxData.Owner, playingJukeboxData);
                    continue;
                }
            }

            Dirty(playingJukeboxData.Owner, playingJukeboxData);
        }

        _updateTimer = 0;
    }
}
