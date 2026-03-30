using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Content.Server.Database;
using Robust.Shared.IoC;
using Robust.Shared.Log;
using Robust.Shared.Network;
using Robust.Shared.Player;

namespace Content.Server._Amour.Discord;

public sealed class DiscordLinkChecker : IDiscordLinkChecker
{
    [Dependency] private readonly IServerDbManager _db = default!;
    
    private ISawmill _sawmill = default!;
    private readonly ConcurrentDictionary<Guid, bool> _linkCache = new();

    public void Initialize()
    {
        _sawmill = Logger.GetSawmill("discord.link");
    }

    public async Task<bool> IsDiscordLinkedAsync(ICommonSession session)
    {
        var userId = session.UserId.UserId;
        
        try
        {
            var isLinked = await _db.HasLinkedAccount(userId, CancellationToken.None);
            _linkCache[userId] = isLinked;
            return isLinked;
        }
        catch (Exception ex)
        {
            _sawmill.Error($"Failed to check Discord link for {userId}: {ex}");
            return false;
        }
    }
    
    public bool IsDiscordLinkedCached(NetUserId userId)
    {
        return _linkCache.TryGetValue(userId.UserId, out var isLinked) && isLinked;
    }
    
    public async Task RefreshLinkStatusAsync(ICommonSession session)
    {
        await IsDiscordLinkedAsync(session);
    }
    
    public void ClearCache()
    {
        _linkCache.Clear();
    }
}
