using System;
using Content.Shared._Amour.Jukebox;
using Robust.Client.GameObjects;
using Robust.Shared.GameObjects;
using Robust.Shared.GameStates;

namespace Content.Client._Amour.Jukebox;

public sealed class AmourTapeCreatorSystem : EntitySystem
{
    public event Action<NetEntity, bool, string?>? UploadResponseReceived;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<AmourTapeCreatorComponent, ComponentHandleState>(OnStateChanged);
        SubscribeLocalEvent<AmourTapeComponent, ComponentHandleState>(OnTapeStateChanged);
        SubscribeNetworkEvent<AmourJukeboxSongUploadResponse>(OnUploadResponse);
    }

    private void OnUploadResponse(AmourJukeboxSongUploadResponse ev)
    {
        UploadResponseReceived?.Invoke(ev.TapeCreatorUid, ev.Success, ev.Message);
    }

    private void OnTapeStateChanged(EntityUid uid, AmourTapeComponent component, ref ComponentHandleState args)
    {
        if (args.Current is not AmourTapeComponentState state)
            return;

        component.Songs = state.Songs;
    }

    private void OnStateChanged(EntityUid uid, AmourTapeCreatorComponent component, ref ComponentHandleState args)
    {
        if (args.Current is not AmourTapeCreatorComponentState state)
            return;

        component.Recording = state.Recording;
        component.CoinBalance = state.CoinBalance;
        component.InsertedTape = state.InsertedTape;

        SetTapeLayerVisible(uid, state.InsertedTape.HasValue);
    }

    private void SetTapeLayerVisible(EntityUid uid, bool visible)
    {
        if (!TryComp<SpriteComponent>(uid, out var sprite))
            return;

        if (sprite.LayerMapTryGet("tape", out var layer))
            sprite.LayerSetVisible(layer, visible);
    }
}
