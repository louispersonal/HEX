using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Pop : Pawn
{
    public float GatheringEfficiency = 1f;
    
    public int Population { get; private set; }
    
    public Dictionary<ResourceID, float> ResourceStockpile { get; private set; }
    
    public AxialCoordinate Location;

    public HexData CurrentHex =>
        GameController.Instance.SessionManager.WorldData.Grid.GetHex(Location);
    
    private List<Assignment> _assignments = new();
    
    public List<Assignment> Assignments => _assignments;

    public Pop(int startingPopulation)
    {
        Population = startingPopulation;
    }
    
    public override void Tick(TickInfo tickInfo)
    {
        foreach (var assignment in _assignments)
        {
            assignment.Tick(this);
        }
    }

    public int CheckAssignmentNumbers()
    {
        int sum = 0;
        foreach (var assignment in _assignments)
        {
            sum += assignment.Workers;
        }
        return sum;
    }

    public GatherAssignment CreateGatherAssignment(int workers)
    {
        var gather = new GatherAssignment(workers);
        _assignments.Add(gather);
        return gather;
    }
}