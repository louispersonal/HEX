using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpatialLookup<TId, TObject> where TObject : ISpatialObject<TId>
{
    public Dictionary<TId, TObject> Objects { get; } = new();
    public Dictionary<AxialCoordinate, TId> Lookup { get; } = new();

    public SpatialLookup() { }
    
    public SpatialLookup(TObject[] objects)
    {
        foreach (TObject obj in objects) Add(obj);
    }

    public TObject[] ToArray()
    {
        TObject[] array = new TObject[Objects.Count];
        Objects.Values.CopyTo(array, 0);
        return array;
    }
    
    public void Add(TObject obj)
    {
        Objects[obj.ID] = obj;

        foreach (AxialCoordinate coord in obj.Coords)
        {
            Lookup[coord] = obj.ID;
        }
    }

    public bool TryGetObject(TId id, out TObject obj)
    {
        return Objects.TryGetValue(id, out obj);
    }

    public bool TryGetIdAt(AxialCoordinate coord, out TId id)
    {
        return Lookup.TryGetValue(coord, out id);
    }

    public bool TryGetObjectAt(AxialCoordinate coord, out TObject obj)
    {
        obj = default;

        if (!Lookup.TryGetValue(coord, out TId id))
            return false;

        return Objects.TryGetValue(id, out obj);
    }

    public bool ContainsAt(AxialCoordinate coord)
    {
        return Lookup.ContainsKey(coord);
    }

    public void Remove(TId id)
    {
        if (!Objects.Remove(id))
            return;

        // remove all matching lookup entries
        List<AxialCoordinate> toRemove = new();

        foreach (var pair in Lookup)
        {
            if (EqualityComparer<TId>.Default.Equals(pair.Value, id))
                toRemove.Add(pair.Key);
        }

        foreach (AxialCoordinate coord in toRemove)
        {
            Lookup.Remove(coord);
        }
    }
}

public interface ISpatialObject<TId>
{
    TId ID { get; }
    List<AxialCoordinate> Coords { get; }
}