using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Culture : IIndexedItem<CultureID>
{
    [SerializeField] private CultureID _id;
    public CultureID ID => _id;
    public string Name;

    public CultureID? ParentID;
    public List<CultureID> Children;

    public int Depth;
    
    public Dictionary<ResourceID, float> GatheringProficiency { get; private set; }

    public Culture(CultureID id, string name)
    {
        _id = id;
        Name = name;
        GatheringProficiency = new Dictionary<ResourceID, float>()
        {
            { ResourceRefs.Greens, 0.2f },
            { ResourceRefs.Fruit, 0.2f },
            { ResourceRefs.Fungus, 0.2f },
            { ResourceRefs.Seeds, 0.2f },
            { ResourceRefs.Roots, 0.2f },
            { ResourceRefs.Grubs, 0.2f }
        };
    }
}

[Serializable]
public struct CultureID : IEquatable<CultureID>
{
    public int Value;
    public CultureID(int value) => Value = value;

    public Culture Culture => GameController.Instance.SessionManager.GameData.Cultures.GetValue(this);

    public bool Equals(CultureID other)
    {
        return Value == other.Value;
    }

    public override bool Equals(object obj)
    {
        return obj is CultureID other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Value;
    }
}