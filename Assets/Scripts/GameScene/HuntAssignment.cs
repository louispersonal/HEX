using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntAssignment : Assignment
{
    public override string AssignmentName => "Hunt";

    public override Color Color => Color.red;
    
    public HuntAssignment(int workerCount) : base(workerCount)
    {
        
    }

    public override void Tick(Pop pop)
    {
        Hunt(pop);
    }

    private void Hunt(Pop pop)
    {
        foreach (var a in pop.CurrentHex.Animals())
        {
            if (IsHuntable(a.Key.Definition))
            {
                
            }
        }
    }

    private bool IsHuntable(SpeciesDefinition definition)
    {
        return true;
    }
}
