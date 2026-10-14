using Godot;

namespace IslebyIsle.features.inventory;

[GlobalClass]
public partial class ItemData : Resource
{
    [Export] public string Name { get; set; }
    [Export] public Texture2D Icon { get; set; }
    [Export] public ItemType Type { get; set; }
    [Export] public string Description { get; set; }
    [Export] public int MaxStackSize { get; set; } = 99;
}