using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Culture
{
    public CultureID ID;
    public string Name;

    public CultureID? ParentID;
    public List<CultureID> Children;

    public int Depth;
    
    public Dictionary<ResourceID, float> GatheringProficiency { get; private set; }

    public Culture(CultureID culture)
    {
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

public readonly struct CultureID : IEquatable<CultureID>
{
    public readonly int Value;
    public CultureID(int value) => Value = value;

    public Culture Culture => GameController.Instance.SessionManager.GameData.Cultures[this];

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