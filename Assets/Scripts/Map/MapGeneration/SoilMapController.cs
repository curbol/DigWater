﻿using UnityEngine;

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

            Smooth(SoilMap.SoilGrid, SoilType.Dirt, SoilType.Default, SoilMap.Seed);
        }
    }

    public void RedrawSoil()
    {
        if (SoilMap != null && SoilMap.SoilGrid != null && MeshFilter != null)
        {
            bool[,] dirtMap = SoilMap.SoilGrid.GetSoilBitMap(SoilType.Dirt);
            MeshData meshData = dirtMap.GetMarchingSquaresMeshData(SoilMap.Width, SoilMap.Height);
            MeshFilter.mesh = meshData.GetMesh();

            CreateEdgeColliders(meshData);
        }
    }

    private void Awake()
    {
        GenerateSoil();
        RedrawSoil();
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
            edgeColliderHolder.AddComponent<EdgeCollider2D>().points = edgePoints;
        }
    }
}