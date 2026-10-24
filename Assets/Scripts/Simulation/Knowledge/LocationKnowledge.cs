using UnityEngine;

[System.Serializable]
public struct LocationKnowledge
{
    [SerializeField] private TickInfo _lastSeen;
    public TickInfo LastSeen => _lastSeen;
    
    public LocationKnowledge(TickInfo lastSeen)
    {
        _lastSeen = lastSeen;
    }
}