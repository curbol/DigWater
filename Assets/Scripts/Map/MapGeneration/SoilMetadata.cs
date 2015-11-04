using System;
using UnityEngine;

[Serializable]
public class SoilMetadata
{
    [SerializeField]
    private Transform digEffectPrefab;

    [SerializeField]
    private bool isCollidable;

    [SerializeField]
    private Material material;

    [SerializeField]
    [Range(20, 80)]
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