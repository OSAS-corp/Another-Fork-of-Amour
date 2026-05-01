using System.Linq;
using Content.Server.Popups;
using Content.Shared._Amour.Jukebox;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.Interaction;
using Content.Shared.Popups;
using Content.Shared.Tag;
using Content.Shared.Verbs;
using Robust.Server.Containers;
using Robust.Shared.Containers;
using Robust.Shared.GameObjects;
using Robust.Shared.GameStates;
using Robust.Shared.IoC;
using Robust.Shared.Utility;

namespace Content.Server._Amour.Jukebox;

public sealed class AmourTapeCreatorSystem : EntitySystem
{
    [Dependency] private readonly ServerAmourJukeboxSongsSyncManager _songsSyncManager = default!;
    [Dependency] private readonly PopupSystem _popup = default!;
    [Dependency] private readonly ContainerSystem _container = default!;
    [Dependency] private readonly SharedHandsSystem _hands = default!;
    [Dependency] private readonly TagSystem _tag = default!;

    private const string TapeCreatorContainerName = "amour_tape_creator_container";
    private const string CoinTag = "AmourTapeRecorderCoin";

    public override void Initialize()
    {
        base.Initialize();
        SubscribeNetworkEvent<AmourJukeboxSongUploadRequest>(OnSongUploaded);
        SubscribeLocalEvent<AmourTapeCreatorComponent, ComponentInit>(OnComponentInit);
        SubscribeLocalEvent<AmourTapeCreatorComponent, InteractUsingEvent>(OnInteract);
        SubscribeLocalEvent<AmourTapeCreatorComponent, GetVerbsEvent<Verb>>(OnTapeCreatorGetVerb);
        SubscribeLocalEvent<AmourTapeCreatorComponent, ComponentGetState>(OnTapeCreatorStateChanged);
        SubscribeLocalEvent<AmourTapeComponent, ComponentGetState>(OnTapeStateChanged);
    }

    private void OnTapeCreatorGetVerb(EntityUid uid, AmourTapeCreatorComponent component, GetVerbsEvent<Verb> ev)
    {
        if (component.Recording)
            return;
        if (ev.Hands == null)
            return;
        if (component.TapeContainer.ContainedEntities.Count == 0)
            return;

        var removeTapeVerb = new Verb
        {
            Text = "Вытащить кассету",
            Priority = 10000,
            Icon = new SpriteSpecifier.Texture(new ResPath("/Textures/Interface/VerbIcons/remove_tape.png")),
            Act = () =>
            {
                var tapes = component.TapeContainer.ContainedEntities.ToList();
                _container.EmptyContainer(component.TapeContainer, true);

                foreach (var tape in tapes)
                {
                    _hands.PickupOrDrop(ev.User, tape);
                }

                component.InsertedTape = null;
                Dirty(uid, component);
            }
        };

        ev.Verbs.Add(removeTapeVerb);
    }

    private void OnTapeStateChanged(EntityUid uid, AmourTapeComponent component, ref ComponentGetState args)
    {
        args.State = new AmourTapeComponentState
        {
            Songs = component.Songs
        };
    }

    private void OnTapeCreatorStateChanged(EntityUid uid, AmourTapeCreatorComponent component, ref ComponentGetState args)
    {
        args.State = new AmourTapeCreatorComponentState
        {
            Recording = component.Recording,
            CoinBalance = component.CoinBalance,
            InsertedTape = component.InsertedTape
        };
    }

    private void OnComponentInit(EntityUid uid, AmourTapeCreatorComponent component, ComponentInit args)
    {
        component.TapeContainer = _container.EnsureContainer<Container>(uid, TapeCreatorContainerName);
    }

    private void OnInteract(EntityUid uid, AmourTapeCreatorComponent component, InteractUsingEvent args)
    {
        if (component.Recording)
            return;

        if (HasComp<AmourTapeComponent>(args.Used))
        {
            var containedEntities = component.TapeContainer.ContainedEntities;

            if (containedEntities.Count > 1)
            {
                var removedTapes = _container.EmptyContainer(component.TapeContainer, true).ToList();
                _container.Insert(args.Used, component.TapeContainer);

                foreach (var tape in removedTapes)
                {
                    _hands.PickupOrDrop(args.User, tape);
                }
            }
            else
            {
                _container.Insert(args.Used, component.TapeContainer);
            }

            component.InsertedTape = GetNetEntity(args.Used);
            Dirty(uid, component);
            return;
        }

        if (_tag.HasTag(args.Used, CoinTag))
        {
            Del(args.Used);
            component.CoinBalance += 1;
            Dirty(uid, component);
        }
    }

    private void OnSongUploaded(AmourJukeboxSongUploadRequest ev, EntitySessionEventArgs args)
    {
        var tapeCreator = GetEntity(ev.TapeCreatorUid);
        if (!TryComp<AmourTapeCreatorComponent>(tapeCreator, out var tapeCreatorComponent))
        {
            SendUploadResponse(args, ev.TapeCreatorUid, false, "Записывающее устройство не найдено.");
            return;
        }

        if (!tapeCreatorComponent.InsertedTape.HasValue || tapeCreatorComponent.CoinBalance <= 0)
        {
            _popup.PopupEntity("Запись была прервана: нет кассеты или жетонов.", tapeCreator);
            SendUploadResponse(args, ev.TapeCreatorUid, false, "Нет кассеты или жетонов.");
            return;
        }

        tapeCreatorComponent.CoinBalance -= 1;
        tapeCreatorComponent.Recording = true;

        var insertedTape = GetEntity(tapeCreatorComponent.InsertedTape.Value);
        if (!TryComp<AmourTapeComponent>(insertedTape, out var tapeComponent))
        {
            tapeCreatorComponent.Recording = false;
            SendUploadResponse(args, ev.TapeCreatorUid, false, "Кассета недоступна.");
            return;
        }

        var songData = _songsSyncManager.SyncSongData(ev.SongName, ev.SongBytes);

        var song = new AmourJukeboxSong
        {
            SongName = songData.SongName,
            SongPath = songData.Path
        };

        tapeComponent.Songs.Add(song);

        DirtyEntity(tapeCreator);
        Dirty(insertedTape, tapeComponent);

        FinishRecording(tapeCreator, tapeCreatorComponent);
        SendUploadResponse(args, ev.TapeCreatorUid, true, "Запись на кассету завершена.");
    }

    private void SendUploadResponse(EntitySessionEventArgs args, NetEntity tapeCreatorUid, bool success, string? message)
    {
        RaiseNetworkEvent(new AmourJukeboxSongUploadResponse
        {
            TapeCreatorUid = tapeCreatorUid,
            Success = success,
            Message = message
        }, args.SenderSession);
    }

    private void FinishRecording(EntityUid uid, AmourTapeCreatorComponent component)
    {
        _container.EmptyContainer(component.TapeContainer, force: true);

        component.Recording = false;
        component.InsertedTape = null;

        _popup.PopupEntity("Запись на кассету завершена.", uid);
        Dirty(uid, component);
    }
}
