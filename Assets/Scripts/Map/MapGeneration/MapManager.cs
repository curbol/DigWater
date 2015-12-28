using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField]
    private bool showGizmos;

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

    private void OnDrawGizmos()
    {
        if (!showGizmos)
            return;

        for (int x = 0; x < Map.SizeX; x++)
        {
            for (int y = 0; y < Map.SizeY; y++)
            {
                switch (Map[x, y])
                {
                    case (SoilType.Bark):
                        Gizmos.color = Color.cyan;
                        break;

                    case (SoilType.Dirt):
                        Gizmos.color = Color.blue;
                        break;

                    case (SoilType.Grass):
                        Gizmos.color = Color.red;
                        break;

                    case (SoilType.Leaves):
                        Gizmos.color = Color.magenta;
                        break;

                    case (SoilType.Rock):
                        Gizmos.color = Color.black;
                        break;

                    case (SoilType.Sand):
                        Gizmos.color = Color.green;
                        break;

                    default:
                        Gizmos.color = Color.gray;
                        break;
                }

                Gizmos.DrawCube(Map.GetPositionFromCoordinate(x, y), Vector2.one * 0.2F);
            }
        }
    }
}