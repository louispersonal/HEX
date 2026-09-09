using System.Collections.Generic;

public class MultiObjectSpatialLookup<TId, TObject>
{
    private readonly Dictionary<TId, TObject> _objects = new();
    private readonly Dictionary<AxialCoordinate, HashSet<TId>> _idsByCoordinate = new();

    public IReadOnlyDictionary<TId, TObject> Objects => _objects;

    public bool TryAdd(TId id, TObject obj, IEnumerable<AxialCoordinate> coordinates)
    {
        if (!_objects.TryAdd(id, obj)) return false;

        foreach (AxialCoordinate coordinate in coordinates)
        {
            AddToCoordinate(id, coordinate);
        }

        return true;
    }

    public bool TryGetObject(TId id, out TObject obj)
    {
        return _objects.TryGetValue(id, out obj);
    }

    public IEnumerable<TObject> GetObjectsAt(AxialCoordinate coordinate)
    {
        if (!_idsByCoordinate.TryGetValue(coordinate, out var ids)) yield break;

        foreach (TId id in ids)
        {
            if (_objects.TryGetValue(id, out TObject obj)) yield return obj;
        }
    }

    public bool TryMove(TId id, AxialCoordinate origin, AxialCoordinate destination)
    {
        if (!_objects.ContainsKey(id)) return false;

        if (!_idsByCoordinate.TryGetValue(origin, out var originIds)) return false;

        if (!originIds.Remove(id)) return false;

        if (originIds.Count == 0) _idsByCoordinate.Remove(origin);

        AddToCoordinate(id, destination);
        return true;
    }

    public bool Remove(TId id)
    {
        if (!_objects.Remove(id)) return false;

        List<AxialCoordinate> emptyCoordinates = new();

        foreach (var pair in _idsByCoordinate)
        {
            pair.Value.Remove(id);

            if (pair.Value.Count == 0) emptyCoordinates.Add(pair.Key);
        }

        foreach (AxialCoordinate coordinate in emptyCoordinates)
        {
            _idsByCoordinate.Remove(coordinate);
        }

        return true;
    }

    private void AddToCoordinate(TId id, AxialCoordinate coordinate)
    {
        if (!_idsByCoordinate.TryGetValue(coordinate, out var ids))
        {
            ids = new HashSet<TId>();
            _idsByCoordinate.Add(coordinate, ids);
        }

        ids.Add(id);
    }
}
