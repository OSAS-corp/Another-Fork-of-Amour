using System.Collections.Generic;
using Robust.Shared.GameObjects;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.Manager.Attributes;

namespace Content.Shared._Amour.Jukebox;

[RegisterComponent, NetworkedComponent]
public sealed partial class AmourTapeComponent : Component
{
    [DataField("songs")]
    public List<AmourJukeboxSong> Songs { get; set; } = new();
}

[Serializable, NetSerializable]
public sealed partial class AmourTapeComponentState : ComponentState
{
    public List<AmourJukeboxSong> Songs { get; set; } = new();
}
