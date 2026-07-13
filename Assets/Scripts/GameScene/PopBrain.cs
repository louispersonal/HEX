using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopBrain : ITickable
{   
    private readonly Pop _pop;
    public Pop Pop => _pop;

    public PopBrain(Pop pop)
    {
        _pop = pop;
    }
    public void Tick(TickInfo tickInfo)
    {
        if (Random.Range(0, 1000) <= 1 && _pop.MovementData.Path.Count == 0)
        {
            SetMigrationDestination();
        }
        _pop.AdvanceMovement();
    }

    private void SetMigrationDestination()
    {
        var world = GameController.Instance.SessionManager.WorldData;
        List<AxialCoordinate> candidates = new();
        foreach (var scoutHex in HexGridGeometry.HexesWithinRadiusOfHex(world.Grid, _pop.HexData, 4))
        {
            if (_pop.TryMigrate(scoutHex.Coord)) candidates.Add(scoutHex.Coord);
        }

        if (candidates.Count > 0) _pop.TryMigrate(candidates[Random.Range(0, candidates.Count)]);
    }
}
