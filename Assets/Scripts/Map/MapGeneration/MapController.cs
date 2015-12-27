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

            bool redraw = false;
            bool[,] bitMap = MapManager.Map.GetSoilBitMap(soilType);

            if (!soilBitMaps.ContainsKey(soilType))
            {
                soilBitMaps.Add(soilType, bitMap);
                redraw = true;
            }

            if (!bitMap.Cast<bool>().SequenceEqual(soilBitMaps[soilType].Cast<bool>()))
            {
                soilBitMaps[soilType] = bitMap;
                redraw = true;
            }

            if (redraw || forceRedraw)
            {
                MeshData meshData = bitMap.GetMarchingSquaresMeshData(MapManager.Map.Scale);
                GameObject meshHolder = GetUniqueChildGameObject(transform, soilType.ToString());
                meshHolder.AddComponent<MeshFilter>().sharedMesh = meshData.GetMesh();
                meshHolder.AddComponent<MeshRenderer>().materials = new Material[] { soilType.Material() };

                if (soilType.IsCollidable())
                {
                    CreateEdgeColliders(meshHolder.transform, meshData, soilType.PhysicsMaterial());
                }
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

    private void Awake()
    {
        RedrawSoilMesh();
    }

    private void CreateEdgeColliders(Transform parent, MeshData meshData, PhysicsMaterial2D physicsMaterial = null)
    {
        GameObject edgeColliderHolder = GetUniqueChildGameObject(parent, "Edge Colliders");

        foreach (Vector2[] edgePoints in meshData.GetMeshEdges())
        {
            EdgeCollider2D edgeCollider = edgeColliderHolder.AddComponent<EdgeCollider2D>();
            edgeCollider.points = edgePoints.Select(e => e * MapManager.Map.Scale).ToArray();

            if (physicsMaterial != null)
            {
                edgeCollider.sharedMaterial = physicsMaterial;
            }
        }
    }
}