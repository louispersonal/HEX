using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GatherAssignment : Assignment
{
    public override string AssignmentName => "Gather";

    public override Color Color => Color.green;

    private WorldData _worldData => GameController.Instance.SessionManager.WorldData;
    
    public GatherAssignment(int workerCount) : base(workerCount)
    {
        
    }

    public override void Tick(Pop pop)
    {
        Gather(pop);
        
    }

    private void Gather(Pop pop)
    {

    }
}
