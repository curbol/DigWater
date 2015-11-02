using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(SoilMapController))]
public class DigController : MonoBehaviour
{
    private SoilMapController soilMapController;
    private SoilMapController SoilMapController
    {
        get
        {
            if (soilMapController == null)
            {
                soilMapController = GetComponent<SoilMapController>();
            }

            return soilMapController;
        }
    }

    private SoilMap SoilMap
    {
        get
        {
            return SoilMapController != null ? SoilMapController.SoilMap : null;
        }
    }

    [SerializeField]
    private float digRadius;
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

    [SerializeField]
    private ParticleSystem digEffect;
    public ParticleSystem DigEffect
    {
        get
        {
            return digEffect;
        }

        set
        {
            digEffect = value;
            digEffect.transform.parent = transform;
        }
    }

    private void Start()
    {
        DigEffect = Instantiate(DigEffect, Vector2.zero, Quaternion.identity) as ParticleSystem;
    }

    private void Update()
    {
        bool leftMouseClicked = Input.GetMouseButton((int)MouseButton.Left);
        bool rightMouseClicked = Input.GetMouseButton((int)MouseButton.Right);

        if (SoilMap != null && (leftMouseClicked || rightMouseClicked))
        {
            SoilType newSoilType = leftMouseClicked ? SoilType.Default : SoilType.Dirt;
            Vector2 screenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Coordinate coordinateToDig = SoilMap.GetCoordinateFromPosition(screenPosition);

            if (SoilMap.SoilGrid[coordinateToDig.X, coordinateToDig.Y] != newSoilType)
            {
                if (leftMouseClicked)
                {
                    PlayDigEffect(screenPosition);
                }

                DrawDirt(SoilMap, coordinateToDig, newSoilType, DigRadius);
                SoilMapController.RedrawSoilMesh();
            }
        }
    }

    private static void DrawDirt(SoilMap soilMap, Coordinate coordinateToDig, SoilType soilType, float digRadius)
    {
        if (soilMap == null || soilMap.SoilGrid == null)
            return;

        soilMap.SoilGrid[coordinateToDig.X, coordinateToDig.Y] = soilType;
        foreach (Coordinate neighborCoordinate in soilMap.SoilGrid.GetNeighborCoordinatesInRadius(coordinateToDig.X, coordinateToDig.Y, digRadius))
        {
            soilMap.SoilGrid[neighborCoordinate.X, neighborCoordinate.Y] = soilType;
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
}