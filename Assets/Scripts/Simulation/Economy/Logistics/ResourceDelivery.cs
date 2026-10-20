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
        Contents = contents;
    }
    
    public void Destroy()
    {
        Contents = null;
    }
}