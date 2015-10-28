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

	private void Update()
	{
        if (SoilMap != null && Input.GetMouseButton((int)MouseButton.Left))
        {
            DrawDirt(SoilMap, GetCoordinateToDig(SoilMap, Input.mousePosition), SoilType.Default, DigRadius);
            SoilMapController.RedrawSoilMesh();
        }
        else if (SoilMap != null && Input.GetMouseButton((int)MouseButton.Right))
        {
            DrawDirt(SoilMap, GetCoordinateToDig(SoilMap, Input.mousePosition), SoilType.Dirt, DigRadius);
            SoilMapController.RedrawSoilMesh();
        }
    }

    private static Coordinate GetCoordinateToDig(SoilMap soilMap, Vector2 digPosition)
    {
        Vector2 positionToDig = Camera.main.ScreenToWorldPoint(digPosition);
        Coordinate coordinateToDig = soilMap.GetCoordinateFromPosition(positionToDig);

        return coordinateToDig;
    }

    private static void DrawDirt(SoilMap soilMap, Coordinate coordinateToDig, SoilType soilType, float digRadius)
    {
        if (soilMap == null || soilMap.SoilGrid == null || soilMap.SoilGrid[coordinateToDig.X, coordinateToDig.Y] == soilType)
            return;

        soilMap.SoilGrid[coordinateToDig.X, coordinateToDig.Y] = soilType;
        foreach (Coordinate neighborCoordinate in soilMap.SoilGrid.GetNeighborCoordinatesInRadius(coordinateToDig.X, coordinateToDig.Y, digRadius))
        {
            soilMap.SoilGrid[neighborCoordinate.X, neighborCoordinate.Y] = soilType;
        }
    }
}