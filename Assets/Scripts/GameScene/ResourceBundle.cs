using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ResourceBundle
{
    [SerializeField] private Dictionary<ResourceID, float> _resources = new();
    
    public bool IsEmpty => _resources.Count == 0;
    
    public float Get(ResourceID resource)
    {
        return _resources.GetValueOrDefault(resource, 0f);
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

    public float Remove(ResourceID resource, float amount)
    {
        if (!_resources.TryGetValue(resource, out float existing)) return 0f;
        
        float removed = Mathf.Min(existing, amount);
        float remaining = existing - removed;

        if (remaining <= 0f) _resources.Remove(resource);
        else _resources[resource] = remaining;

        return removed;
    }
    
    public bool Contains(ResourceID resource, float amount)
    {
        return Get(resource) >= amount;
    }

    public IEnumerable<ResourceID> GetAllResourceIDs()
    {
        return _resources.Keys;
    }
    
    public void Add(ResourceBundle other)
    {
        foreach (ResourceID resource in other.GetAllResourceIDs())
        {
            Deposit(resource, other.Get(resource));
        }
    }
}
