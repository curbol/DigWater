using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(MeshFilter))]
public class MapGenerator : MonoBehaviour
{
    public int seed;
    public int mapWidth;
    public int mapHeight;

    [Range(.1f, 2f)]
    public float mapScale = 1;

    [Range(25, 75)]
    public int randomFillPercent;

    [Range(0, 10)]
    public int borderThickness;

    public void GenerateMap()
    {
        bool[,] map = new bool[mapWidth, mapHeight];

        transform.localScale = Vector3.one * mapScale;
        RandomFillMap(map, seed, randomFillPercent);
        SetMapBorder(map, borderThickness);
        SmoothMap(map, seed);

        GetComponent<MeshFilter>().mesh = MeshGenerator.GenerateMarchingSquaresMesh(map);
    }

    private void Start()
    {
        GenerateMap();
    }

    private static void RandomFillMap(bool[,] map, int seed, int randomFillPercent)
    {
        System.Random random = new System.Random(seed);

        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                map[x, y] = random.Next(0, 100) < randomFillPercent;
            }
        }
    }

    private static void SetMapBorder(bool[,] map, int borderThickness)
    {
        int mapWidth = map.GetLength(0);
        int mapHeight = map.GetLength(1);

        if (borderThickness  > 0 && mapWidth > borderThickness * 2 && mapHeight > borderThickness * 2)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapHeight; y++)
                {
                    map[x, y] = true;

                    if (x > borderThickness - 1 && x < mapWidth - borderThickness && y == borderThickness - 1)
                    {
                        y = mapHeight - borderThickness;
                    }
                }
            }
        }
    }

    private static void SmoothMap(bool[,] map, int seed)
    {
        RandomSmooth(map, seed);
        SmoothMapFromCorner(map, SquareVertex.TopLeft);
        SmoothMapFromCorner(map, SquareVertex.BottomRight);
        SmoothMapFromCorner(map, SquareVertex.TopRight);
        SmoothMapFromCorner(map, SquareVertex.BottomLeft);
    }

    private static void RandomSmooth(bool[,] map, int seed)
    {
        int[] rangeX = Enumerable.Range(0, map.GetLength(0) - 1).ToArray().Shuffle(seed);
        int[] rangeY = Enumerable.Range(0, map.GetLength(1) - 1).ToArray().Shuffle(seed);

        foreach (int x in rangeX)
        {
            foreach (int y in rangeY)
            {
                SmoothCoordinate(map, x, y);
            }
        }
    }

    private static void SmoothMapFromCorner(bool[,] map, SquareVertex corner)
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
                SmoothCoordinate(map, x, y);
            }
        }
    }

    private static void SmoothCoordinate(bool[,] map, int x, int y)
    {
        int mapWidth = map.GetLength(0);
        int mapHeight = map.GetLength(1);
        int neighborWallsCount = map.GetNeighbors(x, y).Count(a => a);

        if (x == 0 || x == mapWidth - 1 || y == 0 || y == mapHeight - 1 || neighborWallsCount > 4)
        {
            map[x, y] = true;
        }
        else if (neighborWallsCount < 4)
        {
            map[x, y] = false;
        }
    }
}