using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePreview
{
    public ResourceCollection Contents { get; private set;  }

    public ResourcePreview(ResourceCollection contents)
    {
        Contents = contents;
    }

    public ResourcePreview(List<ResourcePreview> list)
    {
        
    }
    
    public void Destroy()
    {
        Contents = null;
    }
}
