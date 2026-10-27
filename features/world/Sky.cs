using System.Buffers.Text;
using Godot;
using IslebyIsle.features.world;

public partial class Sky : WorldEnvironment
{
	[Export] public Color DayTopColor;
	[Export] public Color DayHorizonColor;
	[Export] public Color DayBottomColor;
	[Export] public Color DawnTopColor;
	[Export] public Color DawnHorizonColor;
	[Export] public Color DawnBottomColor;
	[Export] public Color EveningTopColor;
	[Export] public Color EveningHorizonColor;
	[Export] public Color EveningBottomColor;
	[Export] public Color NightTopColor;
	[Export] public Color NightHorizonColor;
	[Export] public Color NightBottomColor;
	private DayNightClock _dayNightClock;
	[Export] public ProceduralSkyMaterial  ProceduralSkyMaterial;

	public override void _Ready()
	{
		_dayNightClock = DayNightClock.Instance;
	}
	
	public override void _Process(double delta)
	{
		if (_dayNightClock.TimeOfDay == TimeOfDay.Dawn)
		{
			var progress = (_dayNightClock.GetDayProgress() - WorldConstants.Dawn/WorldConstants.DayLengthSeconds) / WorldConstants.LengthOfDawnPerDay;
			progress = Mathf.Clamp(progress, 0f, 1f);
			SetProgressingColor(NightTopColor, DawnTopColor, DayTopColor, NightHorizonColor, DawnHorizonColor, DayHorizonColor,  NightBottomColor, DawnBottomColor, DayBottomColor, progress);
		} 
		else if (_dayNightClock.TimeOfDay == TimeOfDay.Evening)
		{
			var progress = (_dayNightClock.GetDayProgress() - WorldConstants.Evening/WorldConstants.DayLengthSeconds) / WorldConstants.LengthOfEveningPerDay;
			progress = Mathf.Clamp(progress, 0f, 1f);
			SetProgressingColor(DayTopColor, EveningTopColor, NightTopColor, DayHorizonColor, EveningHorizonColor, NightHorizonColor,  DayBottomColor, EveningBottomColor, NightBottomColor, progress);
		} else if (_dayNightClock.TimeOfDay == TimeOfDay.Night)
		{
			SetColor(NightTopColor, NightHorizonColor, NightBottomColor);
		}
		else if (_dayNightClock.TimeOfDay == TimeOfDay.Day)
		{
			SetColor(DayTopColor, DayHorizonColor,  DayBottomColor);
		}
	}
	
	private void SetProgressingColor(Color firstTopColor, Color midTopColor, Color lastTopColor,
		Color firstHorizonColor, Color midHorizonColor, Color lastHorizonColor,
		Color firstBottomColor, Color midBottomColor, Color lastBottomColor, float progress)
	{
		Color topColor, horizonColor, bottomColor;
		if (progress <= 0.5)
		{
			topColor = firstTopColor.Lerp(midTopColor, progress*2);
			horizonColor = firstHorizonColor.Lerp(midHorizonColor, progress * 2);
			bottomColor = firstBottomColor.Lerp(midBottomColor, progress*2);
		}
		else
		{
			topColor = midTopColor.Lerp(lastTopColor, (progress-0.5f)*2);
			horizonColor =  midHorizonColor.Lerp(lastHorizonColor, (progress-0.5f) * 2);
			bottomColor = midBottomColor.Lerp(lastBottomColor, (progress-0.5f)*2);
		}
		ProceduralSkyMaterial.SkyTopColor = topColor;
		ProceduralSkyMaterial.SkyHorizonColor = horizonColor;
		ProceduralSkyMaterial.GroundHorizonColor = horizonColor;
		ProceduralSkyMaterial.GroundBottomColor = bottomColor;
	}

	private void SetColor(Color topColor, Color horizonColor, Color bottomColor)
	{
		ProceduralSkyMaterial.SkyTopColor = topColor;
		ProceduralSkyMaterial.SkyHorizonColor = horizonColor;
		ProceduralSkyMaterial.GroundHorizonColor = horizonColor;
		ProceduralSkyMaterial.GroundBottomColor = bottomColor;
	}

}
