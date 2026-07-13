using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pop
{
    private AxialCoordinate _location;
    public AxialCoordinate Location => _location;
    private MovementData _movementData;
    public MovementData MovementData => _movementData;

    private CultureID _cultureId;
    public CultureID CultureId => _cultureId;
    
    private ReligionID _religionId;
    public ReligionID ReligionId => _religionId;

    public HexData HexData => GetCurrentHex();
    
    private float _cultureDecay;
    
    public Pop(AxialCoordinate location, CultureID cultureId, ReligionID religionId)
    {
        _location = location;
        _movementData = new MovementData();
        _cultureId = cultureId;
        _religionId = religionId;
        _cultureDecay = 1.0f;
    }
    
    public void AdvanceMovement()
    {
        if (_movementData.Path.Count == 0) return;
        AxialCoordinate nextLocation = _movementData.Path.Dequeue();
        _location = nextLocation;
    }
    
    public bool TryMigrate(AxialCoordinate target)
    {
        return TrySetPath(target);
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

    private HexData GetCurrentHex()
    {
        GameController.Instance.SessionManager.WorldData.Grid.TryGetHex(_location, out var hex);
        return hex;
    }
}

public class MovementData
{
    public AxialCoordinate Destination;
    public Queue<AxialCoordinate> Path = new();
}