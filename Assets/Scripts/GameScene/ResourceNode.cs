using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceNode : IResourceSource
{
    public int Order => 2;
    
    public void Tick(TickInfo tickInfo)
    {
        throw new System.NotImplementedException();
    }

    public ResourcePreview PreviewAvailableResources()
    {
        throw new System.NotImplementedException();
    }

    public void AddExtractRequest(ResourceRequest request)
    {
        throw new System.NotImplementedException();
    }

    public void SpawnSource()
    {
        throw new System.NotImplementedException();
    }

    public void RegenerateSource()
    {
        throw new System.NotImplementedException();
    }

    public void ResolveRequests()
    {
        throw new System.NotImplementedException();
    }
}
