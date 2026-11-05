using System;
using Godot;
using System.Collections.Generic;
using IslebyIsle.features.farming;

namespace IslebyIsle.features.world;

public partial class WorldGrid : Node
{
    public static WorldGrid Instance;
    [Export] public float TileSize = 1f;

    private readonly Dictionary<Vector2I, IGridOccupant> _cells = new();

    public override void _Ready()
    {
        Instance ??= this;
    }
    public Vector2I WorldToTile(Vector3 worldPos)
    {
        var tilePos  = new Vector2I((int)Math.Floor(worldPos.X/TileSize), (int)Math.Floor(worldPos.Z/TileSize));
        return tilePos;
    }

    public Vector3 TileToWorld(Vector2I tile)
    {
        var centerX = TileSize * (2 * tile.X + 1) / 2;
        var centerZ = TileSize * (2 * tile.Y + 1) / 2;
        return new Vector3(centerX, 0, centerZ);
    }

    public bool IsOccupied(Vector2I tile) => _cells.ContainsKey(tile);
    public IGridOccupant GetOccupant(Vector2I tile) => _cells.GetValueOrDefault(tile);

    public Dictionary<Vector2I, IGridOccupant> GetOccupants()
    {
        return _cells;
    }
    
    public void SetOccupant(Vector2I tile, IGridOccupant occupant) => _cells[tile] = occupant;
    public void ClearOccupant(Vector2I tile) => _cells.Remove(tile);
    
}