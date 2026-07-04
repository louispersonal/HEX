using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationClock
{
    public TickInfo TickInfo;
    
    public List<ITickable> TickableRegsitry { get; private set; } = new List<ITickable>();
    
    public void ProgressTick()
    {
        foreach (var tickable in TickableRegsitry)
        {
            TickInfo.Increment();
            tickable.Tick(TickInfo);
        }
    }
}
