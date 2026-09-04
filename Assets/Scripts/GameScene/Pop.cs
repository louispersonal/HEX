using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;

public class Pop : Pawn, IAssignmentTick
{
    public string Name;
    
    public int Population => WorkingPopulation + PersonsOfInterest.Count;
    
    public int WorkingPopulation { get; private set; }
    
    public List<PersonOfInterest> PersonsOfInterest { get; private set; }

    public string Faction;

    public CultureID CultureID;
    
    public Culture Culture => GameController.Instance.SessionManager.GameData.Cultures[CultureID];
    
    public ReligionID ReligionID;
    
    public Religion Religion => GameController.Instance.SessionManager.GameData.Religions[ReligionID];
    
    public AxialCoordinate Location;

    public Hex CurrentHex =>
        GameController.Instance.SessionManager.WorldData.Grid.GetHex(Location);
    
    private List<Assignment> _assignments = new();
    
    public IReadOnlyList<Assignment> Assignments => _assignments;

    public ResourceStockpile Stockpile {get; private set;}
    
    private bool _isStarving = false;
    
    public Pop(string name, int startingPopulation, CultureID culture, ReligionID religion)
    {
        Name = name;
        WorkingPopulation = startingPopulation - 1; //subtract leader
        CultureID = culture;
        ReligionID = religion;
        Stockpile = new ResourceStockpile(new ResourceCollection(), this);
        PersonsOfInterest = new List<PersonOfInterest>();
        PersonOfInterest leader = new PersonOfInterest("Kiko", PersonOfInterestRole.Leader);
        PersonsOfInterest.Add(leader);
    }
    
    public void AssignmentTick(TickInfo tickInfo)
    {
        foreach (var assignment in _assignments)
        {
            assignment.Tick(this);
        }
    }

    public int WorkerSurplus()
    {
        int assignedWorkers = 0;

        foreach (var assignment in _assignments)
        {
            assignedWorkers += assignment.Workers;
        }
        
        return WorkingPopulation - assignedWorkers;
    }

    public void CreateGatherAssignment(int workers)
    {
        if (workers > WorkerSurplus()) return;
        var gather = new GatherAssignment(workers);
        _assignments.Add(gather);
    }
    
    public override void UpkeepTick(TickInfo tickInfo)
    {
        EatUpkeep();
        PopGrowth();
        base.UpkeepTick(tickInfo);
    }

    private void EatUpkeep()
    {
        float nutritionRemaining = PopBasicData.BaseNutrition * Population;

        var stockPreview = Stockpile.GetPreview();
        ResourceCollection requestCollection = new();
        
        List<ResourceID> availableFoods = stockPreview.Contents.GetAllResourceIDs().Where(resource =>
                resource.Definition.IsEdible && stockPreview.Contents.Get(resource) > 0f).ToList();

        const float epsilon = 0.001f;

        while (nutritionRemaining > epsilon && availableFoods.Count > 0)
        {
            float nutritionShare = nutritionRemaining / availableFoods.Count;

            for (int i = availableFoods.Count - 1; i >= 0; i--)
            {
                ResourceID food = availableFoods[i];
                float available = stockPreview.Contents.Get(food);
                float amountRequested = Mathf.Min(available, nutritionShare);

                stockPreview.Contents.Withdraw(food, amountRequested, out float amountRemoved);

                requestCollection.Deposit(food, amountRemoved);
                nutritionRemaining -= amountRemoved;

                if (stockPreview.Contents.Get(food) <= epsilon) availableFoods.RemoveAt(i);
            }
        }

        bool receivedEnoughNutrition = nutritionRemaining <= epsilon;

        bool requestFulfilled = Stockpile.Consume(new ResourceRequest(requestCollection, this));

        _isStarving = !receivedEnoughNutrition || !requestFulfilled;
    }

    private void PopGrowth()
    {
        if (_isStarving) return;
    }
}