using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SoilMapController : MonoBehaviour
{
    private MeshFilter meshFilter;
    private MeshFilter MeshFilter
    {
        get
        {
            if (meshFilter == null)
            {
                meshFilter = GetComponent<MeshFilter>();
            }

            return meshFilter;
        }
    }

    private MeshRenderer meshRenderer;
    private MeshRenderer MeshRenderer
    {
        get
        {
            if (meshRenderer == null)
            {
                meshRenderer = GetComponent<MeshRenderer>();
            }

            return meshRenderer;
        }
    }

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
        SoilMap.SoilGrid.RandomFill(SoilType.Dirt, SoilMap.PercentDirt, SoilMap.Seed);
        SoilMap.SoilGrid.SetBorder(SoilType.Dirt, SoilMap.BorderThickness);
        Smooth(SoilMap.SoilGrid, SoilType.Dirt, SoilType.Default, SoilMap.Seed);
    }

    public void RedrawSoilMesh()
    {
        if (SoilMap == null || SoilMap.SoilGrid == null || MeshFilter == null)
            return;

        bool[,] dirtMap = SoilMap.SoilGrid.GetSoilBitMap(SoilType.Dirt);
        MeshData meshData = dirtMap.GetMarchingSquaresMeshData(SoilMap.Width, SoilMap.Height);
        MeshFilter.sharedMesh = meshData.GetMesh();
        MeshRenderer.materials = new Material[] { SoilMap.DirtMaterial };
        CreateEdgeColliders(meshData);
    }

    private void Awake()
    {
        GenerateSoilMap();
        RedrawSoilMesh();
    }

    private static T[,] Smooth<T>(T[,] map, T positiveValue, T negativeValue, int seed = 0)
    {
        map.RandomSmoothPass(positiveValue, negativeValue, seed);
        map.CornerSmoothPass(positiveValue, negativeValue, SquareVertex.TopLeft);
        map.CornerSmoothPass(positiveValue, negativeValue, SquareVertex.BottomRight);
        map.CornerSmoothPass(positiveValue, negativeValue, SquareVertex.TopRight);
        map.CornerSmoothPass(positiveValue, negativeValue, SquareVertex.BottomLeft);

        return map;
    }

    private void CreateEdgeColliders(MeshData meshData)
    {
        string edgeColliderHolderName = "Edge Colliders";
        if (transform.FindChild(edgeColliderHolderName))
        {
            DestroyImmediate(transform.FindChild(edgeColliderHolderName).gameObject);
        }

        GameObject edgeColliderHolder = new GameObject(edgeColliderHolderName);
        edgeColliderHolder.transform.parent = transform;

        foreach (Vector2[] edgePoints in meshData.GetMeshEdges())
        {
            EdgeCollider2D edgeCollider = edgeColliderHolder.AddComponent<EdgeCollider2D>();
            edgeCollider.sharedMaterial = SoilMap.DirtPhysics;
            edgeCollider.points = edgePoints.Select(e => e * SoilMap.Scale).ToArray();
        }
    }
}