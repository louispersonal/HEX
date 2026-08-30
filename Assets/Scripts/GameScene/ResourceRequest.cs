using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceRequest : MonoBehaviour
{
    public ResourceCollection Contents { get; private set;  }

    public ResourceRequest(ResourceCollection contents)
    {
        Contents = contents;
    }
    
    public void Destroy()
    {
        Contents = null;
    }
}
