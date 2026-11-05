using Godot;
using IslebyIsle.features.interaction;
using System.Collections.Generic;
using IslebyIsle.features.farming;
using IslebyIsle.features.inventory;
using IslebyIsle.features.world;

public partial class Player : CharacterBody3D
{
	[Export]
	public float Speed = 5.0f;
	[Export]
	public float Deceleration = 20f;
	[Export]
	public Node3D CameraRig { get; set; }

	public ToolData EquippedToolData;
	public ItemData EquippedItemData;
	public readonly List<IInteractable> NearbyInteractables = new();
	[Export] public Label InteractionLabel;
	[Export] public Label ErrorLabel;
	[Export] public ProgressBar GatherProgressBar;
	private Timer _errorTimer;
	public FarmSystem FarmSystem { get; set; }

	public Inventory Inventory { get; } = new Inventory(InventoryConstants.HotBarSize + InventoryConstants.BackPackSize);
	[Export] public InventoryUi InventoryUi { get; set; }
	private bool _inventoryOpen = false;

	public override void _Ready()
	{
		_errorTimer = new Timer();
		_errorTimer.OneShot = true;
		AddChild(_errorTimer);
		_errorTimer.Timeout += OnErrorEnded;
		_errorTimer.WaitTime = Constants.ErrorMessageLasts;
		InventoryUi.Initialize(Inventory);
		InventoryUi.InventoryPanel.Visible = false;
		EquippedToolData = InventoryUi.GetActiveTool();
		FarmSystem = FarmSystem.Instance;
		Inventory.InventoryChanged += RefreshEquippedItem;
	}

	private void ToggleInventory(bool toggle = true)
	{
		_inventoryOpen= toggle;
		InventoryUi.InventoryPanel.Visible = toggle;
	}
	
	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;
		
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		if (!_inventoryOpen)
		{
			Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_up", "move_down");
			Vector3 direction = new Vector3(inputDir.X, 0, inputDir.Y);
			if (CameraRig != null)
			{
				direction = direction.Rotated(Vector3.Up, CameraRig.Rotation.Y);
			}

			direction = direction.Normalized();
			if (direction != Vector3.Zero)
			{
				velocity.X = direction.X * Speed;
				velocity.Z = direction.Z * Speed;
			}
			else
			{
				velocity.X = Mathf.MoveToward(Velocity.X, 0, Deceleration * (float)delta);
				velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Deceleration * (float)delta);
			}

			if (direction.Length() > 0.1f)
			{
				float targetAngle = Mathf.Atan2(direction.X, direction.Z);
				Rotation = new Vector3(Rotation.X, Mathf.LerpAngle(Rotation.Y, targetAngle, 10f * (float)delta),
					Rotation.Z);
			}

			Velocity = velocity;
			MoveAndSlide();

			var closestInteractable = GetNearestInteractable();
			InteractionLabel.Text = closestInteractable?.GetPrompt() ?? "";

			if (Input.IsActionJustPressed("interact"))
			{
				if (closestInteractable != null)
				{
					closestInteractable.Interact(this);
				}
			}
		
			if (closestInteractable is GatherableNode gatherable && (gatherable.HitsRemaining < Constants.FullProgress && gatherable.HitsRemaining > 0))
			{
				Vector2 screenPos = GetViewport().GetCamera3D().UnprojectPosition(gatherable.GlobalPosition);
				GatherProgressBar.Position = screenPos;
				GatherProgressBar.Visible = true;
				GatherProgressBar.Value = 100.0 - gatherable.HitsRemaining; 
			}
			else
			{
				GatherProgressBar.Visible = false;
			}
			
			if (Input.IsActionJustPressed("use_farming_tool"))
			{
				GD.Print("Hello");
				var camera = GetViewport().GetCamera3D();
				var mousePos = GetViewport().GetMousePosition();

				var from = camera.ProjectRayOrigin(mousePos);
				var to = from + camera.ProjectRayNormal(mousePos) * 1000f;

				var query = PhysicsRayQueryParameters3D.Create(from, to);
				query.CollisionMask = 1u << 2; // only layer 3, the ground layer

				var result = GetWorld3D().DirectSpaceState.IntersectRay(query);
				if (result.Count > 0)
				{
					Vector3 hit = result["position"].AsVector3();
					GD.Print($"Clicked ground at x={hit.X}, z={hit.Z}, y={hit.Y}");
					if (EquippedToolData.ToolType == ToolType.Hoe && GlobalPosition.DistanceTo(hit) <= Constants.MaxInteractDistance)
					{
						FarmSystem.Till(hit);
					}
					else if (EquippedToolData.ToolType == ToolType.WateringCan &&
							 GlobalPosition.DistanceTo(hit) <= Constants.MaxInteractDistance)
					{
						FarmSystem.Water(hit);
					} else if (EquippedItemData is SeedData seed && GlobalPosition.DistanceTo(hit) <= Constants.MaxInteractDistance)
					{
						FarmSystem.Plant(seed, hit);
						Inventory.RemoveItem(seed, 1);
					}
				
				}
			}
		}

		if (Input.IsActionJustPressed("toggle_inventory"))
		{
			ToggleInventory();
		}

		if (Input.IsActionJustPressed("close_inventory"))
		{
			ToggleInventory(false);
		}

		for (Key i = Key.Key1; i <= Key.Key8; i++)
		{
			if (Input.IsKeyPressed(i))
			{
				InventoryUi.SetActive((int)i - (int)Key.Key1);
				RefreshEquippedItem();
			}
		}

	}

	private IInteractable GetNearestInteractable()
	{
		double closest = Constants.MaxInteractDistance;
		IInteractable closestInteractable = null;
		foreach (IInteractable interactable in NearbyInteractables)
		{
			if (GlobalPosition.DistanceTo(interactable.GlobalPosition) < closest)
			{
				closestInteractable = interactable;
				closest = GlobalPosition.DistanceTo(interactable.GlobalPosition);
			}
		}
		return  closestInteractable;
	}

	public void RegisterInteractable(IInteractable interactable)
	{
		if (!NearbyInteractables.Contains(interactable))
		{
			NearbyInteractables.Add(interactable);
		}
	}

	public void UnregisterInteractable(IInteractable interactable)
	{
		if (NearbyInteractables.Contains(interactable))
		{
			NearbyInteractables.Remove(interactable);
		}
	}
	
	public void WriteError(string message)
	{
		ErrorLabel.Text = message;
		_errorTimer.Start();
	}

	private void OnErrorEnded()
	{
		ErrorLabel.Text = string.Empty;
	}

	private void RefreshEquippedItem()
	{
		EquippedToolData = InventoryUi.GetActiveTool();
		EquippedItemData = InventoryUi.GetActiveItem();
	}
}
