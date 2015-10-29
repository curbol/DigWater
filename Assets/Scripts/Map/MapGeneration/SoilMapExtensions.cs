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

    public static bool[,] GetSoilBitMap(this SoilType[,] soilGrid, SoilType soilType)
    {
        bool[,] bitMap = null;

        if (soilGrid != null)
        {
            int sizeX = soilGrid.GetLength(0);
            int sizeY = soilGrid.GetLength(1);

            bitMap = new bool[sizeX, sizeY];
            for (int x = 0; x < sizeX; x++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    bitMap[x, y] = soilGrid[x, y] == soilType;
                }
            }
        }

        return bitMap;
    }
}