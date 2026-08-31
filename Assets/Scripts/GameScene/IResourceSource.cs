using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IResourceSource : ITickable
{
    public ResourcePreview PreviewAvailableResources();
    public void AddExtractRequest(ResourceRequest request);
    public void RegenerateSource();
    public void ResolveRequests();
}
