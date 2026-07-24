using UnityEngine;

[System.Serializable]
public class ResourceDefinition
{
    public ResourceID Id;
    public string DisplayName;

    private ResourceTag _tags;
    
    public float NutritionalValue;

    public bool HasTag(ResourceTag tag)
    {
        return (_tags & tag) == tag;
    }
}

[System.Serializable]
public struct HexResources
{
    public AvailableResource[] Resources;
}

[System.Serializable]
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

[System.Serializable]
public struct ResourceID
{
    public ushort Value;

    public ResourceID(ushort value)
    {
        Value = value;
    }
    
    public bool Equals(ResourceID other) => Value == other.Value;
}

[System.Flags]
public enum ResourceTag : int
{
    None          = 0,
    Edible        = 1 << 0,
    Fuel          = 1 << 1
}