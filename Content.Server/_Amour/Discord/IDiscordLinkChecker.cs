using System.Threading.Tasks;
using Robust.Shared.Network;
using Robust.Shared.Player;

namespace Content.Server._Amour.Discord;

public interface IDiscordLinkChecker
{
    Task<bool> IsDiscordLinkedAsync(ICommonSession session);
    bool IsDiscordLinkedCached(NetUserId userId);
    Task RefreshLinkStatusAsync(ICommonSession session);
    void ClearCache();
}
