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

[Serializable]
public readonly struct ResourceID : IEquatable<ResourceID>
{
    public readonly ushort Value;

    public ResourceID(ushort value)
    {
        Value = value;
    }

    public bool Equals(ResourceID other) => Value == other.Value;

    public override bool Equals(object obj) => obj is ResourceID other && Equals(other);

    public override int GetHashCode() => Value.GetHashCode();

    public static bool operator ==(ResourceID left, ResourceID right) => left.Equals(right);

    public static bool operator !=(ResourceID left, ResourceID right) => !left.Equals(right);
}

[Flags]
public enum ResourceTag : int
{
    None          = 0,
    Edible        = 1 << 0,
    Fuel          = 1 << 1
}