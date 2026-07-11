using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ticker
{
    public TickInfo TickInfo { get;  private set; }
    
    public List<ITickable> Tickables { get; private set; }
    
    public Ticker(TickInfo tickInfo)
    {
        TickInfo = tickInfo;
        Tickables = new List<ITickable>();
    }

    public void Register(ITickable tickable)
    {
        Tickables.Add(tickable);
    }

    public void ProgressTick()
    {
        TickInfo.Increment();
        foreach (var tickable in Tickables)
        {
            tickable.Tick(TickInfo);
        }
    }
}
