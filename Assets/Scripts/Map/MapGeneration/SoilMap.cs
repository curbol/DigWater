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

    [SerializeField]
    [Range(.1f, 2f)]
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

    public SoilType[,] SoilGrid { get; set; }

    [SerializeField]
    [Range(25, 75)]
    private int percentDirt;
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

    [SerializeField]
    [Range(0, 10)]
    private int borderThickness;
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

    [SerializeField]
    private Material dirtMaterial;
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

    [SerializeField]
    private PhysicsMaterial2D dirtPhysics;
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
}