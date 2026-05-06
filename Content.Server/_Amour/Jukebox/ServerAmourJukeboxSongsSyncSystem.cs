using Content.Shared.GameTicking;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;

namespace Content.Server._Amour.Jukebox;

public sealed class ServerAmourJukeboxSongsSyncSystem : EntitySystem
{
    [Dependency] private readonly ServerAmourJukeboxSongsSyncManager _jukeboxManager = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<RoundRestartCleanupEvent>(_ => _jukeboxManager.CleanUp());
    }
}
