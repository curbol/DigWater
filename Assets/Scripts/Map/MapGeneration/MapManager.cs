using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField]
    private int mapIndex;

    [SerializeField]
    private Map[] maps;

    private static MapManager instance;
    private static MapManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (MapManager)FindObjectOfType(typeof(MapManager));
                if (instance == null)
                {
                    instance = new GameObject("MapManager").AddComponent<MapManager>();
                }
            }

            return instance;
        }
    }

    public static Map Map
    {
        get
        {
            return Instance.maps != null && Instance.maps.Length > Instance.mapIndex ? Instance.maps[Instance.mapIndex] : null;
        }
    }

    private void Awake()
    {
        instance = this;
    }
}