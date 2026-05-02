using Content.Shared._White.Move;
using Content.Shared.Interaction;
using Content.Shared.IdentityManagement;
using Content.Shared.Hands.Components;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.Interaction.Events;

namespace Content.Shared._EE.OfferItem;
public abstract partial class SharedOfferItemSystem : EntitySystem
{
    [Dependency] private readonly SharedTransformSystem _transform = default!;
    [Dependency] private readonly SharedHandsSystem _hands = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<OfferItemComponent, InteractUsingEvent>(SetInReceiveMode);
        SubscribeLocalEvent<OfferItemComponent, MoveEventProxy>(OnMove);
        SubscribeLocalEvent<OfferItemComponent, AcceptOfferAlertEvent>(OnAcceptOfferAlert);

        InitializeInteractions();
    }

    private void SetInReceiveMode(EntityUid uid, OfferItemComponent component, InteractUsingEvent args)
    {
        if (!TryComp<OfferItemComponent>(args.User, out var offerItem))
            return;

        if (args.User == uid || component.IsInReceiveMode || !offerItem.IsInOfferMode ||
            (offerItem.IsInReceiveMode && offerItem.Target != uid))
            return;

        component.IsInReceiveMode = true;
        component.Target = args.User;

        Dirty(uid, component);

        offerItem.Target = uid;
        offerItem.IsInOfferMode = false;

        Dirty(args.User, offerItem);

        if (offerItem.Item == null)
            return;

        _popup.PopupPredicted(
            Loc.GetString("offer-item-try-give",
                ("item", Identity.Entity(offerItem.Item.Value, EntityManager)),
                ("target", Identity.Entity(uid, EntityManager))),
            Loc.GetString("offer-item-try-give-target",
                ("user", Identity.Entity(component.Target.Value, EntityManager)),
                ("item", Identity.Entity(offerItem.Item.Value, EntityManager))),
            component.Target.Value,
            component.Target.Value);

        args.Handled = true;
    }
    private void OnMove(EntityUid uid, OfferItemComponent component, MoveEventProxy args)
    {
        if (component.Target == null ||
            args.NewPosition.InRange(EntityManager, _transform,
                Transform(component.Target.Value).Coordinates, component.MaxOfferDistance))
            return;

        UnOffer(uid, component);
    }

    private void OnAcceptOfferAlert(EntityUid uid, OfferItemComponent component, AcceptOfferAlertEvent args)
    {
        if (!TryComp<OfferItemComponent>(component.Target, out var offerItem)
            || !TryComp<HandsComponent>(uid, out var hands)
            || offerItem.Hand == null)
            return;

        if (offerItem.Item != null)
        {
            if (!_hands.TryPickup(uid, offerItem.Item.Value, handsComp: hands))
            {
                _popup.PopupClient(Loc.GetString("offer-item-full-hand"), uid, uid);
                return;
            }

            _popup.PopupPredicted(
                Loc.GetString(
                    "offer-item-give",
                    ("item", Identity.Entity(offerItem.Item.Value, EntityManager)),
                    ("target", Identity.Entity(uid, EntityManager))),
                Loc.GetString(
                    "offer-item-give-other",
                    ("user", Identity.Entity(component.Target.Value, EntityManager)),
                    ("item", Identity.Entity(offerItem.Item.Value, EntityManager)),
                    ("target", Identity.Entity(uid, EntityManager))),
                component.Target.Value,
                component.Target.Value);
            RaiseLocalEvent(offerItem.Item.Value, new DroppedEvent(uid)); // WD EDIT
        }

        offerItem.Item = null;
        UnReceive(uid, component, offerItem);
    }

    /// <summary>
    /// Resets the <see cref="OfferItemComponent"/> of the user and the target
    /// </summary>
    protected void UnOffer(EntityUid uid, OfferItemComponent component)
    {
        if (!TryComp<HandsComponent>(uid, out var hands) || _hands.GetActiveHand((uid, hands)) == null)
            return;


        if (TryComp<OfferItemComponent>(component.Target, out var offerItem) && component.Target != null)
        {

            if (component.Item != null)
            {
                _popup.PopupPredicted(
                    Loc.GetString("offer-item-no-give",
                        ("item", Identity.Entity(component.Item.Value, EntityManager)),
                        ("target", Identity.Entity(component.Target.Value, EntityManager))),
                    Loc.GetString("offer-item-no-give-target",
                        ("user", Identity.Entity(uid, EntityManager)),
                        ("item", Identity.Entity(component.Item.Value, EntityManager))),
                    uid,
                    uid);
            }

            else if (offerItem.Item != null)
            {
                _popup.PopupPredicted(
                    Loc.GetString("offer-item-no-give",
                        ("item", Identity.Entity(offerItem.Item.Value, EntityManager)),
                        ("target", Identity.Entity(uid, EntityManager))),
                    Loc.GetString("offer-item-no-give-target",
                        ("user", Identity.Entity(component.Target.Value, EntityManager)),
                        ("item", Identity.Entity(offerItem.Item.Value, EntityManager))),
                    component.Target.Value,
                    component.Target.Value);
            }

            offerItem.IsInOfferMode = false;
            offerItem.IsInReceiveMode = false;
            offerItem.Hand = null;
            offerItem.Target = null;
            offerItem.Item = null;

            Dirty(component.Target.Value, offerItem);
        }
        component.IsInOfferMode = false;
        component.IsInReceiveMode = false;
        component.Hand = null;
        component.Target = null;
        component.Item = null;

        Dirty(uid, component);
    }


    /// <summary>
    /// Cancels the transfer of the item
    /// </summary>
    protected void UnReceive(EntityUid uid, OfferItemComponent? component = null, OfferItemComponent? offerItem = null)
    {
        if (component == null && !TryComp(uid, out component))
            return;

        if (offerItem == null && !TryComp(component.Target, out offerItem))
            return;

        if (!TryComp<HandsComponent>(uid, out var hands) || _hands.GetActiveHand((uid, hands)) == null ||
            component.Target == null)
            return;

        if (offerItem.Item != null)
        {
            _popup.PopupPredicted(
                Loc.GetString("offer-item-no-give",
                    ("item", Identity.Entity(offerItem.Item.Value, EntityManager)),
                    ("target", Identity.Entity(uid, EntityManager))),
                Loc.GetString("offer-item-no-give-target",
                    ("user", Identity.Entity(component.Target.Value, EntityManager)),
                    ("item", Identity.Entity(offerItem.Item.Value, EntityManager))),
                component.Target.Value,
                component.Target.Value);
        }

        var offerer = component.Target.Value;

        offerItem.Item = null;
        offerItem.Hand = null;
        offerItem.IsInOfferMode = false;
        offerItem.IsInReceiveMode = false;
        offerItem.Target = null;

        component.IsInReceiveMode = false;
        component.IsInOfferMode = false;
        component.Hand = null;
        component.Item = null;
        component.Target = null;

        Dirty(uid, component);
        Dirty(offerer, offerItem);
    }
    /// <summary>
    /// Returns true if <see cref="OfferItemComponent.IsInOfferMode"/> = true
    /// </summary>
    protected bool IsInOfferMode(EntityUid? entity, OfferItemComponent? component = null)
    {
        return entity != null && Resolve(entity.Value, ref component, false) && component.IsInOfferMode;
    }
}
