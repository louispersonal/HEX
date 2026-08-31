using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

[Serializable]
public class ResourceDelivery
{
    public ResourceCollection Contents { get; private set; }
    
    public ResourceDelivery(ResourceCollection contents, Pop sender)
    {
        Contents = new ResourceCollection();
    }
    
    public void Destroy()
    {
        Contents = null;
    }
}