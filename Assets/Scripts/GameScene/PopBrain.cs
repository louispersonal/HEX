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
        foreach (var scoutHex in HexGridGeometry.HexesInRingOfRadiusOfHex(world.Grid, _pop.HexData, 4))
        {
            if (_pop.TryMigrate(scoutHex.Coord)) break;
        }
    }
}
