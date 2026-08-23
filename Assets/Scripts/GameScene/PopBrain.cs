using System.Collections;
using System.Collections.Generic;
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
            
        }
        
        int workerSurplus = Pop.CheckAssignmentNumbers();
    }
}
