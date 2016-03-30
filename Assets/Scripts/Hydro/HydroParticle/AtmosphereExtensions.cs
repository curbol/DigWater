using UnityEngine;

public static class AtmosphereExtensions
{
    public static bool InCloudZone(this Transform transform)
    {
        return transform.MapY() >= HydroManager.GetProperties<CondensationProperties>().CloudLevelLowerBound && transform.MapY() <= HydroManager.GetProperties<CondensationProperties>().CloudLevelUpperBound;
    }
}