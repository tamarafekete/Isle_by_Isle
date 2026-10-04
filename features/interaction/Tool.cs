using Godot;

namespace IslebyIsle.features.interaction;

public class Tool
{
    public ToolTier Tier { get; set; }
    public ToolType Type  { get; set; }
    public string Name { get; set; }

    public Tool()
    {
        Tier = ToolTier.Hand;
        Type = ToolType.None;
        Name = "None";
    }
}