using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(MeshFilter))]
public class MapGenerator : MonoBehaviour
{
    public int seed;
    public bool useRandomSeed;

    public int width;
    public int height;

    [Range(0,100)]
    public int randomFillPercent;

    int[,] map;

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

    private void GenerateMap()
    {
        map = new int[width, height];
        RandomFillMap();

        for (int i = 0; i < 5; i++)
        {
            SmoothMap();
        }

        GetComponent<MeshFilter>().mesh = MeshGenerator.GenerateMesh(map, 1);
    }

    private void RandomFillMap()
    {
        if (useRandomSeed)
        {
            seed = (int)Time.time;
        }

        System.Random random = new System.Random(seed);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    map[x, y] = 1;
                }
                else
                {
                    map[x, y] = random.Next(0, 100) < randomFillPercent ? 1 : 0;
                }
            }
        }
    }

    // TODO try reversing loop to avoid patterns
    private void SmoothMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighborWallTilesCount = GetSurroundingWallCount(x, y);

                if (x == 0 || x == width - 1 || y == 0 || y == height - 1 || neighborWallTilesCount > 4)
                {
                    map[x, y] = 1;
                }
                else if (neighborWallTilesCount < 4)
                {
                    map[x, y] = 0;
                }
            }
        }
    }

    private int GetSurroundingWallCount(int gridX, int gridY)
    {
        return map.GetNeighbors(gridX, gridY).Count(a => a == 1);
    }

    private void OnDrawGizmos()
    {
        //if (map != null)
        //{
        //    for (int x = 0; x < width; x++)
        //    {
        //        for (int y = 0; y < height; y++)
        //        {
        //            Gizmos.color = map[x, y] == 1 ? Color.black : Color.white;
        //            Vector2 pos = new Vector2(-width / 2 + x + 0.5f, -height / 2 + y + 0.5f);
        //            Gizmos.DrawCube(pos, Vector2.one);
        //        }
        //    }
        //}
    }
}
