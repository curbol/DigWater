using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MapController : MonoBehaviour
{
    private readonly Dictionary<SoilType, bool[,]> soilBitMaps = new Dictionary<SoilType, bool[,]>();

    // will not redraw if nothing changed by default
    public void RedrawSoilMesh()
    {
        RedrawSoilMesh(false);
    }

    public void RedrawSoilMesh(bool forceRedraw)
    {
        if (MapManager.Map == null)
            return;

        foreach (SoilType soilType in Enum.GetValues(typeof(SoilType)) as IEnumerable<SoilType>)
        {
            if (soilType.Material() == null)
                continue;

            bool[,] bitMap = MapManager.Map.GetSoilBitMap(soilType);
            bool redraw = SoilMapHasChanged(soilType, bitMap);

            if (redraw.IsFalse() && forceRedraw.IsFalse())
                continue;

            MeshData meshData = bitMap.GetMarchingSquaresMeshData(MapManager.Map.Scale);
            Mesh mesh = meshData.BuildMesh();
            GameObject soilMeshHolder = transform.CreateUniqueChildGameObject(soilType.ToString());
            soilMeshHolder.AddComponent<MeshFilter>().sharedMesh = mesh;
            soilMeshHolder.AddComponent<MeshRenderer>().materials = new Material[] { soilType.Material() };

            if (soilType.IsCollidable())
            {
                CreateEdgeColliders(soilMeshHolder.transform, meshData, soilType.PhysicsMaterial());
            }
        }
    }

    private void Awake()
    {
        RedrawSoilMesh();
    }

    private bool SoilMapHasChanged(SoilType soilType, bool[,] bitMap)
    {
        bool hasChanged = false;
        if (soilBitMaps.ContainsKey(soilType).IsFalse())
        {
            soilBitMaps.Add(soilType, bitMap);
            hasChanged = true;
        }
        else if (bitMap.Cast<bool>().SequenceEqual(soilBitMaps[soilType].Cast<bool>()).IsFalse())
        {
            soilBitMaps[soilType] = bitMap;
            hasChanged = true;
        }

        return hasChanged;
    }

    private void CreateEdgeColliders(Transform parent, MeshData meshData, PhysicsMaterial2D physicsMaterial = null)
    {
        GameObject edgeColliderHolder = parent.CreateUniqueChildGameObject("Edge Colliders");

        foreach (Vector2[] edgePoints in meshData.BuildMeshEdges())
        {
            EdgeCollider2D edgeCollider = edgeColliderHolder.AddComponent<EdgeCollider2D>();
            edgeCollider.points = edgePoints;

            if (physicsMaterial != null)
            {
                edgeCollider.sharedMaterial = physicsMaterial;
            }
        }
    }
}