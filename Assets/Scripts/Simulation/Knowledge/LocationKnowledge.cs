public readonly struct LocationKnowledge
{
    public TickInfo LastSeen { get; }
    
    public LocationKnowledge(TickInfo lastSeen)
    {
        LastSeen = lastSeen;
    }
}