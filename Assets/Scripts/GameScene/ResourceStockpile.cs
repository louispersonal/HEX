using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceStockpile
{
    public ResourceCollection Contents { get; private set;  }

    public Pop Owner { get; private set; }

    public ResourceStockpile(ResourceCollection contents, Pop owner)
    {
        Contents = contents;
        Owner = owner;
    }
    
    public void ReceiveDelivery(ResourceDelivery delivery)
    {
        foreach (ResourceID resource in delivery.Contents.GetAllResourceIDs())
        {
            Contents.Deposit(resource, delivery.Contents.Get(resource));
        }
        
        delivery.Destroy();
    }
    
    public bool RequestDelivery(ResourceRequest request, out ResourceDelivery delivery)
    {
        var deliveryContents = new ResourceCollection();
        bool completeFulfilment = true;
        foreach (ResourceID resource in request.Contents.GetAllResourceIDs())
        {
            completeFulfilment &= Contents.Withdraw(resource,
                request.Contents.Get(resource), out var amountRemoved);
            deliveryContents.Deposit(resource, amountRemoved);
        }
        
        request.Destroy();
        
        delivery = new ResourceDelivery(deliveryContents, Owner);
        return completeFulfilment;
    }

    public ResourcePreview GetPreview()
    {
        return new ResourcePreview(Contents);
    }
}
