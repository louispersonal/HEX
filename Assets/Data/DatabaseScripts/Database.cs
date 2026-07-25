using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Database<TKey, TItem> : ScriptableObject 
    where TItem : IDatabaseItem<TKey> 
    where TKey : IEquatable<TKey>
{
    [SerializeField]
    private TItem[] _items;

    private Dictionary<TKey, TItem> _lookup;

    public TItem Get(TKey id)
    {
        EnsureLookup();
        return _lookup[id];
    }

    public bool TryGet(TKey id, out TItem item)
    {
        EnsureLookup();
        return _lookup.TryGetValue(id, out item);
    }

    public IReadOnlyList<TItem> Items => _items;

    private void EnsureLookup()
    {
        if (_lookup != null)
            return;

        _lookup = new Dictionary<TKey, TItem>(_items.Length);

        foreach (TItem item in _items)
        {
            _lookup.Add(item.Id, item);
        }
    }
}