using UnityEngine;

[DisallowMultipleComponent]
public class MapGenerator : MonoBehaviour
{
    public ParticleSystem digEffect;
    public float digRadius;
    public int mapIndex;
    public SoilMap[] maps;
    private static readonly string mapHolderName = "Map";

    public SoilMap CurrentMap
    {
        get
        {
            SoilMap soilMap = maps != null && maps.Length > mapIndex ? maps[mapIndex] : null;

            if (soilMap.SoilGrid == null)
            {
                soilMap.SoilGrid = new SoilType[soilMap.SizeX, soilMap.SizeY];
            }

            return soilMap;
        }
    }

    public void GenerateMap()
    {
        if (CurrentMap == null)
            return;

        if (transform.FindChild(mapHolderName))
        {
            DestroyImmediate(transform.FindChild(mapHolderName).gameObject);
        }

        GameObject mapHolder = new GameObject(mapHolderName);
        mapHolder.transform.parent = transform;
        mapHolder.transform.localScale = Vector3.one * CurrentMap.Scale;

        SoilMapController soilMapController = mapHolder.AddComponent<SoilMapController>();
        DigController digController = mapHolder.AddComponent<DigController>();

        soilMapController.SoilMap = CurrentMap;
        digController.DigRadius = digRadius;
        digController.DigEffect = digEffect;

        //soilMapController.GenerateSoilMap();
        soilMapController.RedrawSoilMesh();
    }
}