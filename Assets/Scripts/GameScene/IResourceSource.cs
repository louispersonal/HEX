using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IResourceSource : ITickable
{
    public ResourcePreview PreviewAvailableResources();
    public void AddExtractRequest(ResourceRequest request);
    public void SpawnSource();
    public void RegenerateSource();
    public void ResolveRequests();
}
