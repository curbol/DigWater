using UnityEngine;

[DisallowMultipleComponent]
public class MapGenerator : MonoBehaviour
{
    public bool enableSoilDrawing = true;
    public SoilType drawingSoilType;

    [Range(.1F, 5)]
    public float drawRadius;

    private static readonly string mapHolderName = "Map";

    public GameObject MapHolder { get; set; }

    public void GenerateMap()
    {
        if (MapManager.Map == null)
            return;

        if (transform.FindChild(mapHolderName))
        {
            DestroyImmediate(transform.FindChild(mapHolderName).gameObject);
        }

        MapHolder = new GameObject(mapHolderName);
        MapHolder.transform.parent = transform;
        MapHolder.transform.localScale = Vector3.one * MapManager.Map.Scale;

        MapController soilMapController = MapHolder.AddComponent<MapController>();
        MapHolder.AddComponent<DigController>();

        soilMapController.RedrawSoilMesh();
    }
}