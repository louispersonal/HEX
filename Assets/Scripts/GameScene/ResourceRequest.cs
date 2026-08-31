using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceRequest
{
    public ResourceCollection Contents { get; private set;  }
    
    public Pop Sender { get; private set;  }

    public ResourceRequest(ResourceCollection contents, Pop sender)
    {
        Contents = contents;
        Sender = sender;
    }
    
    public void Destroy()
    {
        Contents = null;
    }
}
