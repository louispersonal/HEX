using System.Collections.Generic;
using System;

public class PopCollection
{
    private readonly MultiObjectSpatialLookup<PopID, Pop> _spatial = new();

    private ushort _nextID;
    
    public IReadOnlyDictionary<PopID, Pop> All => _spatial.Objects;

    public event Action<Pop> PopAdded;
    public event Action<Pop, AxialCoordinate, AxialCoordinate> PopMoved;
    public event Action<Pop> PopRemoved;

    public bool TryCreateNewPop(string name, int startingPopulation, AxialCoordinate startingCoordinate,
        CultureID culture, ReligionID religion, out Pop pop)
    {
        PopID id = new(_nextID++);
        pop = new Pop(name, id, startingPopulation, startingCoordinate, culture, religion);

        if (!_spatial.TryAdd(pop.ID, pop, new[] { pop.Location }))
        {
            pop = null;
            return false;
        }

        PopAdded?.Invoke(pop);
        return true;
    }

    public bool TryMove(Pop pop, AxialCoordinate destination)
    {
        AxialCoordinate origin = pop.Location;

        if (!AxialGeometry.AreAdjacent(origin, destination)) return false;
        
        if (!_spatial.TryMove(pop.ID, origin, destination)) return false;

        pop.SetLocation(destination);
        PopMoved?.Invoke(pop, origin, destination);
        return true;
    }
    
    public bool TryRemove(PopID id)
    {
        if (!_spatial.TryGetObject(id, out Pop pop)) return false;

        if (!_spatial.Remove(id)) return false;

        PopRemoved?.Invoke(pop);
        return true;
    }
    
    public IEnumerable<Pop> GetAt(AxialCoordinate coordinate)
    {
        return _spatial.GetObjectsAt(coordinate);
    }
}
