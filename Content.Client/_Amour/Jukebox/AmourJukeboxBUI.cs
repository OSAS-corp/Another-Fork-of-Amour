using Content.Shared._Amour.Jukebox;
using Content.Shared.Popups;
using Robust.Client.GameObjects;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using System;

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
            _entityManager.System<SharedPopupSystem>().PopupEntity("Тут нет AmourJukeboxComponent", Owner);
            Close();
            return;
        }

        _window = new AmourJukeboxMenu(Owner, jukeboxComponent);
        _window.RepeatButton.OnToggled += OnRepeatButtonToggled;
        _window.StopButton.OnPressed += OnStopButtonPressed;
        _window.EjectButton.OnPressed += OnEjectButtonPressed;
        _window.PlayPausePressed += OnPlayPausePressed;
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

    private void OnEjectButtonPressed(BaseButton.ButtonEventArgs obj)
    {
        SendMessage(new AmourJukeboxEjectRequest());
    }

    private void OnPlayPausePressed()
    {
        SendMessage(new AmourJukeboxStopRequest());
    }

    private void OnStopButtonPressed(BaseButton.ButtonEventArgs obj)
    {
        SendMessage(new AmourJukeboxStopRequest());
    }

    private void OnRepeatButtonToggled(BaseButton.ButtonToggledEventArgs obj)
    {
        SendMessage(new AmourJukeboxRepeatToggled(obj.Pressed));
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (!disposing) return;

        _window?.Dispose();
    }
}
