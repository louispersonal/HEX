using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ticker
{
    public TickInfo TickInfo { get;  private set; }

    private readonly List<ITickable> _brains;
    private readonly List<ITickable> _simulators;
    
    private readonly List<ITickable> _pendingRegistration;
    private readonly List<ITickable> _pendingRemoval;
    
    private bool _isTicking;
    
    public Ticker(TickInfo tickInfo)
    {
        TickInfo = tickInfo;
        _brains = new List<ITickable>();
        _simulators = new List<ITickable>();
        
        _pendingRegistration = new List<ITickable>();
        _pendingRemoval = new List<ITickable>();
    }

    public void Register(ITickable tickable)
    {
        switch (tickable.TickableType)
        {
            case TickableType.Brain:
                _brains.Add(tickable);
                break;
            case TickableType.Simulator:
                _simulators.Add(tickable);
                break;
        }
    }

    public void ProgressTick()
    {
        _isTicking = true;
        
        TickInfo.Increment();
        
        foreach (ITickable brain in _brains)
        {
            brain.Tick(TickInfo);
        }
        
        foreach (ITickable simulator in _simulators)
        {
            simulator.Tick(TickInfo);
        }
        
        _isTicking = false;
    }
}

public enum TickableType
{
    Brain,
    Simulator
}