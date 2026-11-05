using Godot;
using IslebyIsle.features.world;

namespace IslebyIsle.features.farming;

public partial class SoilTileData : Node, IGridOccupant
{
	public Plant Plant { get; set; }
	private bool _isWatered = false;
	public SoilState State { get; set; } = SoilState.Tilled;
	private Decal _decal;

	public void Till(PackedScene scene, Vector3 position)
	{
		ChangeSoil(scene, position);
		GD.Print($"Decal!s position at {position}");
	}

	public void UnTill()
	{
		_decal.QueueFree();
		QueueFree();
	}

	public void Water(PackedScene scene, Vector3 position)
	{
		ChangeSoil(scene, position);
		_isWatered = true;
	}

	public bool PlantSeed(SeedData seed, Vector3 position)
	{
		if (Plant == null)
		{
			State =SoilState.Planted;
			Plant = new Plant(seed);
			AddChild(Plant);
			Plant.GlobalPosition = position;
			return true;
		}
		return false;
	}

	public void AdvanceDay(PackedScene scene, Vector3 position)
	{
		if (_isWatered)
		{
			if (Plant != null)
			{
				Plant.AdvanceDay();
			}
			ChangeSoil(scene, position);
			_isWatered = false;
		}
	}

	public void ClearPlant()
	{
		Plant.QueueFree();
		Plant = null;
	}

	private void ChangeSoil(PackedScene scene, Vector3 position)
	{
		_decal?.QueueFree();
		_decal =  scene.Instantiate<Decal>();
		AddChild(_decal);
		_decal.GlobalPosition = position;
	}

	/*public void AdvanceDay()
	{
		if (PlantedSeed!= null && WateredToday && GrowthStage != GrowthStage.Mature)
		{
			DaysPassed++;
			if (DaysPassed >= PlantedSeed.Plant.StageLengths[GrowthStage])
			{
				GrowthStage++;
				DaysPassed = 0;
			}
		}
		WateredToday = false;
	}*/
}
