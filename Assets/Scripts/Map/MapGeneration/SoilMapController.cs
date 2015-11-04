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

        foreach (SoilMetadata soil in SoilMap.Soils)
        {
            SoilMap.SoilGrid.RandomFill(soil.SoilType, soil.PercentCoverage, SoilMap.Seed + (int)soil.SoilType);
        }

        SoilMap.SoilGrid.SetBorder(SoilType.Dirt, SoilMap.BorderThickness);

        Smooth(SoilMap.SoilGrid, SoilMap.Seed);
    }

    public void RedrawSoilMesh()
    {
        if (SoilMap == null || SoilMap.SoilGrid == null)
            return;

        foreach (SoilMetadata soil in SoilMap.Soils)
        {
            bool[,] bitMap = SoilMap.SoilGrid.GetSoilBitMap(soil.SoilType);
            MeshData meshData = bitMap.GetMarchingSquaresMeshData(SoilMap.Width, SoilMap.Height);
            GameObject meshHolder = GetUniqueChildGameObject(transform, soil.SoilType.ToString());
            meshHolder.AddComponent<MeshFilter>().sharedMesh = meshData.GetMesh();
            meshHolder.AddComponent<MeshRenderer>().materials = new Material[] { soil.Material };

            if (soil.IsCollidable)
            {
                CreateEdgeColliders(meshHolder.transform, meshData, soil.PhysicsMaterial);
            }
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

    private static T[,] Smooth<T>(T[,] map, int seed = 0)
    {
        map.RandomSmoothPass(seed);
        map.CornerSmoothPass(SquareVertex.TopLeft);
        map.CornerSmoothPass(SquareVertex.BottomRight);
        map.CornerSmoothPass(SquareVertex.TopRight);
        map.CornerSmoothPass(SquareVertex.BottomLeft);

        return map;
    }

    private void Awake()
    {
        GenerateSoilMap();
        RedrawSoilMesh();
    }

    private void CreateEdgeColliders(Transform parent, MeshData meshData, PhysicsMaterial2D physicsMaterial = null)
    {
        GameObject edgeColliderHolder = GetUniqueChildGameObject(parent, "Edge Colliders");

        foreach (Vector2[] edgePoints in meshData.GetMeshEdges())
        {
            EdgeCollider2D edgeCollider = edgeColliderHolder.AddComponent<EdgeCollider2D>();
            edgeCollider.points = edgePoints.Select(e => e * SoilMap.Scale).ToArray();

            if (physicsMaterial != null)
            {
                edgeCollider.sharedMaterial = physicsMaterial;
            }
        }
    }
}