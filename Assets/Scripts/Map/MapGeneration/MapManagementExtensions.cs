using UnityEngine;

public static class MapManagementExtensions
{
    public static float MapX(this Transform transform)
    {
        return transform.position.x + MapManager.Map.SizeX / 2F;
    }

    public static float MapY(this Transform transform)
    {
        return transform.position.y + MapManager.Map.SizeY / 2F;
    }

    public static bool InMapRegion(this Transform transform)
    {
        return transform.MapY() >= 0 && transform.MapY() <= MapManager.Map.SizeY && transform.MapX() >= 0 && transform.MapX() <= MapManager.Map.SizeX;
    }
}