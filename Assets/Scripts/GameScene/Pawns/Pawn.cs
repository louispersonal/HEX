using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : ITickable
{
    public TickableType TickableType => TickableType.Simulator;

    public virtual void Tick(TickInfo tickInfo)
    {
        Upkeep(tickInfo);
    }

    protected virtual void Upkeep(TickInfo tickInfo)
    {
        
    }
}
