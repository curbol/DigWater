using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SoilMapController : MonoBehaviour
{
    [SerializeField]
    private SoilMap soilMap;
    public SoilMap SoilMap
    {
        get
        {
            return soilMap;
        }

        set
        {
            soilMap = value;
        }
    }

    public void GenerateSoilMap()
    {
        if (SoilMap == null)
            return;

        SoilMap.SoilGrid = new SoilType[SoilMap.SizeX, SoilMap.SizeY];

        SoilMap.SoilGrid.RandomFill(SoilType.Rock, SoilMap.PercentDirt, SoilMap.Seed+1);
        SoilMap.SoilGrid.RandomFill(SoilType.Dirt, SoilMap.PercentDirt, SoilMap.Seed);

        SoilMap.SoilGrid.SetBorder(SoilType.Dirt, SoilMap.BorderThickness);

        Smooth(SoilMap.SoilGrid, SoilMap.Seed);
    }

    public void RedrawSoilMesh()
    {
        if (SoilMap == null || SoilMap.SoilGrid == null)
            return;

        bool[,] dirtMap = SoilMap.SoilGrid.GetSoilBitMap(SoilType.Dirt);
        MeshData meshData = dirtMap.GetMarchingSquaresMeshData(SoilMap.Width, SoilMap.Height);
        GameObject dirtMeshHolder = GetUniqueChildGameObject(transform, "Dirt Mesh");
        dirtMeshHolder.AddComponent<MeshFilter>().sharedMesh = meshData.GetMesh();
        dirtMeshHolder.AddComponent<MeshRenderer>().materials = new Material[] { SoilMap.DirtMaterial };

        bool[,] rockMap = SoilMap.SoilGrid.GetSoilBitMap(SoilType.Rock);
        meshData = rockMap.GetMarchingSquaresMeshData(SoilMap.Width, SoilMap.Height);
        GameObject rockMeshHolder = GetUniqueChildGameObject(transform, "Rock Mesh");
        rockMeshHolder.AddComponent<MeshFilter>().sharedMesh = meshData.GetMesh();
        rockMeshHolder.AddComponent<MeshRenderer>().materials = new Material[] { SoilMap.RockMaterial };
        CreateEdgeColliders(meshData);
    }

    private void Awake()
    {
        GenerateSoilMap();
        RedrawSoilMesh();
    }

    private static T[,] Smooth<T>(T[,] map, int seed = 0)
    {
        map.RandomSmoothPass(seed);
        map.CornerSmoothPass(SquareVertex.TopLeft);
        map.CornerSmoothPass(SquareVertex.BottomRight);
        map.CornerSmoothPass(SquareVertex.TopRight);
        map.CornerSmoothPass(SquareVertex.BottomLeft);

        return map;
    }

    private void CreateEdgeColliders(MeshData meshData)
    {
        GameObject edgeColliderHolder = GetUniqueChildGameObject(transform, "Edge Colliders");

        foreach (Vector2[] edgePoints in meshData.GetMeshEdges())
        {
            EdgeCollider2D edgeCollider = edgeColliderHolder.AddComponent<EdgeCollider2D>();
            edgeCollider.sharedMaterial = SoilMap.DirtPhysics;
            edgeCollider.points = edgePoints.Select(e => e * SoilMap.Scale).ToArray();
        }
    }

    private static GameObject GetUniqueChildGameObject(Transform transform, string gameObjectName)
    {
        if (transform.FindChild(gameObjectName))
        {
            DestroyImmediate(transform.FindChild(gameObjectName).gameObject);
        }

        GameObject gameObjectHolder = new GameObject(gameObjectName);
        gameObjectHolder.transform.parent = transform;

        return gameObjectHolder;
    }
}