using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : ITickable
{
    public int Order => 1;
    
    public virtual void Tick(TickInfo tickInfo)
    {
        Upkeep(tickInfo);
    }

    protected virtual void Upkeep(TickInfo tickInfo)
    {
        
    }
}
