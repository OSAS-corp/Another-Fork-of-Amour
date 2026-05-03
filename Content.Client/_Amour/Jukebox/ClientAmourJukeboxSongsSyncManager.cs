using Content.Shared._Amour.Jukebox;

namespace Content.Client._Amour.Jukebox;

public sealed class ClientAmourJukeboxSongsSyncManager : AmourJukeboxSongsSyncManager
{
    public override void OnSongUploaded(AmourJukeboxSongUploadNetMessage message)
    {
        ContentRoot.AddOrUpdateFile(message.RelativePath, message.Data);
    }
}
