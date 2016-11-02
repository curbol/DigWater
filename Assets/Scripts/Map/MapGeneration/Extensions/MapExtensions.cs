using UnityEngine;

public static class MapExtensions
{
    public static Coordinate GetCoordinateFromPosition(this Map soilMap, Vector2 position)
    {
        int x = Mathf.RoundToInt((soilMap.SizeX - 1) / 2F + position.x / soilMap.Scale);
        int y = Mathf.RoundToInt((soilMap.SizeY - 1) / 2F + position.y / soilMap.Scale);
        x = Mathf.Clamp(x, 0, soilMap.SizeX - 1);
        y = Mathf.Clamp(y, 0, soilMap.SizeY - 1);

        return new Coordinate(x, y);
    }

    public static Vector2 GetPositionFromCoordinate(this Map soilMap, int x, int y)
    {
        return new Vector2((x + 0.5F) * soilMap.Scale - soilMap.Width / 2, (y + 0.5F) * soilMap.Scale - soilMap.Height / 2);
    }

    public static SoilType GetSoilTypeFromPosition(this Map soilMap, Vector2 position)
    {
        Coordinate mapCoordinate = soilMap.GetCoordinateFromPosition(position);
        return soilMap[mapCoordinate.X, mapCoordinate.Y];
    }

    public static void Draw(this Map soilMap, Coordinate drawCoordinate, SoilType soilType)
    {
        Draw(soilMap, drawCoordinate, soilType, 0.5F);
    }

    public static void Draw(this Map soilMap, Coordinate drawCoordinate, SoilType soilType, float radius)
    {
        if (soilMap == null)
            return;

        soilMap[drawCoordinate.X, drawCoordinate.Y] = soilType;
        foreach (Coordinate neighborCoordinate in GridArrayExtensions.GetNeighborCoordinatesInRadius(drawCoordinate.X, drawCoordinate.Y, soilMap.SizeX, soilMap.SizeY, radius))
        {
            soilMap[neighborCoordinate.X, neighborCoordinate.Y] = soilType;
        }
    }

    public static bool[,] GetSoilBitMap(this Map soilMap, SoilType soilType)
    {
        bool[,] bitMap = null;

        if (soilMap != null)
        {
            bitMap = new bool[soilMap.SizeX, soilMap.SizeY];
            for (int x = 0; x < soilMap.SizeX; x++)
            {
                for (int y = 0; y < soilMap.SizeY; y++)
                {
                    bitMap[x, y] = soilMap[x, y] == soilType;
                }
            }
        }

        return bitMap;
    }

    public static int[,] GetIntMap(this Map map)
    {
        int[,] intMap = null;

        if (map != null)
        {
            intMap = new int[map.SizeX, map.SizeY];
            for (int x = 0; x < map.SizeX; x++)
            {
                for (int y = 0; y < map.SizeY; y++)
                {
                    if (map[x, y] != SoilType.None)
                        intMap[x, y] = (int)map[x, y];
                }
            }
        }

        return intMap;
    }
}