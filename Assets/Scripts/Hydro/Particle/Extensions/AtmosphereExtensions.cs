﻿using UnityEngine;

public static class AtmosphereExtensions
{
    public static bool InCloudZone(this Transform transform)
    {
        return transform.MapY() >= CondensationManager.CloudLevelLowerBound && transform.MapY() <= CondensationManager.CloudLevelUpperBound;
    }
}