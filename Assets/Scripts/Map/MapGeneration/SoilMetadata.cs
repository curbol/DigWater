using System;
using UnityEngine;

[Serializable]
public class SoilMetadata
{
    [SerializeField]
    private Transform digEffectPrefab;

    [SerializeField]
    private int fillEndX;

    [SerializeField]
    private int fillEndY;

    [SerializeField]
    private int fillStartX;

    [SerializeField]
    private int fillStartY;

    [SerializeField]
    private bool isCollidable;

    [SerializeField]
    private bool isDiggable;

    [SerializeField]
    private Material material;

    [SerializeField]
    [Range(0, 100)]
    private int percentCoverage;

    [SerializeField]
    private PhysicsMaterial2D physicsMaterial;

    [SerializeField]
    private SoilType soilType;

    public Transform DigEffectPrefab
    {
        get
        {
            return digEffectPrefab;
        }

        set
        {
            digEffectPrefab = value;
        }
    }

    public int FillEndX
    {
        get
        {
            return fillEndX;
        }

        set
        {
            fillEndX = value;
        }
    }

    public int FillEndY
    {
        get
        {
            return fillEndY;
        }

        set
        {
            fillEndY = value;
        }
    }

    public int FillStartX
    {
        get
        {
            return fillStartX;
        }

        set
        {
            fillStartX = value;
        }
    }

    public int FillStartY
    {
        get
        {
            return fillStartY;
        }

        set
        {
            fillStartY = value;
        }
    }

    public bool IsCollidable
    {
        get
        {
            return isCollidable;
        }

        set
        {
            isCollidable = value;
        }
    }

    public bool IsDiggable
    {
        get
        {
            return isDiggable;
        }

        set
        {
            isDiggable = value;
        }
    }

    public Material Material
    {
        get
        {
            return material;
        }

        set
        {
            material = value;
        }
    }

    public int PercentCoverage
    {
        get
        {
            return percentCoverage;
        }

        set
        {
            percentCoverage = value;
        }
    }

    public PhysicsMaterial2D PhysicsMaterial
    {
        get
        {
            return physicsMaterial;
        }

        set
        {
            physicsMaterial = value;
        }
    }

    public SoilType SoilType
    {
        get
        {
            return soilType;
        }

        set
        {
            soilType = value;
        }
    }
}