using System.Diagnostics.CodeAnalysis;
using Content.Shared.Preferences;
using JetBrains.Annotations;
using Robust.Shared.Localization;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using Robust.Shared.Utility;

namespace Content.Shared.Roles;

[UsedImplicitly]
[Serializable, NetSerializable]
public sealed partial class AnyOfRequirement : JobRequirement
{
    [DataField(required: true)]
    public List<JobRequirement> Requirements = new();

    public override bool Check(IEntityManager entManager,
        IPrototypeManager protoManager,
        HumanoidCharacterProfile? profile,
        IReadOnlyDictionary<string, TimeSpan> playTimes,
        [NotNullWhen(false)] out FormattedMessage? reason)
    {
        var reasons = new List<FormattedMessage>();
        var hasDiscordRequirement = false;
        
        foreach (var requirement in Requirements)
        {
            if (requirement is Content.Shared._Amour.Discord.DiscordLinkRequirement)
            {
                hasDiscordRequirement = true;
            }
            
            if (requirement.Check(entManager, protoManager, profile, playTimes, out var subReason))
            {
                reason = null;
                return !Inverted;
            }

            if (subReason != null && requirement is not Content.Shared._Amour.Discord.DiscordLinkRequirement)
                reasons.Add(subReason);
        }

        if (!Inverted)
        {
            reason = new FormattedMessage();

            if (hasDiscordRequirement && reasons.Count > 0)
            {
                reason.AddText(Loc.GetString("role-timer-discord-or-playtime"));
                reason.PushNewline();
                
                for (var i = 0; i < reasons.Count; i++)
                {
                    if (i > 0)
                        reason.PushNewline();
                    reason.AddMessage(reasons[i]);
                }
            }
            else if (reasons.Count > 0)
            {
                for (var i = 0; i < reasons.Count; i++)
                {
                    if (i > 0)
                        reason.PushNewline();
                    reason.AddMessage(reasons[i]);
                }
            }
            else
            {
                reason.AddText(Loc.GetString("role-timer-any-of-requirement-failed"));
            }

            return false;
        }

        reason = null;
        return true;
    }
}
