using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpatialLookup<TId, TObject>
{
    public Dictionary<TId, TObject> Objects { get; } = new();
    public Dictionary<AxialCoordinate, TId> Lookup { get; } = new();

    public void Add(TId id, TObject obj, IEnumerable<AxialCoordinate> coordinates)
    {
        Objects[id] = obj;

        foreach (AxialCoordinate coord in coordinates)
        {
            Lookup[coord] = id;
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
