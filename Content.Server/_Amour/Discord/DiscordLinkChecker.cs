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

public sealed class DiscordLinkChecker : IDiscordLinkChecker, IPostInjectInit
{
    [Dependency] private readonly IServerDbManager _db = default!;
    
    private ISawmill _sawmill = default!;
    private readonly ConcurrentDictionary<Guid, (bool IsLinked, TimeSpan LastCheck)> _linkCache = new();
    private readonly TimeSpan _cacheExpiry = TimeSpan.FromMinutes(5);
    private readonly ConcurrentDictionary<Guid, SemaphoreSlim> _checkLocks = new();

    public void PostInject()
    {
        _sawmill = Logger.GetSawmill("discord.link");
    }

    public async Task<bool> IsDiscordLinkedAsync(ICommonSession session)
    {
        var userId = session.UserId.UserId;

        var lockObj = _checkLocks.GetOrAdd(userId, _ => new SemaphoreSlim(1, 1));
        
        await lockObj.WaitAsync();
        try
        {
            if (_linkCache.TryGetValue(userId, out var cached))
            {
                var age = TimeSpan.FromTicks(DateTime.UtcNow.Ticks) - cached.LastCheck;
                if (age < _cacheExpiry)
                {
                    return cached.IsLinked;
                }
            }

            var isLinked = await _db.HasLinkedAccount(userId, CancellationToken.None);
            _linkCache[userId] = (isLinked, TimeSpan.FromTicks(DateTime.UtcNow.Ticks));
            return isLinked;
        }
        catch (Exception ex)
        {
            _sawmill.Error($"Failed to check Discord link for {userId}: {ex}");
            return false;
        }
        finally
        {
            lockObj.Release();
        }
    }
    
    public bool IsDiscordLinkedCached(NetUserId userId)
    {
        if (_linkCache.TryGetValue(userId.UserId, out var cached))
        {
            var age = TimeSpan.FromTicks(DateTime.UtcNow.Ticks) - cached.LastCheck;
            if (age < _cacheExpiry)
            {
                return cached.IsLinked;
            }
        }
        return false;
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
