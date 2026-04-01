using Content.Shared._Amour.Discord;
using Robust.Shared.Network;

namespace Content.Client._Amour.Discord;

public sealed class DiscordLinkSystem : EntitySystem
{
    [Dependency] private readonly INetManager _net = default!;
    [Dependency] private readonly DiscordLinkManager _linkManager = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeNetworkEvent<DiscordLinkStatusMsg>(OnStatusReceived);
    }

    private void OnStatusReceived(DiscordLinkStatusMsg message) { }

    public void RequestLinkCode()
    {
        _net.ClientSendMessage(new DiscordLinkRequestMsg());
    }
}
