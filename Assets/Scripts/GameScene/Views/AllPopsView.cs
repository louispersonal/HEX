using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class AllPopsView : MonoBehaviour
{
    [SerializeField] private PopView _popViewPrefab;
    
    ObjectPool<PopView> _popPool;

    private Dictionary<AxialCoordinate, PopView> _livePops;

    private void Awake()
    {
        _popPool = new ObjectPool<PopView>(CreatePop, OnTakeFromPool, OnReturnedToPool, OnDestroyPooledObject,  collectionCheck:false, defaultCapacity:100, maxSize:500);
        _livePops = new Dictionary<AxialCoordinate, PopView>();
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
        
    }

    public void SpawnPop(Pop data)
    {
        PopView pop = _popPool.Get();
        pop.Initialize(data);
        _livePops.Add(data.Location, pop);
    }

    public void DeSpawnPop(AxialCoordinate coord)
    {
        _livePops[coord].Terminate();
        _popPool.Release(_livePops[coord]);
        _livePops.Remove(coord);
    }
}
