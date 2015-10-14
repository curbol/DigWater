using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(MeshFilter))]
public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    private int seed;
    public int Seed
    {
        get
        {
            return seed;
        }

        set
        {
            seed = value;
        }
    }

    [SerializeField]
    private int width;
    public int Width
    {
        get
        {
            return width;
        }
    }

    [SerializeField]
    private int height;
    public int Height
    {
        get
        {
            return height;
        }
    }

    [SerializeField]
    [Range(.1f, 2f)]
    private float nodeSize = 1;
    public float NodeSize
    {
        get
        {
            return nodeSize;
        }
    }

    [SerializeField]
    [Range(25, 75)]
    private int randomFillPercent;
    public int RandomFillPercent
    {
        get
        {
            return randomFillPercent;
        }
    }

    public void GenerateMap()
    {
        bool[,] map = new bool[Width, Height];

        RandomFillMap(map, Seed, RandomFillPercent);
        SmoothMap(map);

        GetComponent<MeshFilter>().mesh = MeshGenerator.GenerateMarchingSquaresMesh(map, NodeSize);
    }

    private void Start()
    {
        GenerateMap();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown((int)MouseButton.Left))
        {
            GenerateMap();
        }
    }

    private static void RandomFillMap(bool[,] map, int seed, int randomFillPercent)
    {
        int width = map.GetLength(0);
        int height = map.GetLength(1);
        System.Random random = new System.Random(seed);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    map[x, y] = true;
                }
                else
                {
                    map[x, y] = random.Next(0, 100) < randomFillPercent;
                }
            }
        }
    }

    private static void SmoothMap(bool[,] map)
    {
        bool smoothForward = true;
        for (int i = 0; i < 5; i++)
        {
            if (smoothForward)
            {
                SmoothMapForward(map);
            }
            else
            {
                SmoothMapBackwards(map);
            }

            smoothForward = !smoothForward;
        }
    }

    private static void SmoothMapForward(bool[,] map)
    {
        int width = map.GetLength(0);
        int height = map.GetLength(1);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                SmoothCoordinate(map, x, y);
            }
        }
    }

    private static void SmoothMapBackwards(bool[,] map)
    {
        int width = map.GetLength(0);
        int height = map.GetLength(1);

        for (int x = width - 1; x >= 0; x--)
        {
            for (int y = height - 1; y >= 0; y--)
            {
                SmoothCoordinate(map, x, y);
            }
        }
    }

    private static void SmoothCoordinate(bool[,] map, int x, int y)
    {
        int width = map.GetLength(0);
        int height = map.GetLength(1);
        int neighborWallsCount = map.GetNeighbors(x, y).Count(a => a);

        if (x == 0 || x == width - 1 || y == 0 || y == height - 1 || neighborWallsCount > 4)
        {
            map[x, y] = true;
        }
        else if (neighborWallsCount < 4)
        {
            map[x, y] = false;
        }
    }
}