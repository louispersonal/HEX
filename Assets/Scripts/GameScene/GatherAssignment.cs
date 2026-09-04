using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GatherAssignment : Assignment
{
    public override string AssignmentName => "Gather";

    public override Color Color => Color.green;
    
    public GatherAssignment(int workerCount) : base(workerCount)
    {
        
    }

    public override void Tick(Pop pop)
    {
        Gather(pop);
    }

    private void Gather(Pop pop)
    {
        ResourceCollection gatherBill = new  ResourceCollection();
        var preview = pop.CurrentHex.SeeAllAvailableResources();
        foreach (var id in preview.Contents.GetAllResourceIDs())
        {
            if (id.Definition.HasTag(ResourceTag.Edible))
            {
                float amountCanBeGathered = CalculateMaximumGatherable(id, pop);
                gatherBill.Deposit(id,  amountCanBeGathered);
            }
        }
        ResourceRequest request = new ResourceRequest(gatherBill, pop);
        pop.CurrentHex.VegetationSource.AddExtractRequest(request);
    }

    private float CalculateMaximumGatherable(ResourceID id, Pop pop)
    {
        return pop.Culture.GatheringProficiency[id] * Workers;
    }
}
