using UnityEngine;

public class DigRequest
{
    private SoilMap soilMap;
    public SoilMap SoilMap
    {
        get
        {
            return soilMap;
        }

        private set
        {
            soilMap = value;
        }
    }

    private Vector2 position;
    public Vector2 Position
    {
        get
        {
            return position;
        }

        private set
        {
            position = value;
        }
    }

    private float digRadius;
    public float DigRadius
    {
        get
        {
            return digRadius;
        }

        private set
        {
            digRadius = value;
        }
    }

    public DigRequest(SoilMap soilMap, Vector2 position, float digRadius)
    {
        SoilMap = soilMap;
        Position = position;
        DigRadius = digRadius;
    }
}