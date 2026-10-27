namespace IslebyIsle.features.world;

public static class WorldConstants
{
    public const float DayLengthSeconds = 600f;
    public const float Dawn = DayLengthSeconds * 0.25f;
    public const float Day = DayLengthSeconds * 0.375f;
    public const float Evening = DayLengthSeconds * 0.75f;
    public const float Night = DayLengthSeconds * 0.875f;
    public const float StartOfDayFirst = (Day + Dawn) / 2;
    public const float NoonFraction = (Day + Evening) / 2 / DayLengthSeconds;
    public const float LengthOfDawnPerDay = (Day-Dawn)/DayLengthSeconds;
    public const float LengthOfEveningPerDay = (Night - Evening) / DayLengthSeconds;
}