using Content.Shared.Popups;
using Content.Shared.ActionBlocker;
using Content.Shared.Input;
using Content.Shared.Hands.Components;
using Robust.Shared.Input.Binding;
using Robust.Shared.Player;
using Robust.Shared.Log;

namespace Content.Shared._EE.OfferItem;

public abstract partial class SharedOfferItemSystem
{
    [Dependency] private readonly ActionBlockerSystem _actionBlocker = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;

    private void InitializeInteractions()
    {
        Log.Info("OfferItem: Registering keybind for OfferItem");
        CommandBinds.Builder
            .Bind(ContentKeyFunctions.OfferItem, InputCmdHandler.FromDelegate(SetInOfferMode))
            .Register<SharedOfferItemSystem>();
        Log.Info("OfferItem: Keybind registered successfully");
    }

    public override void Shutdown()
    {
        base.Shutdown();

        CommandBinds.Unregister<SharedOfferItemSystem>();
    }

    private void SetInOfferMode(ICommonSession? session)
    {
        Log.Info("OfferItem: SetInOfferMode called!");
        
        if (session is not { } playerSession)
        {
            Log.Warning("OfferItem: No session");
            return;
        }

        if ((playerSession.AttachedEntity is not { Valid: true } uid || !Exists(uid)) ||
            !_actionBlocker.CanInteract(uid, null))
        {
            Log.Warning("OfferItem: No entity or can't interact");
            return;
        }

        if (!TryComp<OfferItemComponent>(uid, out var offerItem))
        {
            Log.Warning($"OfferItem: No OfferItemComponent on {uid}");
            return;
        }

        if (!TryComp<HandsComponent>(uid, out var hands))
        {
            Log.Warning($"OfferItem: No HandsComponent on {uid}");
            return;
        }

        var activeHand = _hands.GetActiveHand((uid, hands));
        if (activeHand == null)
        {
            Log.Warning("OfferItem: No active hand");
            return;
        }

        offerItem.Item = _hands.GetHeldItem((uid, hands), activeHand);

        if (!offerItem.IsInOfferMode)
        {
            if (offerItem.Item == null)
            {
                Log.Info("OfferItem: Empty hand");
                _popup.PopupClient(Loc.GetString("offer-item-empty-hand"), uid, uid);
                return;
            }

            offerItem.IsInOfferMode = true;
            offerItem.Hand = activeHand;
            Dirty(uid, offerItem);
            Log.Info($"OfferItem: ENTERED offer mode for {uid}");
            return;
        }

        Log.Info($"OfferItem: EXITING offer mode for {uid}");
        if (offerItem.Target != null)
        {
            UnReceive(offerItem.Target.Value, offerItem: offerItem);
            return;
        }

        UnOffer(uid, offerItem);
    }
}
