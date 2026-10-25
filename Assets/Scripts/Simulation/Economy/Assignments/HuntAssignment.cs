using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntAssignment : Assignment
{
    public override string AssignmentName => "Hunt";

    public override Color Color => Color.red;
    
    public HuntAssignment(int workerCount) : base(new AssignmentID((int)AssignmentTypes.Hunt), workerCount)
    {
        
    }

    public override void Tick(Pop pop)
    {
        Hunt(pop);
    }

    private void Hunt(Pop pop)
    {
        if (!pop.CurrentHex.TryGetAnimals(out Dictionary<SpeciesID, int> animals)) return;
        foreach (SpeciesID animal in animals.Keys)
        {
            if (IsHuntable(animal.Definition))
            {
                
            }
        }
    }

    private bool IsHuntable(SpeciesDefinition definition)
    {
        return true;
    }
}
