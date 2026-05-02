using System;
using Content.Shared._Amour.Jukebox;
using Content.Shared.Popups;
using Robust.Client.GameObjects;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.Localization;

namespace Content.Client._Amour.Jukebox;

public sealed class AmourJukeboxBUI : BoundUserInterface
{
    [Dependency] private readonly IEntityManager _entityManager = default!;

    private AmourJukeboxMenu? _window;

    public AmourJukeboxBUI(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
        IoCManager.InjectDependencies(this);
    }

    protected override void Open()
    {
        base.Open();

        if (!_entityManager.TryGetComponent(Owner, out AmourJukeboxComponent? jukeboxComponent))
        {
            _entityManager.System<SharedPopupSystem>()
                .PopupEntity(Loc.GetString("amour-jukebox-missing-component"), Owner);
            Close();
            return;
        }

        _window = new AmourJukeboxMenu(Owner, jukeboxComponent);
        _window.PlayPausePressed += OnPlayPausePressed;
        _window.StopPressed += OnStopPressed;
        _window.EjectPressed += OnEjectPressed;
        _window.RepeatToggled += OnRepeatToggled;
        _window.SetPlaybackPosition += OnSetPlaybackPosition;
        _window.SetVolume += OnSetVolume;
        _window.OpenCentered();
        _window.OnClose += Close;
    }

    private void OnSetPlaybackPosition(float playbackPosition)
    {
        SendMessage(new AmourJukeboxSetPlaybackPosition(playbackPosition));
    }

    private void OnSetVolume(float volume)
    {
        SendMessage(new AmourJukeboxSetVolume(volume));
    }

    private void OnEjectPressed()
    {
        SendMessage(new AmourJukeboxEjectRequest());
    }

    private void OnPlayPausePressed()
    {
        SendMessage(new AmourJukeboxStopRequest());
    }

    private void OnStopPressed()
    {
        SendMessage(new AmourJukeboxStopRequest());
    }

    private void OnRepeatToggled(bool newState)
    {
        SendMessage(new AmourJukeboxRepeatToggled(newState));
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (!disposing) return;

        _window?.Dispose();
    }
}
