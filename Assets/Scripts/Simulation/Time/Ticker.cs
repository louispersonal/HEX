using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;

public class Ticker
{
    /*
     * Might be worth considering changing the tick frequency for the upkeep and simulation steps
     * Only gather / eat / take update stockpiles once a month
     */
    public TickInfo TickInfo { get;  private set; }

    private HashSet<IDecisionTick> _decisionTickables = new();
    private HashSet<IJobTickable> _jobTickables = new();
    private HashSet<IAssignmentTick> _assignmentTickables = new();
    private HashSet<IResolutionTick> _resolutionTickables = new();
    private HashSet<IUpkeepTick> _upkeepTickables = new();
    private HashSet<IUITickable> _uiTickables = new();
    
    private readonly HashSet<ITickable> _pendingRegistration;
    private readonly HashSet<ITickable> _pendingRemoval;
    
    private bool _isTicking;
    
    public Ticker(TickInfo tickInfo)
    {
        TickInfo = tickInfo;
        
        _pendingRegistration = new HashSet<ITickable>();
        _pendingRemoval = new HashSet<ITickable>();
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

        if (tickable is IJobTickable job)
        {
            _jobTickables.Add(job);
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

        if (tickable is IUITickable uiTick)
        {
            _uiTickables.Add(uiTick);
        }
    }

    private void InstantRemove(ITickable tickable)
    {
        if (tickable is IDecisionTick decision)
        {
            _decisionTickables.Remove(decision);
        }
        
        if (tickable is IJobTickable job)
        {
            _jobTickables.Remove(job);
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
        
        if (tickable is IUITickable uiTick)
        {
            _uiTickables.Remove(uiTick);
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
        foreach (IJobTickable jobTick in _jobTickables)
        {
            jobTick.JobTick(TickInfo);
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
        
        _isTicking = true;
        foreach (IUITickable uiTick in _uiTickables)
        {
            uiTick.UITick(TickInfo);
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