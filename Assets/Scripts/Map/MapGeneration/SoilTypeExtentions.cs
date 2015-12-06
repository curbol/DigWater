using System.Linq;
using UnityEngine;

public static class SoilTypeExtentions
{
    public static SoilTypeMetadata[] SoilTypeMetadata { get; set; }

    private static SoilTypeMetadata GetSoilTypeMetadata(this SoilType soilType)
    {
        return SoilTypeMetadata.First(a => a.SoilType == soilType);
    }

    public static Material Material(this SoilType soilType)
    {
        SoilTypeMetadata soilTypeMetadata = GetSoilTypeMetadata(soilType);
        return soilTypeMetadata != null ? soilTypeMetadata.Material : null;
    }

    public static bool IsCollidable(this SoilType soilType)
    {
        SoilTypeMetadata soilTypeMetadata = GetSoilTypeMetadata(soilType);
        return soilTypeMetadata != null ? soilTypeMetadata.IsCollidable : false;
    }

    public static PhysicsMaterial2D PhysicsMaterial(this SoilType soilType)
    {
        SoilTypeMetadata soilTypeMetadata = GetSoilTypeMetadata(soilType);
        return soilTypeMetadata != null ? soilTypeMetadata.PhysicsMaterial : null;
    }
}