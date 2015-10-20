using UnityEngine;

[DisallowMultipleComponent]
public class MapGenerator : MonoBehaviour
{
    private static readonly string mapHolderName = "Map";

    public SoilMap[] maps;
    public int mapIndex;
    public Material dirtMaterial;
    public float digRadius;

    public SoilMap CurrentMap
    {
        get
        {
            return maps != null && maps.Length > mapIndex ? maps[mapIndex] : null;
        }
    }

    public void GenerateMap()
    {
        if (CurrentMap != null)
        {
            if (transform.FindChild(mapHolderName))
            {
                DestroyImmediate(transform.FindChild(mapHolderName).gameObject);
            }

            GameObject mapHolder = new GameObject(mapHolderName);
            mapHolder.transform.parent = transform;
            mapHolder.transform.localScale = Vector3.one * CurrentMap.Scale;

            mapHolder.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = mapHolder.AddComponent<MeshRenderer>();
            SoilMapController soilMapController = mapHolder.AddComponent<SoilMapController>();
            DigController digController = mapHolder.AddComponent<DigController>();

            meshRenderer.materials = new Material[] { dirtMaterial };
            soilMapController.SoilMap = CurrentMap;
            digController.DigRadius = digRadius;

            soilMapController.GenerateSoil();
            soilMapController.DrawSoil();
        }
    }
}