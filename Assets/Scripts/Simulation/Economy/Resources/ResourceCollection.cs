using System.Collections.Generic;
using UnityEngine;

public class ResourceCollection
{
    private SerializableDictionary<ResourceID, float> _resources;

    public SerializableDictionary<ResourceID, float> Resources => _resources;
    
    public bool IsEmpty => _resources.Count == 0;
    
    public void ClearAll()
    {
        _resources.Clear();
    }

    public ResourceCollection()
    {
        _resources = new SerializableDictionary<ResourceID, float>();
    }

    private ResourceCollection(ResourceCollection original)
    {
        _resources = new SerializableDictionary<ResourceID, float>(original.Resources);
    }

    public ResourceCollection(SerializableDictionarySaveData<ResourceID, float> saveData)
    {
        _resources = new SerializableDictionary<ResourceID, float>(saveData);
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
        ResourceCollection clonedCollection = new(this);
        return clonedCollection;
    }
}