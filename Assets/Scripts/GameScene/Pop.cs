using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Pop : Pawn
{
    public float GatheringEfficiency = 1f;

    public string Name;
    
    public int Population { get; private set; }

    public string Faction;

    public CultureID CultureID;
    
    public Culture Culture => GameController.Instance.SessionManager.GameData.Cultures[CultureID];
    
    public ReligionID ReligionID;
    
    public Religion Religion => GameController.Instance.SessionManager.GameData.Religions[ReligionID];
    
    public AxialCoordinate Location;

    public HexData CurrentHex =>
        GameController.Instance.SessionManager.WorldData.Grid.GetHex(Location);
    
    private List<Assignment> _assignments = new();
    
    public List<Assignment> Assignments => _assignments;

    public ResourceBundle Stockpile {get; private set;}
    
    public Pop(string name, int startingPopulation, CultureID culure, ReligionID religion)
    {
        Name = name;
        Population = startingPopulation;
        CultureID = culure;
        ReligionID = religion;
        Stockpile = new ResourceBundle(BundleType.Stockpile);
    }
    
    public override void Tick(TickInfo tickInfo)
    {
        foreach (var assignment in _assignments)
        {
            assignment.Tick(this);
        }
        
        base.Tick(tickInfo);
    }

    public int CheckAssignmentNumbers()
    {
        int assignedWorkers = 0;

        foreach (var assignment in _assignments)
        {
            assignedWorkers += assignment.Workers;
        }
        
        return Population - assignedWorkers;
    }

    public void CreateGatherAssignment(int workers)
    {
        var gather = new GatherAssignment(workers);
        _assignments.Add(gather);
    }

    protected override void Upkeep(TickInfo tickInfo)
    {
        EatUpkeep();
        base.Upkeep(tickInfo);
    }

    private void EatUpkeep()
    {

    }
}