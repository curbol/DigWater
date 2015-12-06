using UnityEngine;

[DisallowMultipleComponent]
public class MapGenerator : MonoBehaviour
{
    public SoilTypeMetadata[] soilTypeMetadata =
    {
        new SoilTypeMetadata
        {
            SoilType = SoilType.None,
            Material = null,
            IsDiggable = false,
            DigEffectPrefab = null,
            IsCollidable = false,
            PhysicsMaterial = null
        },
        new SoilTypeMetadata
        {
            SoilType = SoilType.Dirt,
            Material = null,
            IsDiggable = true,
            DigEffectPrefab = null,
            IsCollidable = true,
            PhysicsMaterial = null
        },
        new SoilTypeMetadata
        {
            SoilType = SoilType.Rock,
            Material = null,
            IsDiggable = true,
            DigEffectPrefab = null,
            IsCollidable = true,
            PhysicsMaterial = null
        },
        new SoilTypeMetadata
        {
            SoilType = SoilType.Sand,
            Material = null,
            IsDiggable = true,
            DigEffectPrefab = null,
            IsCollidable = true,
            PhysicsMaterial = null
        }
    };

    public ParticleSystem digEffect;
    public float digRadius;
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
        SoilTypeExtentions.SoilTypeMetadata = soilTypeMetadata;

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
        DigController digController = MapHolder.AddComponent<DigController>();

        soilMapController.SoilMap = CurrentMap;
        digController.DigRadius = digRadius;
        digController.DigEffect = digEffect;

        soilMapController.RedrawSoilMesh();
    }
}