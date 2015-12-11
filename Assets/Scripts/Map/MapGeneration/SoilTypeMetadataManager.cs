using UnityEngine;

public class SoilTypeMetadataManager : MonoBehaviour
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

    private static SoilTypeMetadataManager instance;
    private static SoilTypeMetadataManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (SoilTypeMetadataManager)FindObjectOfType(typeof(SoilTypeMetadataManager));
                if (instance == null)
                {
                    instance = new GameObject("SoilTypeMetadataManager").AddComponent<SoilTypeMetadataManager>();
                }
            }

            return instance;
        }
    }
}