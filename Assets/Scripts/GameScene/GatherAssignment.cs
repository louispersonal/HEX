using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GatherAssignment : Assignment
{
    private const float VEGETATION_ACCESSIBILITY = 0.1f;
    
    private const float BASE_GATHER_RATE = 1f;

    private Dictionary<ResourceID, float> _resourceBuffer = new();

    private WorldData _worldData => GameController.Instance.SessionManager.WorldData;
    
    public GatherAssignment(int workerCount) : base(workerCount)
    {
        
    }

    public override void Tick(Pop pop)
    {
        Gather(pop);
        
        foreach (ResourceID resourceID in _resourceBuffer.Keys)
        {
            pop.ResourceStockpile.TryGetValue(resourceID, out float resourceStockpile);
            pop.ResourceStockpile[resourceID] = resourceStockpile + _resourceBuffer[resourceID];
        }
    }

    private void Gather(Pop pop)
    {
        Dictionary<ResourceID, float> allResources = new();

        _worldData.GetAvailableResources(pop.CurrentHex.Coord, allResources);

        foreach (ResourceID resourceId in allResources.Keys)
        {
            var resourceDefinition = GameController.Instance.StaticDatabases.ResourceDatabase.Get(resourceId);
            if (resourceDefinition.HasTag(ResourceTag.Edible))
            {
                float maximumGatherable = allResources[resourceId] * VEGETATION_ACCESSIBILITY;
                allResources[resourceId] = maximumGatherable;
            }
            else allResources[resourceId] = 0;
            
            float workforceCapacity = Workers * BASE_GATHER_RATE * pop.GatheringEfficiency;
            _resourceBuffer[resourceId] = Mathf.Min(workforceCapacity, allResources[resourceId]);
        }
    }
}
