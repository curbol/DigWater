using System;
using UnityEngine;

[Serializable]
public class SoilTypeMetadata
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