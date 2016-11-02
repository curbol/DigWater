using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskCompletionHandler : MonoBehaviour
{
    [SerializeField]
    private bool showGizmos;

    [SerializeField]
    private DepthMeasurer depthMeasurer;

    [SerializeField]
    private SoilAnalyzer soilAnalyzer;

    [SerializeField]
    private LocationObjective2D measureArmoryDepthObjective = new LocationObjective2D();

    [SerializeField]
    private LocationObjective2D analyzeSoilAboveArmoryObjective = new LocationObjective2D();

    [SerializeField]
    private LocationObjective2D analyzeSoilBelowArmoryObjective = new LocationObjective2D();

    [SerializeField]
    private LocationObjective2D measureFoodWarehouseDepthObjective = new LocationObjective2D();

    [SerializeField]
    private LocationObjective2D analyzeSoilAboveFoodWarehouseObjective = new LocationObjective2D();

    [SerializeField]
    private LocationObjective2D analyzeSoilBelowFoodWarehouseObjective = new LocationObjective2D();

    public void HandleMeasureDepth(Vector2 position)
    {
        var Objectives = new List<LocationObjective2D>
        {
            measureArmoryDepthObjective,
            measureFoodWarehouseDepthObjective,
        };

        foreach (LocationObjective2D objective in Objectives)
        {
            if (objective.Location.Contains(position) && objective.Toggle != null)
                objective.Toggle.isOn = true;
        }
    }

    public void HandleAnalyzeSoil(Vector2 position)
    {
        SoilType clickedOnSoilType = MapManager.Map.GetSoilTypeFromPosition(position);

        var Objectives = new Dictionary<LocationObjective2D, SoilType>
        {
            { analyzeSoilAboveArmoryObjective, SoilType.Sand },
            { analyzeSoilBelowArmoryObjective, SoilType.Clay },
            { analyzeSoilAboveFoodWarehouseObjective, SoilType.Sand },
            { analyzeSoilBelowFoodWarehouseObjective, SoilType.Rock },
        };

        foreach (KeyValuePair<LocationObjective2D, SoilType> objective in Objectives)
        {
            Location2D location = objective.Key.Location;
            Toggle toggle = objective.Key.Toggle;
            SoilType objectiveSoilType = objective.Value;

            if (location.Contains(position) && clickedOnSoilType == objectiveSoilType && toggle != null)
                toggle.isOn = true;
        }
    }

    private void Start()
    {
        depthMeasurer.MeasureDepthEvent += HandleMeasureDepth;
        soilAnalyzer.AnalyzeSoilEvent += HandleAnalyzeSoil;
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos)
            return;

        Vector2 mouseScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Location2D[] locations = new[]
        {
            measureArmoryDepthObjective.Location,
            analyzeSoilAboveArmoryObjective.Location,
            analyzeSoilBelowArmoryObjective.Location,
            measureFoodWarehouseDepthObjective.Location,
            analyzeSoilAboveFoodWarehouseObjective.Location,
            analyzeSoilBelowFoodWarehouseObjective.Location,
        };

        foreach (Location2D location in locations)
        {
            if (location.Contains(mouseScreenPosition))
                Gizmos.color = new Color(0, 1, 0);
            else
                Gizmos.color = new Color(1, 0, 0);

            Gizmos.DrawLine(new Vector2(location.BottomLeft.x, location.BottomLeft.y), new Vector2(location.TopRight.x, location.BottomLeft.y));
            Gizmos.DrawLine(new Vector2(location.BottomLeft.x, location.TopRight.y), new Vector2(location.TopRight.x, location.TopRight.y));
            Gizmos.DrawLine(new Vector2(location.BottomLeft.x, location.BottomLeft.y), new Vector2(location.BottomLeft.x, location.TopRight.y));
            Gizmos.DrawLine(new Vector2(location.TopRight.x, location.BottomLeft.y), new Vector2(location.TopRight.x, location.TopRight.y));
        }
    }
}