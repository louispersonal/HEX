using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ticker
{
    /*
     * Might be worth considering changing the tick frequency for the upkeep and simulation steps
     * Only gather / eat / take update stockpiles once a month
     */
    public TickInfo TickInfo { get;  private set; }

    private List<List<ITickable>> _tickables;
    
    private readonly List<ITickable> _pendingRegistration;
    private readonly List<ITickable> _pendingRemoval;
    
    private bool _isTicking;
    
    public Ticker(TickInfo tickInfo)
    {
        TickInfo = tickInfo;
        _tickables = new List<List<ITickable>>();
        _tickables.Add(new List<ITickable>());
        _tickables.Add(new List<ITickable>());
        _tickables.Add(new List<ITickable>());
        
        _pendingRegistration = new List<ITickable>();
        _pendingRemoval = new List<ITickable>();
    }

    public void Register(ITickable tickable)
    {
        if (!_isTicking) InstantRegister(tickable);
        else
        {
            _pendingRegistration.Add(tickable);
        }
    }

    public void Remove(ITickable tickable)
    {
        if (!_isTicking) InstantRemove(tickable);
        else
        {
            _pendingRemoval.Add(tickable);
        }
    }
    
    private void InstantRegister(ITickable tickable)
    {
        if (!_tickables[tickable.Order].Contains(tickable))
            _tickables[tickable.Order].Add(tickable);
    }

    private void InstantRemove(ITickable tickable)
    {
        if  (_tickables[tickable.Order].Contains(tickable))
            _tickables[tickable.Order].Remove(tickable);
    }

    public void ProgressTick()
    {
        TickInfo.Increment();

        foreach (List<ITickable> TickPhase in _tickables)
        {
            _isTicking = true;
            foreach (ITickable tickable in TickPhase)
            {
                tickable.Tick(TickInfo);
            }
            _isTicking = false;
            SyncPending();
        }
    }

    private void SyncPending()
    {
        foreach (ITickable pending in _pendingRegistration)
        {
            InstantRegister(pending);
        }

        foreach (ITickable registration in _pendingRemoval)
        {
            InstantRemove(registration);
        }
        
        _pendingRegistration.Clear();
        _pendingRemoval.Clear();
    }
}