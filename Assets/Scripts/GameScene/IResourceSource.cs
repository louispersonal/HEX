using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface IResourceSource : IResolutionTick
{
    public ResourcePreview PreviewAvailableResources();
    public void AddExtractRequest(ResourceRequest request);
    public void RegenerateSource();
    public void ResolveRequests();
}
