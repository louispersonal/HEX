using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class River : ISpatialObject<RiverID>
{
    [SerializeField] private RiverID id;
    public RiverID ID => id;
    
    [SerializeField] private List<AxialCoordinate> coords;
    public List<AxialCoordinate> Coords => coords;
    
    public Dictionary<AxialCoordinate, byte> RiverConnections;
    public AxialCoordinate Source;

    public River (RiverID iD, AxialCoordinate source)
    {
        Source = source;
        coords = new List<AxialCoordinate>();
        coords.Add(source);
        id = iD;
    }

    public void PopulateRiverConnections()
    {
        RiverConnections = new Dictionary<AxialCoordinate, byte>(coords.Count);
        
        foreach (AxialCoordinate coord in coords)
        {
            RiverConnections[coord] = 0;
        }
        
        for (int i = 0; i < coords.Count - 1; i++)
        {
            AxialCoordinate current = coords[i];
            AxialCoordinate next = coords[i + 1];

            AxialCardinalDirections directionToNext = AxialGeometry.GetDirection(current, next);

            AddConnection(current, directionToNext);
            // add connection in reverse
            AddConnection(next, AxialGeometry.GetOppositeDirection(directionToNext));
        }
    }
    
    private void AddConnection(AxialCoordinate currentCoord, AxialCardinalDirections direction)
    {
        RiverConnections[currentCoord] |= (byte)(1 << (int)direction);
    }

    public bool CheckConnection(AxialCoordinate currentCoord, AxialCardinalDirections direction)
    {
        return (RiverConnections[currentCoord] & (1 << (int)direction)) != 0;
    }

    public List<AxialCardinalDirections> GetConnections(AxialCoordinate currentCoord)
    {
        List<AxialCardinalDirections> connections = new List<AxialCardinalDirections>();
        
        for (int d = 0; d < 6; d++)
        {
            if (CheckConnection(currentCoord, (AxialCardinalDirections)d)) connections.Add((AxialCardinalDirections)d);
        }

        return connections;
    }
}

[System.Serializable]
public struct RiverID
{
    public int Value;

    public RiverID(int value)
    {
        Value = value;
    }
}