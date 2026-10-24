using System;
using UnityEngine;

[Serializable]
public class Faction : IIndexedItem<FactionID>
{
    [SerializeField] private FactionID _id;
    public FactionID ID => _id;
    
    public string Name;
}

[Serializable]
public struct FactionID : IEquatable<FactionID>
{
    public int Value;
    public FactionID(int value) => Value = value;

    public Faction Faction => GameController.Instance.SessionManager.GameData.Factions.GetValue(this);

    public bool Equals(FactionID other)
    {
        return Value == other.Value;
    }

    public override bool Equals(object obj)
    {
        return obj is FactionID other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Value;
    }
}