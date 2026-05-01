using System;
using Robust.Shared.ContentPack;
using Robust.Shared.IoC;
using Robust.Shared.Network;
using Robust.Shared.Utility;

namespace Content.Shared._Amour.Jukebox;

public abstract class AmourJukeboxSongsSyncManager : IDisposable
{
    [Dependency] private readonly INetManager _netManager = default!;
    [Dependency] protected readonly IResourceManager ResourceManager = default!;

    // Отдельная корневая папка, чтобы не пересекаться со штатным Jukebox.
    public static readonly ResPath Prefix = ResPath.Root / "AmourJukebox";

    protected readonly MemoryContentRoot ContentRoot = new();

    public virtual void Initialize()
    {
        ResourceManager.AddRoot(Prefix, ContentRoot);

        _netManager.RegisterNetMessage<AmourJukeboxSongUploadNetMessage>(OnSongUploaded);
    }

    public abstract void OnSongUploaded(AmourJukeboxSongUploadNetMessage message);

    public void Dispose()
    {
        ContentRoot.Dispose();
    }
}
