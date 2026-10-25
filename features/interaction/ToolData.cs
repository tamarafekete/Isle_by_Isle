using Godot;
using IslebyIsle.features.inventory;

namespace IslebyIsle.features.interaction;

[GlobalClass]
public partial class ToolData : ItemData
{
	[Export] public ToolTier Tier { get; set; }
	[Export] public ToolType ToolType  { get; set; }

	public ToolData()
	{
		Tier = ToolTier.Hand;
		ToolType = ToolType.None;
		Name = "Hand";
		Type = ItemType.Tool;
	}
}
