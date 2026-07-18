// Stub: custom animation interval player base class
using System;

public class IntervalAnimationPlayer
{
    public float startTime;
    public float endTime;
    public bool IsActive(float time) => time >= startTime && time <= endTime;
}

public class IntervalExcludeRepulsion : IntervalAnimationPlayer { }
