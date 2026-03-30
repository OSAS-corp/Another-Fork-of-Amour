using System;
using System.Threading.Tasks;
using Content.Server.GameTicking.Events;
using Content.Shared.Roles;
using Robust.Server.Player;
using Robust.Shared.Enums;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;

namespace Content.Server._Amour.Discord;

public sealed class DiscordLinkRequirementSystem : EntitySystem
{
    [Dependency] private readonly IDiscordLinkChecker _discordLinkChecker = default!;
    [Dependency] private readonly IPlayerManager _playerManager = default!;
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;

    public override void Initialize()
    {
        base.Initialize();
        
        SubscribeLocalEvent<IsJobAllowedEvent>(OnIsJobAllowed);
        _playerManager.PlayerStatusChanged += OnPlayerStatusChanged;
    }

    public override void Shutdown()
    {
        base.Shutdown();
        _playerManager.PlayerStatusChanged -= OnPlayerStatusChanged;
    }

    private async void OnPlayerStatusChanged(object? sender, SessionStatusEventArgs e)
    {
        if (e.NewStatus != SessionStatus.Connected)
            return;

        try
        {
            await _discordLinkChecker.RefreshLinkStatusAsync(e.Session);
        }
        catch (Exception ex)
        {
            Log.Error($"Failed to refresh Discord link status for {e.Session.Name}: {ex}");
        }
    }

    private void OnIsJobAllowed(ref IsJobAllowedEvent ev)
    {
        if (!_prototypeManager.TryIndex<JobPrototype>(ev.JobId, out var job))
            return;

        var roleSystem = EntityManager.System<SharedRoleSystem>();
        var requirements = roleSystem.GetJobRequirement(job);
        
        if (requirements == null)
            return;

        foreach (var requirement in requirements)
        {
            if (requirement is DiscordLinkRequirement { Inverted: false })
            {
                var isLinked = _discordLinkChecker.IsDiscordLinkedCached(ev.Player.UserId);
                
                if (!isLinked)
                {
                    // Cache miss or not linked - do blocking async check
                    isLinked = _discordLinkChecker.IsDiscordLinkedAsync(ev.Player).GetAwaiter().GetResult();
                }
                
                if (!isLinked)
                {
                    ev.Cancelled = true;
                    return;
                }
            }
        }
    }
}
