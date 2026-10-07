using Godot;
using IslebyIsle.features.interaction;
using System;
using System.Collections.Generic;

public partial class Player : CharacterBody3D
{
	[Export]
	public float Speed = 5.0f;
	[Export]
	public float Deceleration = 20f;
	[Export]
	public Node3D CameraRig { get; set; }

	public Tool EquippedTool { get; set; } = new Tool();
	public readonly List<IInteractable> nearbyInteractables = new();
	[Export] public Label InteractionLabel;

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}
		
		

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
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
			Rotation = new Vector3(Rotation.X, Mathf.LerpAngle(Rotation.Y, targetAngle, 10f * (float)delta), Rotation.Z);
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
	}
	
	private IInteractable GetNearestInteractable()
	{
		double closest = Constants.MaxInteractDistance;
		IInteractable closestInteractable = null;
		foreach (IInteractable interactable in nearbyInteractables)
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
		if (!nearbyInteractables.Contains(interactable))
		{
			nearbyInteractables.Add(interactable);
		}
	}

	public void UnregisterInteractable(IInteractable interactable)
	{
		if (nearbyInteractables.Contains(interactable))
		{
			nearbyInteractables.Remove(interactable);
		}
	}
}
