using System.Collections.Generic;

public class Brain : IDecisionTick
{
    public int Order => 0;
    
    private readonly Pawn _pawn;
    public Pawn Pawn => _pawn;
    
    public Brain(Pawn pawn)
    {
        _pawn = pawn;
        _pawn.SetBrain(this);
    }
    
    public virtual void DecisionTick(TickInfo tickInfo)
    {
        
    }
}
