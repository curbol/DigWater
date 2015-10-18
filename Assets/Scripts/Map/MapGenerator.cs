using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(MeshFilter))]
public class MapGenerator : MonoBehaviour
{
    private SoilNode[,] map;

    public int seed;
    public int sizeX;
    public int sizeY;

    [Range(.1f, 2f)]
    public float mapScale = 1;

    private float Width
    {
        get
        {
            return sizeX * mapScale;
        }
    }

    private float Height
    {
        get
        {
            return sizeY * mapScale;
        }
    }

    [Range(25, 75)]
    public int randomFillPercent;

    [Range(0, 10)]
    public int borderThickness;

    public void GenerateMap()
    {
        transform.localScale = Vector3.one * mapScale;

        map = BuildMap();
        DrawMesh();
    }

    private void Start()
    {
        GenerateMap();
    }

    private void Update()
    {
        if (Input.GetMouseButton((int)MouseButton.Left))
        {
            Dig();
            DrawMesh();
        }
    }

    private SoilNode[,] BuildMap()
    {
        SoilNode[,] map = new SoilNode[sizeX, sizeY];
        SoilType[,] soilTypeMap = new SoilType[sizeX, sizeY];

        soilTypeMap.RandomFill(SoilType.Dirt, randomFillPercent, seed);
        soilTypeMap.SetBorder(SoilType.Dirt, borderThickness);
        soilTypeMap.Smooth(SoilType.Dirt, SoilType.Default, seed);

        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                map[x, y] = new SoilNode(x, y, soilTypeMap[x, y]);
            }
        }

        return map;
    }

    private void DrawMesh()
    {
        bool[,] bitMap = new bool[sizeX, sizeY];

        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                bitMap[x, y] = map[x, y].SoilType == SoilType.Dirt;
            }
        }

        GetComponent<MeshFilter>().mesh = MeshGenerator.GenerateMarchingSquaresMesh(bitMap);
    }

    private void Dig()
    {
        Vector2 positionToDig = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        SoilNode nodeToDig = GetNodeFromPosition(positionToDig);

        nodeToDig.SoilType = SoilType.Default;

        foreach (SoilNode neighbor in map.GetNeighbors(nodeToDig.X, nodeToDig.Y))
        {
            neighbor.SoilType = SoilType.Default;
        }
    }

    // TODO: Get rid of SoilNode, keep SoilType, create Coordinate class?
    private SoilNode GetNodeFromPosition(Vector2 position)
    {
        int x = Mathf.RoundToInt((sizeX - 1) / 2f + position.x);
        int y = Mathf.RoundToInt((sizeY - 1) / 2f + position.y);
        x = Mathf.Clamp(x, 0, sizeX - 1);
        y = Mathf.Clamp(y, 0, sizeY - 1);

        return map[x, y];
    }
}