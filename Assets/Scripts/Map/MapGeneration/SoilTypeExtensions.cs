using System.Linq;
using UnityEngine;

public static class SoilTypeExtensions
{
    public static SoilTypeMetadata SoilTypeMetadata(this SoilType soilType)
    {
        return SoilTypeMetadataManager.SoilTypeMetadata != null ? SoilTypeMetadataManager.SoilTypeMetadata.FirstOrDefault(a => a.SoilType == soilType) : null;
    }

    public static Material Material(this SoilType soilType)
    {
        SoilTypeMetadata metadata = SoilTypeMetadata(soilType);
        return metadata != null ? metadata.Material : null;
    }

    public static bool IsDiggable(this SoilType soilType)
    {
        SoilTypeMetadata metadata = SoilTypeMetadata(soilType);
        return metadata != null ? metadata.IsCollidable : false;
    }

    public static PhysicsMaterial2D DigEffectPrefab(this SoilType soilType)
    {
        SoilTypeMetadata metadata = SoilTypeMetadata(soilType);
        return metadata != null ? metadata.PhysicsMaterial : null;
    }

    public static bool IsCollidable(this SoilType soilType)
    {
        SoilTypeMetadata metadata = SoilTypeMetadata(soilType);
        return metadata != null ? metadata.IsCollidable : false;
    }

    public static PhysicsMaterial2D PhysicsMaterial(this SoilType soilType)
    {
        SoilTypeMetadata metadata = SoilTypeMetadata(soilType);
        return metadata != null ? metadata.PhysicsMaterial : null;
    }
}