using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
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

    public void GenerateSoil()
    {
        if (SoilMap != null)
        {
            SoilMap.SoilGrid = new SoilType[SoilMap.SizeX, SoilMap.SizeY];

            SoilMap.SoilGrid.RandomFill(SoilType.Dirt, SoilMap.PercentDirt, SoilMap.Seed);
            SoilMap.SoilGrid.SetBorder(SoilType.Dirt, SoilMap.BorderThickness);
            SoilMap.SoilGrid.Smooth(SoilType.Dirt, SoilType.Default, SoilMap.Seed);
        }
    }

    public void DrawSoil()
    {
        if (SoilMap != null && SoilMap.SoilGrid != null && MeshFilter != null)
        {
            bool[,] dirtMap = SoilMap.SoilGrid.GetSoilBitMap(SoilType.Dirt);
            MeshData meshData = dirtMap.GetMarchingSquaresMesh();
            MeshFilter.mesh = meshData.Mesh;

            CreateEdgeColliders(meshData);
        }
    }

    private void Awake()
    {
        GenerateSoil();
        DrawSoil();
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

        foreach (Vector2[] edgePoints in meshData.GetEdges())
        {
            edgeColliderHolder.AddComponent<EdgeCollider2D>().points = edgePoints;
        }
    }
}