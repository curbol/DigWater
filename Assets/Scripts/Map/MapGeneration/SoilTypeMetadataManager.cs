using UnityEngine;

public class SoilTypeMetadataManager : Singleton<SoilTypeMetadataManager>
{
    [SerializeField]
    private SoilTypeMetadata[] soilTypeMetadata;
    public static SoilTypeMetadata[] SoilTypeMetadata
    {
        get
        {
            return Instance.soilTypeMetadata;
        }
    }
}