using System;
using UnityEngine;

[Serializable]
public class SoilMap
{
    [SerializeField]
    [Range(0, 10)]
    private int borderThickness;

    [SerializeField]
    private Material dirtMaterial;

    [SerializeField]
    private PhysicsMaterial2D dirtPhysics;

    [SerializeField]
    [Range(25, 75)]
    private int percentDirt;

    [SerializeField]
    private Material rockMaterial;

    [SerializeField]
    [Range(.1F, 2F)]
    private float scale = 1;

    [SerializeField]
    private int seed;

    [SerializeField]
    private int sizeX;

    [SerializeField]
    private int sizeY;

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

    public Material DirtMaterial
    {
        get
        {
            return dirtMaterial;
        }

        set
        {
            dirtMaterial = value;
        }
    }

    public PhysicsMaterial2D DirtPhysics
    {
        get
        {
            return dirtPhysics;
        }

        set
        {
            dirtPhysics = value;
        }
    }

    public float Height
    {
        get
        {
            return sizeY * Scale;
        }
    }

    public int PercentDirt
    {
        get
        {
            return percentDirt;
        }

        set
        {
            percentDirt = value;
        }
    }

    public Material RockMaterial
    {
        get
        {
            return rockMaterial;
        }

        set
        {
            rockMaterial = value;
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

    public float Width
    {
        get
        {
            return sizeX * Scale;
        }
    }
}