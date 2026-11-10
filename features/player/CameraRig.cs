using Godot;

public partial class CameraRig : Node3D
{
	[Export] public Node3D Target;
	[Export] public Camera3D Camera;
	[Export] public float FollowSpeed = 6f;
	[Export] public float ZoomStep = 0.9f;
	[Export] public float RotateSpeed = 10f;

	[Export] public float MinZoom = 6f;
	[Export] public float MaxZoom = 24f;
	[Export] public float RotateDuration = 0.4f;

	private float _yaw = Mathf.DegToRad(45f);
	private Tween _rotateTween;
	private bool _following = true;
	private bool _dragging;
	private Vector3 _lastTargetPos;
	private int _viewStep = 0; 
	
	public override void _Ready()
	{
		if (Target != null)
		{
			GlobalPosition = new Vector3(Target.GlobalPosition.X, GlobalPosition.Y, Target.GlobalPosition.Z);
		}
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event.IsActionPressed("camera_rotate_left"))
		{
			RotateView(-1);
		}
		else if (@event.IsActionPressed("camera_rotate_right"))
		{
			RotateView(1);
		}
		else if (@event.IsActionPressed("camera_zoom_in"))
		{
			Camera.Size = Mathf.Clamp(Camera.Size * ZoomStep, MinZoom, MaxZoom);
		}
		else if (@event.IsActionPressed("camera_zoom_out"))
		{
			Camera.Size = Mathf.Clamp(Camera.Size / ZoomStep, MinZoom, MaxZoom);
		}
	}

	public override void _Process(double delta)
	{
		float dt = (float)delta;
		
		if (Target != null)
		{
			var goal = new Vector3(Target.GlobalPosition.X, GlobalPosition.Y, Target.GlobalPosition.Z);
			GlobalPosition = GlobalPosition.Lerp(goal, 1f - Mathf.Exp(-FollowSpeed * dt));
		}
	}
	
	private void RotateView(int direction)
	{
		_viewStep += direction;
		float targetYaw = Mathf.DegToRad(45f + 90f * _viewStep);

		_rotateTween?.Kill();
		_rotateTween = CreateTween();
		_rotateTween.SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.InOut);
		_rotateTween.TweenMethod(Callable.From<float>(SetYaw), _yaw, targetYaw, RotateDuration);
	}

	private void SetYaw(float value)
	{
		_yaw = value;
		Rotation = new Vector3(Rotation.X, _yaw, Rotation.Z);
	}
}
