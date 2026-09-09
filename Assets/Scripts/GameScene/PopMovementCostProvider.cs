using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopMovementCostProvider : IMovementCostProvider
{
    public float MinimumCost => 1f;

    private Pop _pop;
    
    public PopMovementCostProvider(Pop pop)
    {
        _pop = pop;
    }
    
    public bool CanEnter(Hex from, Hex to)
    {
        if (to.ExtraData.IsSea) return false;
        return true;
    }
    
    // Must return float >= MinimumCost
    public float GetCost(Hex from, Hex to)
    {
        switch (to.ExtraData.Biome)
        {
            case Biome.Desert:
            case Biome.Tropical:
            case Biome.Tundra:
            case Biome.Taiga:
                return 3f;
            case Biome.Savanna:
            case Biome.Steppe:
            case Biome.Temperate:
                return 2f;
        }
        return 1f;
    }
}
