using System;
using UnityEngine;

[Serializable]
public class SoilMetadata
{
    [SerializeField]
    private SoilType soilType;
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

    [SerializeField]
    private int fillStartX;
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

    [SerializeField]
    private int fillEndX;
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

    [SerializeField]
    private int fillStartY;
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

    [SerializeField]
    private int fillEndY;
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

    [SerializeField]
    [Range(0, 100)]
    private int percentCoverage;
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

    [SerializeField]
    private Material material;
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

    [SerializeField]
    private PhysicsMaterial2D physicsMaterial;
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

    [SerializeField]
    private Transform digEffectPrefab;
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

    [SerializeField]
    private bool isDiggable;
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

    [SerializeField]
    private bool isCollidable;
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
}