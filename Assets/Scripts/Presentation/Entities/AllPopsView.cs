using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class AllPopsView : MonoBehaviour
{
    [SerializeField] private PopView _popViewPrefab;
    
    private ObjectPool<PopView> _popPool;

    private readonly Dictionary<PopID, PopView> _livePops = new();

    private PopCollection Pops => GameController.Instance.SessionManager.GameData.Pops;
    
    private void Awake()
    {
        _popPool = new ObjectPool<PopView>(CreatePop, OnTakeFromPool, OnReturnedToPool, OnDestroyPooledObject,  collectionCheck:false, defaultCapacity:100, maxSize:500);
    }

    private void Start()
    {
        Pops.PopAdded += HandlePopAdded;
        Pops.PopMoved += HandlePopMoved;
        Pops.PopRemoved += HandlePopRemoved;
        
        foreach (Pop pop in Pops.All.Values)
        {
            SpawnPop(pop);
        }
    }

    private void OnDestroy()
    {
        Pops.PopAdded -= HandlePopAdded;
        Pops.PopMoved -= HandlePopMoved;
        Pops.PopRemoved -= HandlePopRemoved;
    }
    
    private PopView CreatePop()
    {
        PopView popView = Instantiate(_popViewPrefab, transform);
        popView.gameObject.SetActive(false);
        return popView;
    }

    private void OnTakeFromPool(PopView pop)
    {
        pop.gameObject.SetActive(true);
    }

    private void OnReturnedToPool(PopView pop)
    {
        pop.gameObject.SetActive(false);
    }

    private void OnDestroyPooledObject(PopView pop)
    {
        Destroy(pop.gameObject);
    }

    private void HandlePopAdded(Pop pop)
    {
        SpawnPop(pop);
    }

    private void HandlePopMoved(
        Pop pop,
        AxialCoordinate origin,
        AxialCoordinate destination)
    {
        if (_livePops.TryGetValue(pop.ID, out PopView view))
        {
            view.MoveTo(destination);
        }
    }

    private void HandlePopRemoved(Pop pop)
    {
        DespawnPop(pop.ID);
    }
    
    private void SpawnPop(Pop pop)
    {
        if (_livePops.ContainsKey(pop.ID)) return;

        PopView view = _popPool.Get();
        view.Initialize(pop);
        _livePops.Add(pop.ID, view);
    }

    private void DespawnPop(PopID id)
    {
        if (!_livePops.Remove(id, out PopView view)) return;

        view.Terminate();
        _popPool.Release(view);
    }
}
