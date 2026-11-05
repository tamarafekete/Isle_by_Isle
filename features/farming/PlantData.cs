using Godot;
using Godot.Collections;

namespace IslebyIsle.features.farming;

[GlobalClass]
public partial class PlantData : Resource
{
    [Export] public string Name { get;set; }
    [Export] public Dictionary<GrowthStage, int> StageLengths = new();
    [Export] public Dictionary<GrowthStage, PackedScene> StageScenes = new();
    [Export] public CropData CropData { get; set; }
    [Export] public int MaxCrops { get; set; }
    [Export] public int MaxSeeds { get; set; }
}