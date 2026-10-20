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

    public ResourcePreview(List<ResourcePreview> allPreviews)
    {
        if (allPreviews == null ||  allPreviews.Count == 0) Debug.LogError("All previews are empty");
        
        ResourceCollection newCollection = new ResourceCollection();

        for (int i = allPreviews.Count - 1; i >= 0; i--)
        {
            foreach (var id in allPreviews[i].Contents.GetAllResourceIDs())
            {
                newCollection.Deposit(id, allPreviews[i].Contents.Get(id));
            }

            allPreviews[i].Destroy();
            allPreviews.RemoveAt(i);
        }
        
        Contents = newCollection;
    }
    
    public void Destroy()
    {
        Contents = null;
    }
}
