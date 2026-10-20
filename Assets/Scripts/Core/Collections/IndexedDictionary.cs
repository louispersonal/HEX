using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

public class IndexedDictionary<TId, T>where T : IIndexedItem<TId>
{
    private Dictionary<TId, T> _dictionary = new();
    
    public IndexedDictionary() { }

    public IndexedDictionary(T[] values)
    {
        foreach (T value in values) TryAdd(value);
    }

    public T GetValue(TId key)
    {
        return _dictionary[key];
    }

    public bool TryGetValue(TId key, out T value)
    {
        return _dictionary.TryGetValue(key, out value);
    }
    
    public bool TryAdd(T value)
    {
        return _dictionary.TryAdd(value.ID, value);
    }

    public T[] ToArray()
    {
        return _dictionary.Values.ToArray();
    }
    
    public IEnumerable<T> Values()
    {
        return _dictionary.Values;
    }
}

public interface IIndexedItem<TId>
{
    TId ID { get; }
}