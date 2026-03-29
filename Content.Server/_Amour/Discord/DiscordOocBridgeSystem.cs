using Robust.Shared.IoC;
using JetBrains.Annotations;

namespace Content.Server._Amour.Discord;

[UsedImplicitly]
public sealed class DiscordOocBridgeSystem : EntitySystem
{
    [Dependency] private readonly DiscordOocBridgeService _bridge = default!;

    public override void Initialize()
    {
        base.Initialize();

        _ = _bridge.StartAsync();
    }

    public override void Shutdown()
    {
        base.Shutdown();
        _bridge.StopAsync().Wait();
    }

    public void OnGameOocMessage(string playerName, string message)
    {
        _ = _bridge.SendToDiscordAsync(playerName, message);
    }
}
