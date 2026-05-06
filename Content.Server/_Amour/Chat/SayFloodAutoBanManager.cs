using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Content.Server.Administration;
using Content.Server.Administration.Managers;
using Content.Shared.Database;
using Robust.Shared.IoC;
using Robust.Shared.Network;
using Robust.Shared.Player;
using Robust.Shared.Timing;

namespace Content.Server._Amour.Chat;

public sealed class SayFloodAutoBanManager
{
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly IBanManager _banManager = default!;
    [Dependency] private readonly IPlayerLocator _locator = default!;

    private const int WindowSeconds = 60;
    private const int LimitPerWindow = 70;
    private const string BanReason = "Твинк";
    private const string BanningAdminName = "Meowklka";

    private readonly Dictionary<NetUserId, Queue<TimeSpan>> _history = new();
    private readonly HashSet<NetUserId> _banned = new();

    private NetUserId? _banningAdminId;
    private bool _banningAdminLookupFailed;

    public void Initialize()
    {
    }

    public void RegisterSayUsage(ICommonSession player)
    {
        var userId = player.UserId;

        if (_banned.Contains(userId))
            return;

        var now = _timing.RealTime;
        var threshold = now - TimeSpan.FromSeconds(WindowSeconds);

        if (!_history.TryGetValue(userId, out var queue))
        {
            queue = new Queue<TimeSpan>();
            _history[userId] = queue;
        }

        while (queue.Count > 0 && queue.Peek() < threshold)
            queue.Dequeue();

        queue.Enqueue(now);

        if (queue.Count <= LimitPerWindow)
            return;

        _banned.Add(userId);
        _history.Remove(userId);

        IssueBan(player);
    }

    private async void IssueBan(ICommonSession player)
    {
        try
        {
            var banningAdmin = await ResolveBanningAdminAsync();

            (IPAddress, int)? targetIp = null;
            ImmutableTypedHwid? targetHwid = null;

            var sessionData = await _locator.LookupIdAsync(player.UserId);
            if (sessionData != null)
            {
                if (sessionData.LastAddress is not null)
                {
                    var prefix = sessionData.LastAddress.AddressFamily == AddressFamily.InterNetwork ? 32 : 64;
                    targetIp = (sessionData.LastAddress, prefix);
                }

                targetHwid = sessionData.LastHWId;
            }

            _banManager.CreateServerBan(
                player.UserId,
                player.Name,
                banningAdmin,
                targetIp,
                targetHwid,
                null,
                NoteSeverity.High,
                BanReason);
        }
        catch
        {

        }
    }

    private async Task<NetUserId?> ResolveBanningAdminAsync()
    {
        if (_banningAdminId.HasValue)
            return _banningAdminId;

        if (_banningAdminLookupFailed)
            return null;

        var data = await _locator.LookupIdByNameAsync(BanningAdminName);
        if (data == null)
        {
            _banningAdminLookupFailed = true;
            return null;
        }

        _banningAdminId = data.UserId;
        return _banningAdminId;
    }
}

