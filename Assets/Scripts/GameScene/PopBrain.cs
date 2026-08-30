using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PopBrain : Brain
{
    public Pop Pop => Pawn as Pop;
    
    public PopBrain(Pop pop) : base(pop)
    {
        
    }
    
    public override void Tick(TickInfo tickInfo)
    {
        ManageAssignments();
        base.Tick(tickInfo);
    }

    private void ManageAssignments()
    {
        if (Pop.Assignments.Count == 0)
        {
            Pop.CreateGatherAssignment(Pop.Population);
        }
        
        int workerSurplus = Pop.CheckAssignmentNumbers();
        if (workerSurplus > 0) Pop.Assignments.OfType<GatherAssignment>().First().AddWorkers(workerSurplus);
        if (workerSurplus < 0) Pop.Assignments.OfType<GatherAssignment>().First().RemoveWorkers(-workerSurplus);
    }

    private void DecideMove()
    {
        var grid = GameController.Instance.SessionManager.WorldData.Grid;
        List<Hex> neighborData = new List<Hex>();
        foreach (AxialCoordinate direction in AxialDirections.Directions)
        {
            if (grid.TryGetHex(Pop.CurrentHex.Coord + direction, out var neighbor))
            {
                neighborData.Add(neighbor);
            }
        }

        float currentHexValue = AssessHex(Pop.CurrentHex);
        var sortedNeighbors = neighborData.OrderByDescending(AssessHex).ToList();
        if (AssessHex(sortedNeighbors[0]) > currentHexValue) CreateMoveJob(sortedNeighbors[0].Coord);
    }

    private float AssessHex(Hex hex)
    {
        // get attracttiveness value of hex
        return 0f;
    }
    
    private void CreateMoveJob(AxialCoordinate destination)
    {
        AddJob(new MoveJob(5, Pop,  destination));
    }
}
