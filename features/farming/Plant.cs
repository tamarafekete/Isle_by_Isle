using System;
using Godot;
using IslebyIsle.features.interaction;

namespace IslebyIsle.features.farming;

public partial class Plant : Node3D, IInteractable
{
    [Export] public Area3D Area { get; set; }
    private Node3D _currentStageVisual;
    private PlantData PlantData  { get; set; }
    private SeedData SeedData { get; set; }
    private GrowthStage GrowthStage  { get; set; } = GrowthStage.Planted;
    private int DaysPassed { get; set; } = 0;
    private readonly Random _random = new Random(); 
    [Signal] public delegate void HarvestedEventHandler(Vector3 coordinates);
    private FarmSystem _farmSystem;

    public Plant()
    {
    }

    public Plant(SeedData seedData)
    {
        PlantData = seedData.Plant;
        SeedData = seedData;
        Area = new Area3D
        {
            CollisionLayer = 0,
            CollisionMask = 1u << 0 
        };

        Area.AddChild(new CollisionShape3D
        {
            Shape = new SphereShape3D { Radius = 1.5f }
        });
        AddChild(Area);

        Area.BodyEntered += BodyEntered;
        Area.BodyExited += BodyExited;

    }

    public override void _Ready()
    {
        _currentStageVisual = PlantData.StageScenes[GrowthStage.Planted].Instantiate<Node3D>();
        AddChild(_currentStageVisual);
        _farmSystem = FarmSystem.Instance;
    }

    public void AdvanceDay()
    {
        if (GrowthStage != GrowthStage.Mature)
        {
            DaysPassed++;
            if (DaysPassed >= PlantData.StageLengths[GrowthStage])
            {
                GrowthStage++;
                DaysPassed = 0;
                _currentStageVisual?.QueueFree();
                _currentStageVisual = PlantData.StageScenes[GrowthStage].Instantiate<Node3D>();
                AddChild(_currentStageVisual);
            }
        }
        
    }

    public void Interact(Player player)
    {
        if (player != null && GrowthStage == GrowthStage.Mature)
        {
            int cropCount = _random.Next(PlantData.MaxCrops)+1;
            if (player.Inventory.AddItem(PlantData.CropData, cropCount) > 0)
            {
                player.WriteError("Inventory Full");
            }
            int seedCount = _random.Next(PlantData.MaxSeeds)+1;
            if (player.Inventory.AddItem(SeedData, seedCount) > 0)
            {
                player.WriteError("Inventory Full");
            }
            _currentStageVisual?.QueueFree();
            _farmSystem.Harvested(GlobalPosition);
        }
    }
    
    public string GetPrompt() => GrowthStage == GrowthStage.Mature
        ? $"Press E to harvest ({PlantData.CropData.Name})"
        : "";
    
    public virtual void BodyEntered(Node3D body)
    {
        if (body is Player player)
        {
            player.RegisterInteractable(this);
        }
    }
	
    public virtual void BodyExited(Node3D body)
    {
        if (body is Player player)
        {
            player.UnregisterInteractable(this);
        }
    }
}