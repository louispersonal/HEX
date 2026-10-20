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
    
    public override void DecisionTick(TickInfo tickInfo)
    {
        ManageAssignments();
        base.DecisionTick(tickInfo);
    }

    private void ManageAssignments()
    {
        int workerSurplus = Pop.WorkerSurplus();
        
        if (workerSurplus > 0)
        {
            Pop.CreateGatherAssignment(Pop.WorkingPopulation);
        }
    }
}
