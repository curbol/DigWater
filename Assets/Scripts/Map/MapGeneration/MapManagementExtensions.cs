using UnityEngine;

public static class MapManagementExtensions
{
    public static float MapX(this Transform transform)
    {
        return transform.position.MapX();
    }

    public static float MapX(this Vector3 position)
    {
        return ((Vector2)position).MapX();
    }

    public static float MapX(this Vector2 position)
    {
        return position.x + MapManager.Map.SizeX / 2F;
    }

    public static float MapY(this Transform transform)
    {
        return transform.position.MapY();
    }

    public static float MapY(this Vector3 position)
    {
        return ((Vector2)position).MapY();
    }

    public static float MapY(this Vector2 position)
    {
        return position.y + MapManager.Map.SizeY / 2F;
    }

    public static bool InMapRegion(this Transform transform)
    {
        return transform.position.InMapRegion();
    }

    public static bool InMapRegion(this Vector3 position)
    {
        return ((Vector2)position).InMapRegion();
    }

    public static bool InMapRegion(this Vector2 position)
    {
        return position.MapY() >= 0 && position.MapY() <= MapManager.Map.SizeY && position.MapX() >= 0 && position.MapX() <= MapManager.Map.SizeX;
    }

    public static bool InCloudRegion(this Transform transform)
    {
        return transform.position.InCloudRegion();
    }

    public static bool InCloudRegion(this Vector3 position)
    {
        return ((Vector2)position).InCloudRegion();
    }

    public static bool InCloudRegion(this Vector2 position)
    {
        return position.MapY() >= HydroManager.CloudProperties.CloudLevelLowerBound && position.MapY() <= HydroManager.CloudProperties.CloudLevelUpperBound;
    }
}