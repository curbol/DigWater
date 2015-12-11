using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(MapController))]
public class DigController : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem digEffect;

    [SerializeField]
    private float digRadius;

    private MapController soilMapController;

    public ParticleSystem DigEffect
    {
        get
        {
            return digEffect;
        }

        set
        {
            digEffect = value;
        }
    }

    public float DigRadius
    {
        get
        {
            return digRadius;
        }

        set
        {
            digRadius = value;
        }
    }

    private MapController SoilMapController
    {
        get
        {
            if (soilMapController == null)
            {
                soilMapController = GetComponent<MapController>();
            }

            return soilMapController;
        }
    }

    private void PlayDigEffect(Vector2 screenPosition)
    {
        if (DigEffect == null)
            return;

        DigEffect.Stop();
        DigEffect.transform.position = screenPosition;
        DigEffect.GetComponent<ParticleSystem>().Play();
    }

    private void Start()
    {
        DigEffect = Instantiate(DigEffect, Vector2.zero, Quaternion.identity) as ParticleSystem;
        DigEffect.transform.parent = transform;
    }

    private void Update()
    {
        bool leftMouseClicked = Input.GetMouseButton((int)MouseButton.Left);
        bool rightMouseClicked = Input.GetMouseButton((int)MouseButton.Right);

        if (MapManager.Map != null && (leftMouseClicked || rightMouseClicked))
        {
            SoilType newSoilType = leftMouseClicked ? SoilType.None : SoilType.Dirt;
            Vector2 screenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Coordinate coordinateToDig = MapManager.Map.GetCoordinateFromPosition(screenPosition);

            if (MapManager.Map[coordinateToDig.X, coordinateToDig.Y] != newSoilType)
            {
                if (leftMouseClicked)
                {
                    PlayDigEffect(screenPosition);
                }

                MapManager.Map.Draw(coordinateToDig, newSoilType, DigRadius);

                SoilMapController.RedrawSoilMesh();
            }
        }
    }
}