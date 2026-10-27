using Godot;

namespace IslebyIsle.features.world;

public partial class DayNightClock : Node
{
    [Signal] public delegate void DawnStartedEventHandler(int dayNumber);
    
    private float _timeElapsedToday = WorldConstants.StartOfDayFirst;
    private int _currentDay = 1;
    public TimeOfDay TimeOfDay = TimeOfDay.Night;
    public static DayNightClock Instance;

    public override void _Ready()
    {
        Instance = this;
    }
    
    public override void _Process(double delta)
    {
        _timeElapsedToday += (float)delta;
        if (_timeElapsedToday >= WorldConstants.DayLengthSeconds)
        {
            _timeElapsedToday = 0;
            _currentDay++;
        }

        if (_timeElapsedToday >= WorldConstants.Dawn && TimeOfDay == TimeOfDay.Night)
        {
            TimeOfDay = TimeOfDay.Dawn;
            EmitSignalDawnStarted(_currentDay);
        }

        if (_timeElapsedToday >= WorldConstants.Day && TimeOfDay == TimeOfDay.Dawn)
        {
            TimeOfDay = TimeOfDay.Day;
        } 

        if (_timeElapsedToday >= WorldConstants.Evening && TimeOfDay == TimeOfDay.Day)
        {
            TimeOfDay =  TimeOfDay.Evening;
        }

        if (_timeElapsedToday >= WorldConstants.Night && TimeOfDay == TimeOfDay.Evening)
        {
            TimeOfDay = TimeOfDay.Night;
        }
    }

    public float GetDayProgress()
    {
        return _timeElapsedToday / WorldConstants.DayLengthSeconds;
    }

}