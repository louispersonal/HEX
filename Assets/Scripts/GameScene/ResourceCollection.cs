using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCollection
{
    [SerializeField] private Dictionary<ResourceID, float> _resources = new();

    public bool IsEmpty => _resources.Count == 0;
    
    public void ClearAll()
    {
        _resources.Clear();
    }

    public void SetAllResources(Dictionary<ResourceID, float> resources)
    {
        _resources = resources;
    }
    
    public void Deposit(ResourceID resource, float amount)
    {
        if (amount <= 0f) return;

        if (_resources.TryGetValue(resource, out float existing))
        {
            _resources[resource] = existing + amount;
        }

        else _resources[resource] = amount;
    }
    
    public bool Withdraw(ResourceID resource, float amount, out float removed)
    {
        removed = 0f;
        if (!_resources.TryGetValue(resource, out float existing)) return false;
        
        bool completeFulfilment = existing >= amount;
        removed = Mathf.Min(existing, amount);
        float remaining = existing - removed;

        if (remaining == 0f) _resources.Remove(resource);

        else _resources[resource] = remaining;

        return completeFulfilment;
    }
    
    public float Get(ResourceID resource)
    {
        return _resources.GetValueOrDefault(resource, 0f);
    }
    
    public bool Contains(ResourceID resource, float amount)
    {
        return Get(resource) >= amount;
    }

    public IEnumerable<ResourceID> GetAllResourceIDs()
    {
        return _resources.Keys;
    }

    public ResourceCollection Clone()
    {
        ResourceCollection clonedCollection = new();
        clonedCollection.SetAllResources(new Dictionary<ResourceID, float>(_resources));
        return clonedCollection;
    }
}
