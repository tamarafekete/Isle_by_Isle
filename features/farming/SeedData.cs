using Godot;
using IslebyIsle.features.inventory;

namespace IslebyIsle.features.farming;
[GlobalClass]
public partial class SeedData : ItemData
{
    [Export] public PlantData Plant { get; set; }
}