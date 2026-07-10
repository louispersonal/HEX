using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pop
{
    private AxialCoordinate _location;
    private MovementData _movementData;

    private CultureID _cultureId;
    public CultureID CultureId => _cultureId;
    
    private ReligionID _religionId;
    public ReligionID ReligionId => _religionId;

    private float _cultureDecay;
    
    public Pop(AxialCoordinate location, CultureID cultureId, ReligionID religionId)
    {
        _location = location;
        _movementData = new MovementData();
        _cultureId = cultureId;
        _religionId = religionId;
        _cultureDecay = 1.0f;
    }
    
    private void Move(AxialCoordinate neighbor)
    {
        _location = neighbor;
    }
    
    private void Migrate(AxialCoordinate target)
    {
        TrySetPath(target);
    }

    private bool TrySetPath(AxialCoordinate target)
    {
        List<AxialCoordinate> path = GameController.Instance.SessionManager.WorldData.PathFinder.AStar(_location, target);
        if (path == null) return false;
        _movementData.Path = new Queue<AxialCoordinate>(path);
        _movementData.Destination = target;
        return true;
    }
    
    private void SwitchCulture(CultureID cultureId)
    {
        _cultureId = cultureId;
    }
}

public class MovementData
{
    public AxialCoordinate Destination;
    public Queue<AxialCoordinate> Path;
}