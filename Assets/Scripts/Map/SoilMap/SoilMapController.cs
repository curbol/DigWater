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
            MeshFilter.mesh = dirtMap.GenerateMarchingSquaresMesh();
        }
    }

    private void Start()
    {
        GenerateSoil();
    }
}