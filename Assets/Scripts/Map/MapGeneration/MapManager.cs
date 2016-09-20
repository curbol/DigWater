using System;
using UnityEngine;

public class MapManager : Singleton<MapManager>
{
    [SerializeField]
    private bool showGizmos;

    [SerializeField]
    private int mapIndex;

    [SerializeField]
    private Map[] maps;

    public static Map Map
    {
        get
        {
            return Instance.maps != null && Instance.maps.Length > Instance.mapIndex ? Instance.maps[Instance.mapIndex] : null;
        }
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos)
            return;

        for (int x = 0; x < Map.SizeX; x++)
        {
            for (int y = 0; y < Map.SizeY; y++)
            {
                if (Map[x, y] == SoilType.None)
                    continue;

                int hash = Enum.GetName(typeof(SoilType), Map[x, y]).GetHashCode();
                float red = hash * 257 % 256 / 255F;
                float blue = hash * 313 % 256 / 255F;
                float green = hash * 379 % 256 / 255F;

                Gizmos.color = new Color(red, blue, green, 1F);
                Gizmos.DrawCube(Map.GetPositionFromCoordinate(x, y), Vector2.one * 0.2F);
                Gizmos.DrawWireCube(Map.GetPositionFromCoordinate(x, y), Vector2.one * 0.98F);
            }
        }
    }
}