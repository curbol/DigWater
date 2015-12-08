using System;
using UnityEngine;

[Serializable]
public class SoilMap
{
    [SerializeField]
    private int seed;
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

    [SerializeField]
    [Range(.1F, 2F)]
    private float scale = 1;
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

    [SerializeField]
    private int sizeX;
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

    [SerializeField]
    private int sizeY;
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

    public float Width
    {
        get
        {
            return sizeX * Scale;
        }
    }

    public float Height
    {
        get
        {
            return sizeY * Scale;
        }
    }

    [HideInInspector]
    [SerializeField]
    private SoilType[] soilGrid;
    private SoilType[] SoilGrid
    {
        get
        {
            if (soilGrid == null || soilGrid.Length != SizeX * SizeY)
                soilGrid = new SoilType[SizeX * SizeY];

            return soilGrid;
        }

        set
        {
            soilGrid = value;
        }
    }

    public SoilType this[int x, int y]
    {
        get
        {
            return SoilGrid[x * SizeY + y];
        }

        set
        {
            SoilGrid[x * SizeY + y] = value;
        }
    }
}