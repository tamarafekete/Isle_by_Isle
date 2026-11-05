using Godot;
using IslebyIsle.features.world;

namespace IslebyIsle.features.farming;

public partial class FarmSystem : Node
{
	private WorldGrid _worldGrid;
	[Export] public PackedScene TilledSoilScene;
	[Export] public PackedScene WateredSoilScene;
	public static FarmSystem Instance { get; private set; }
	private DayNightClock _dayNightClock; 

	public override void _Ready()
	{
		_worldGrid = WorldGrid.Instance;
		Instance ??= this;
		_dayNightClock = DayNightClock.Instance;
		_dayNightClock.DawnStarted += AdvanceDay;
	}

	public void Till(Vector3 position)
	{
		GD.Print($"Tilling at {position}");
		Vector2I pos = _worldGrid.WorldToTile(position);
		if (_worldGrid.IsOccupied(pos))
		{
			if (_worldGrid.GetOccupant(pos) is SoilTileData { State: SoilState.Tilled } soil)
			{
				soil.UnTill();
				RemoveChild(soil);
				_worldGrid.ClearOccupant(pos);
			}
			
		}
		else
		{
			GD.Print($"Tilling at {pos}");
			var soilStateData = new SoilTileData();
			AddChild(soilStateData);
			soilStateData.Till(TilledSoilScene, _worldGrid.TileToWorld(pos));
			_worldGrid.SetOccupant(pos, soilStateData);
		}
	}
	
	public void Water(Vector3 position) {
		Vector2I pos = _worldGrid.WorldToTile(position);
		if (_worldGrid.GetOccupant(pos) is SoilTileData soil)
		{
			soil.Water(WateredSoilScene, _worldGrid.TileToWorld(pos));
		}
	}

	public void Plant(SeedData seed, Vector3 position)
	{
		Vector2I pos = _worldGrid.WorldToTile(position);
		if (_worldGrid.GetOccupant(pos) is SoilTileData { State: SoilState.Tilled } soil)
		{
			soil.PlantSeed(seed, _worldGrid.TileToWorld(pos));
		}
	}
	
	public void Harvested(Vector3 position)
	{
		Vector2I pos = _worldGrid.WorldToTile(position);
		if (_worldGrid.GetOccupant(pos) is SoilTileData soil)
		{
			soil.ClearPlant();
		}
	}

	private void AdvanceDay(int day)
	{
		var cells = _worldGrid.GetOccupants();
		foreach (var cell in cells)
		{
			if (cell.Value is SoilTileData soil)
			{
				soil.AdvanceDay(TilledSoilScene, _worldGrid.TileToWorld(cell.Key));
			}
		}
	}
	
}
