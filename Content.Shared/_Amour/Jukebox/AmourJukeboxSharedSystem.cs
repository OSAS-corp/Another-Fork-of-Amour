using Robust.Shared.Containers;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;

namespace Content.Shared._Amour.Jukebox;

public sealed class AmourJukeboxSharedSystem : EntitySystem
{
    [Dependency] private readonly SharedContainerSystem _containerSystem = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<AmourJukeboxComponent, ComponentStartup>(OnJukeboxInit);
    }

    private void OnJukeboxInit(EntityUid uid, AmourJukeboxComponent component, ComponentStartup args)
    {
        component.TapeContainer =
            _containerSystem.EnsureContainer<Container>(uid, AmourJukeboxComponent.JukeboxContainerName);
    }
}
