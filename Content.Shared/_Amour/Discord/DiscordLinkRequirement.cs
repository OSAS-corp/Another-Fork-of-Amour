using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Content.Shared._Amour.Discord;
using Content.Shared.Preferences;
using JetBrains.Annotations;
using Robust.Shared.IoC;
using Robust.Shared.Localization;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using Robust.Shared.Utility;

namespace Content.Shared.Roles;

[UsedImplicitly]
[Serializable, NetSerializable]
public sealed partial class DiscordLinkRequirement : JobRequirement
{
    public override bool Check(IEntityManager entManager,
        IPrototypeManager protoManager,
        HumanoidCharacterProfile? profile,
        IReadOnlyDictionary<string, TimeSpan> playTimes,
        [NotNullWhen(false)] out FormattedMessage? reason)
    {
        var isLinked = false;
        try
        {
            var linkManager = IoCManager.Resolve<ISharedDiscordLinkManager>();
            isLinked = linkManager.IsLinked;
        }
        catch
        {
            // я хочу спать
        }

        if (!isLinked)
        {
            reason = FormattedMessage.FromMarkupPermissive(Loc.GetString("role-timer-discord-not-linked"));
            return false;
        }
        
        reason = null;
        return true;
    }
}
