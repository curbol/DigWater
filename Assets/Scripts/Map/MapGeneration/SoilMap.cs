using System;
using UnityEngine;

[Serializable]
public class SoilMap
{
    [SerializeField]
    [Range(0, 10)]
    private int borderThickness;

    [SerializeField]
    [Range(.1F, 2F)]
    private float scale = 1;

    [SerializeField]
    private int seed;

    [SerializeField]
    private int sizeX;

    [SerializeField]
    private int sizeY;

    [SerializeField]
    private SoilMetadata[] soils;

    public int BorderThickness
    {
        get
        {
            return borderThickness;
        }

        set
        {
            borderThickness = value;
        }
    }

    public float Height
    {
        get
        {
            return sizeY * Scale;
        }
    }

    public float Scale
    {
        get
        {
            return scale;
        }

        set
        {
            scale = value;
        }
    }

    public int Seed
    {
        get
        {
            return seed;
        }

        set
        {
            seed = value;
        }
    }

    public int SizeX
    {
        get
        {
            return sizeX;
        }

        set
        {
            sizeX = value;
        }
    }

    public int SizeY
    {
        get
        {
            return sizeY;
        }

        set
        {
            sizeY = value;
        }
    }

    public SoilType[,] SoilGrid { get; set; }

    public SoilMetadata[] Soils
    {
        get
        {
            return soils;
        }

        set
        {
            soils = value;
        }
    }

    public float Width
    {
        get
        {
            return sizeX * Scale;
        }
    }
}