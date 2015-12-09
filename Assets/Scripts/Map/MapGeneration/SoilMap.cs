using System;
using UnityEngine;

[Serializable]
public class SoilMap
{
    [SerializeField]
    private SoilTypeMetadata[] soilTypeMetadata =
    {
        new SoilTypeMetadata
        {
            SoilType = SoilType.None,
            Material = null,
            IsDiggable = false,
            DigEffectPrefab = null,
            IsCollidable = false,
            PhysicsMaterial = null
        },
        new SoilTypeMetadata
        {
            SoilType = SoilType.Dirt,
            Material = null,
            IsDiggable = true,
            DigEffectPrefab = null,
            IsCollidable = true,
            PhysicsMaterial = null
        },
        new SoilTypeMetadata
        {
            SoilType = SoilType.Rock,
            Material = null,
            IsDiggable = true,
            DigEffectPrefab = null,
            IsCollidable = true,
            PhysicsMaterial = null
        },
        new SoilTypeMetadata
        {
            SoilType = SoilType.Sand,
            Material = null,
            IsDiggable = true,
            DigEffectPrefab = null,
            IsCollidable = true,
            PhysicsMaterial = null
        }
    };

    public SoilTypeMetadata[] SoilTypeMetadata
    {
        get
        {
            return soilTypeMetadata;
        }

        set
        {
            soilTypeMetadata = value;
        }
    }

    [HideInInspector]
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
    private SoilType[] flattenedSoilGrid;
    public SoilType[] FlattenedSoilGrid
    {
        get
        {
            if (flattenedSoilGrid == null || flattenedSoilGrid.Length != SizeX * SizeY)
                Clear();

            return flattenedSoilGrid;
        }

        private set
        {
            flattenedSoilGrid = value;
        }
    }

    public SoilType this[int x, int y]
    {
        get
        {
            return FlattenedSoilGrid[x * SizeY + y];
        }

        set
        {
            FlattenedSoilGrid[x * SizeY + y] = value;
        }
    }

    public void Clear()
    {
        FlattenedSoilGrid = new SoilType[SizeX * SizeY];
    }
}