using System.Collections.Generic;
using System.Linq;

public static class MapCreationExtensions
{
    public static T[,] RandomFill<T>(this T[,] map, T value, int randomFillPercent, int seed = 0)
    {
        System.Random random = new System.Random(seed);

        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                if (random.Next(0, 100) < randomFillPercent)
                {
                    map[x, y] = value;
                }
            }
        }

        return map;
    }

    public static T[,] SetBorder<T>(this T[,] map, T value, int borderThickness)
    {
        int sizeX = map.GetLength(0);
        int sizeY = map.GetLength(1);

        if (borderThickness > 0 && borderThickness * 2 < sizeX && borderThickness * 2 < sizeY)
        {
            for (int x = 0; x < sizeX; x++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    if (x < borderThickness || x >= sizeX - borderThickness || y < borderThickness || y >= sizeY - borderThickness)
                    {
                        map[x, y] = value;
                    }
                }
            }
        }

        return map;
    }

    public static T[,] RandomSmoothPass<T>(this T[,] map, int seed = 0)
    {
        int[] rangeX = Enumerable.Range(0, map.GetLength(0) - 1).ToArray().Shuffle(seed);
        int[] rangeY = Enumerable.Range(0, map.GetLength(1) - 1).ToArray().Shuffle(seed);

        foreach (int x in rangeX)
        {
            foreach (int y in rangeY)
            {
                map.SmoothCoordinate(x, y);
            }
        }

        return map;
    }

    public static T[,] CornerSmoothPass<T>(this T[,] map, SquareVertex corner)
    {
        IEnumerable<int> rangeX = Enumerable.Range(0, map.GetLength(0) - 1);
        IEnumerable<int> rangeY = Enumerable.Range(0, map.GetLength(1) - 1);

        if (corner == SquareVertex.TopRight || corner == SquareVertex.BottomRight)
        {
            rangeX = rangeX.Reverse();
        }

        if (corner == SquareVertex.BottomLeft || corner == SquareVertex.BottomRight)
        {
            rangeY = rangeY.Reverse();
        }

        foreach (int x in rangeX)
        {
            foreach (int y in rangeY)
            {
                map.SmoothCoordinate(x, y);
            }
        }

        return map;
    }

    public static T[,] SmoothCoordinate<T>(this T[,] map, int x, int y)
    {
        int sizeX = map.GetLength(0);
        int sizeY = map.GetLength(1);
        T positiveValue = map[x, y];
        T negativeValue = map.GetNeighbors(x, y).Where(a => !a.Equals(map[x, y])).GroupBy(a => a).OrderByDescending(a => a.Count()).Select(a => a.Key).FirstOrDefault();
        int positiveValueCount = map.GetNeighbors(x, y).Count(a => a.Equals(positiveValue));

        if (x == 0 || x == sizeX - 1 || y == 0 || y == sizeY - 1 || positiveValueCount > 4)
        {
            map[x, y] = positiveValue;
        }
        else if (positiveValueCount < 4)
        {
            map[x, y] = negativeValue;
        }

        return map;
    }
}