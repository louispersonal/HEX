using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pop : Pawn
{
    public float GatheringEfficiency = 1f;
    
    public Dictionary<ResourceID, float> ResourceStockpile { get; private set; }
    
    public AxialCoordinate Location;
    
    private List<Assignment> _assignments = new();

    public override void Tick(TickInfo tickInfo)
    {
        foreach (var assignment in _assignments)
        {
            assignment.Tick(this);
        }
    }
}