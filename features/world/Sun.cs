using Godot;

namespace IslebyIsle.features.world;

public partial class Sun : DirectionalLight3D
{
	[Export] public float TiltDegrees = 25f;
	[Export] public float MaxEnergy = 1f;

	public override void _Process(double delta)
	{
		float progress = DayNightClock.Instance.GetDayProgress();
		float pitch = Mathf.DegToRad((progress - WorldConstants.NoonFraction) * 360f - 90f);
		float tilt = Mathf.DegToRad(TiltDegrees);

		Basis = new Basis(Vector3.Back, tilt) * new Basis(Vector3.Right, pitch);
		float sunHeight = Basis.Z.Y;
		LightEnergy = MaxEnergy * Mathf.SmoothStep(0f, 0.2f, sunHeight);
	}
}
