using System;
using UnityEngine;

[Serializable]
public class ResourceDefinition : IDatabaseItem<ResourceID>
{
    [SerializeField] private ResourceID _id;
    
    public ResourceID Id => _id;
    
    public string DisplayName;

    private ResourceTag _tags;
    
    public float NutritionalValue;

    public bool HasTag(ResourceTag tag)
    {
        return (_tags & tag) == tag;
    }
}

[Serializable]
public struct HexResources
{
    public AvailableResource[] Resources;
}

[Serializable]
public struct AvailableResource
{
    [SerializeField]
    private ResourceID _resourceId;

    [SerializeField]
    private float _quantity;

    public ResourceID ResourceId => _resourceId;
    public float Quantity => _quantity;

    public AvailableResource(ResourceID resourceId, float quantity)
    {
        _resourceId = resourceId;
        _quantity = quantity;
    }
}