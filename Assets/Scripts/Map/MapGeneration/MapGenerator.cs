using UnityEngine;

[DisallowMultipleComponent]
public class MapGenerator : MonoBehaviour
{
    public bool enableSoilDrawing = true;
    public SoilType selectedDrawingSoilType;

    [Range(.1F, 5)]
    public float drawRadius;

    public int mapIndex;
    public SoilMap[] maps;
    private static readonly string mapHolderName = "Map";

    public SoilMap CurrentMap
    {
        get
        {
            return maps != null && maps.Length > mapIndex ? maps[mapIndex] : null;
        }
    }

    public GameObject MapHolder { get; set; }

    public void GenerateMap()
    {
        if (CurrentMap == null)
            return;

        if (transform.FindChild(mapHolderName))
        {
            DestroyImmediate(transform.FindChild(mapHolderName).gameObject);
        }

        MapHolder = new GameObject(mapHolderName);
        MapHolder.transform.parent = transform;
        MapHolder.transform.localScale = Vector3.one * CurrentMap.Scale;

        SoilMapController soilMapController = MapHolder.AddComponent<SoilMapController>();
        MapHolder.AddComponent<DigController>();

        soilMapController.SoilMap = CurrentMap;
        soilMapController.RedrawSoilMesh();
    }
}