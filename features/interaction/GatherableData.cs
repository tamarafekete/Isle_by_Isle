using System.Reflection;
using Godot;
using Godot.Collections;
using IslebyIsle.features.inventory;

namespace IslebyIsle.features.interaction;

[GlobalClass]
public partial class GatherableData : Resource
{
    [Export] public ItemData ItemGiven;
    [Export] public ToolType ToolType = 0;
    [Export] public Dictionary<ToolTier, int> HitsRequired = new Dictionary<ToolTier, int>();
    [Export] public ToolTier MinToolTier = 0;
    [Export] public int AmountGiven = 1;
    [Export] public float RespawnSeconds = 60f;
}