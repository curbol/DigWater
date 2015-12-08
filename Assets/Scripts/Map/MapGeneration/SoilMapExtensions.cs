using UnityEngine;

public static class SoilMapExtensions
{
    public static Coordinate GetCoordinateFromPosition(this SoilMap soilMap, Vector2 position)
    {
        int x = Mathf.RoundToInt((soilMap.SizeX - 1) / 2F + position.x / soilMap.Scale);
        int y = Mathf.RoundToInt((soilMap.SizeY - 1) / 2F + position.y / soilMap.Scale);
        x = Mathf.Clamp(x, 0, soilMap.SizeX - 1);
        y = Mathf.Clamp(y, 0, soilMap.SizeY - 1);

        return new Coordinate(x, y);
    }

    public static void Draw(this SoilMap soilMap, Coordinate drawCoordinate, SoilType soilType)
    {
        Draw(soilMap, drawCoordinate, soilType, 0.5F);
    }

    public static void Draw(this SoilMap soilMap, Coordinate drawCoordinate, SoilType soilType, float radius)
    {
        if (soilMap == null)
            return;

        soilMap[drawCoordinate.X, drawCoordinate.Y] = soilType;
        //foreach (Coordinate neighborCoordinate in soilMap.GetNeighborCoordinatesInRadius(drawCoordinate.X, drawCoordinate.Y, radius))
        //{
        //    soilMap[neighborCoordinate.X, neighborCoordinate.Y] = soilType;
        //}
    }

    public static bool[,] GetSoilBitMap(this SoilMap soilMap, SoilType soilType)
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
}