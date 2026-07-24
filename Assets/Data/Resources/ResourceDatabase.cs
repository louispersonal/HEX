using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Resources/Resource Database")]
public sealed class ResourceDatabase : ScriptableObject
{
    [SerializeField]
    private ResourceDefinition[] _resources;

    private Dictionary<ResourceID, ResourceDefinition> _lookup;

    public ResourceDefinition Get(ResourceID id)
    {
        EnsureLookup();

        return _lookup[id];
    }

    private void EnsureLookup()
    {
        if (_lookup != null)
            return;

        _lookup = new Dictionary<ResourceID, ResourceDefinition>();

        foreach (ResourceDefinition resource in _resources)
        {
            _lookup.Add(resource.Id, resource);
        }
    }
}