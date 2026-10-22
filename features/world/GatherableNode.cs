using Godot;
using IslebyIsle.features.interaction;

namespace IslebyIsle.features.world;

public abstract partial class GatherableNode : StaticBody3D, IInteractable
{
	[Export] public GatherableData Data;

	private double _hitsRemaining = Constants.FullProgress;
	private bool _isRespawning = false;
	public double HitsRemaining => _hitsRemaining;
	private Timer _respawnTimer;
	[Export] public Node3D Mesh;
	[Export] public CollisionShape3D Collision;
	[Export] public Area3D Area;

	public override void _Ready()
	{
		Mesh.Visible = true;
		Collision.Disabled = false;
		_respawnTimer = new Timer();
		_respawnTimer.OneShot = true;
		AddChild(_respawnTimer);
		_respawnTimer.Timeout += OnRespawn;
		_respawnTimer.WaitTime = Data.RespawnSeconds;
		Area.BodyEntered += BodyEntered;
		Area.BodyExited += BodyExited;
	}

	public void Interact(Player player)
	{
		if (!_isRespawning && player != null)
		{
			if(Data.ToolType != ToolType.None && player.EquippedTool.Type != Data.ToolType)
			{
				player.WriteError("Can't interact with other tools");
				return;
			}

			if (player.EquippedTool.Tier < Data.MinToolTier)
			{
				player.WriteError("Tool is not strong enough");
				return;
			}

			if (Data.HitsRequired.TryGetValue(player.EquippedTool.Tier, out var hitNeeded))
			{
				_hitsRemaining-=(Constants.FullProgress/hitNeeded);
				if (_hitsRemaining <= 0)
				{
					GD.Print("You got the resource");
					player.Inventory.AddItem(Data.ItemGiven, Data.AmountGiven);
					player.Inventory.PrintInventory();
					OnHarvested();
					_isRespawning = true;
					_respawnTimer.Start();
				}
			}
			else
			{
				player.WriteError($"No hit data configured for tier {player.EquippedTool.Tier}");
			}
			
		}
	}

	protected void OnRespawn()
	{
		Mesh.Visible = true;
		Collision.Disabled = false;
		_hitsRemaining = Constants.FullProgress;
		_isRespawning = false;
	}

	protected virtual void OnHarvested()
	{
		Mesh.Visible = false;
		Collision.Disabled = true;
	}

	public string GetPrompt()
	{
		return _isRespawning ? "" : $"Press E to gather ({Data.ItemGiven.Name})";
	}
	
	protected virtual void BodyEntered(Node3D body)
	{
		if (body is Player player)
		{
			player.RegisterInteractable(this);
		}
	}
	
	protected virtual void BodyExited(Node3D body)
	{
		if (body is Player player)
		{
			player.UnregisterInteractable(this);
		}
	}
}
