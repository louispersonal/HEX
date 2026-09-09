using System.Collections.Generic;

public class PopCollection
{
    private readonly MultiObjectSpatialLookup<PopID, Pop> _spatial = new();

    private ushort _nextID;
    
    public IReadOnlyDictionary<PopID, Pop> All => _spatial.Objects;

    public bool TryCreateNewPop(string name, int startingPopulation, AxialCoordinate startingCoordinate,
        CultureID culture, ReligionID religion, out Pop pop)
    {
        PopID id = new(_nextID++);
        pop = new Pop(name, id, startingPopulation, startingCoordinate, culture, religion);
        return TryAdd(pop);
    }
    
    private bool TryAdd(Pop pop)
    {
        return _spatial.TryAdd(pop.ID, pop, new[] { pop.Location });
    }

    public bool TryMove(Pop pop, AxialCoordinate destination)
    {
        AxialCoordinate origin = pop.Location;

        if (!AxialGeometry.AreAdjacent(origin, destination)) return false;
        
        if (!_spatial.TryMove(pop.ID, origin, destination)) return false;

        pop.SetLocation(destination);
        return true;
    }
    
    public IEnumerable<Pop> GetAt(AxialCoordinate coordinate)
    {
        return _spatial.GetObjectsAt(coordinate);
    }
}
