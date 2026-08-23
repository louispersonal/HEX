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
        if (workerSurplus < 0) Pop.Assignments.OfType<GatherAssignment>().First().RemoveWorkers(workerSurplus);
    }
}
