// Stub: animation player info passed as event data
using System;
using System.Collections.Generic;

public class ModelInfoAnimationPlayer
{
    public string animationName;
    public float speed = 1f;
    public List<IntervalAnimationPlayer> animationIntervals = new();
    public List<IntervalAnimationPlayer> GetIntervalExist(object type) => animationIntervals;
}
