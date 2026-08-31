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

    private List<IDecisionTick> _decisionTickables = new();
    private List<IAssignmentTick> _assignmentTickables = new();
    private List<IResolutionTick> _resolutionTickables = new();
    private List<IUpkeepTick> _upkeepTickables = new();
    
    private readonly List<ITickable> _pendingRegistration;
    private readonly List<ITickable> _pendingRemoval;
    
    private bool _isTicking;
    
    public Ticker(TickInfo tickInfo)
    {
        TickInfo = tickInfo;
        
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
        if (tickable is IDecisionTick decision)
        {
            _decisionTickables.Add(decision);
        }
        
        if (tickable is IAssignmentTick assignment)
        {
            _assignmentTickables.Add(assignment);
        }
        
        if (tickable is IResolutionTick resolution)
        {
            _resolutionTickables.Add(resolution);
        }
        
        if (tickable is IUpkeepTick upkeepTick)
        {
            _upkeepTickables.Add(upkeepTick);
        }
    }

    private void InstantRemove(ITickable tickable)
    {
        if (tickable is IDecisionTick decision)
        {
            _decisionTickables.Remove(decision);
        }
        
        if (tickable is IAssignmentTick assignment)
        {
            _assignmentTickables.Remove(assignment);
        }
        
        if (tickable is IResolutionTick resolution)
        {
            _resolutionTickables.Remove(resolution);
        }
        
        if (tickable is IUpkeepTick upkeepTick)
        {
            _upkeepTickables.Remove(upkeepTick);
        }
    }

    public void ProgressTick()
    {
        TickInfo.Increment();
        
        _isTicking = true;
        foreach (IDecisionTick decisionTick in _decisionTickables)
        {
            decisionTick.DecisionTick(TickInfo);
        }
        _isTicking = false;
        SyncPending();
        
        _isTicking = true;
        foreach (IAssignmentTick assignmentTick in _assignmentTickables)
        {
            assignmentTick.AssignmentTick(TickInfo);
        }
        _isTicking = false;
        SyncPending();
        
        _isTicking = true;
        foreach (IResolutionTick resolutionTick in _resolutionTickables)
        {
            resolutionTick.ResolutionTick(TickInfo);
        }
        _isTicking = false;
        SyncPending();
        
        _isTicking = true;
        foreach (IUpkeepTick upkeepTick in _upkeepTickables)
        {
            upkeepTick.UpkeepTick(TickInfo);
        }
        _isTicking = false;
        SyncPending();
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