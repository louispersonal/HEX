using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class Pop : Pawn, IAssignmentTick, ISavable<PopSaveData>
{
    // Serializable Data
    public string Name;
    public PopID ID { get; private set; }
    public int WorkingPopulation { get; private set; }
    public List<PersonOfInterest> PersonsOfInterest { get; private set; }
    public CultureID CultureID;
    public ReligionID ReligionID;
    public FactionID FactionID;
    private List<Assignment> _assignments;
    public ResourceStockpile Stockpile {get; private set;}
    private bool _isStarving = false;
    private SerializableDictionary<AxialCoordinate, LocationKnowledge> _knownLocations;

    // public accessors
    public int Population => WorkingPopulation + PersonsOfInterest.Count;
    public Culture Culture => GameController.Instance.SessionManager.GameData.Cultures.GetValue(CultureID);
    public Religion Religion => GameController.Instance.SessionManager.GameData.Religions.GetValue(ReligionID);
    public Faction Faction => GameController.Instance.SessionManager.GameData.Factions.GetValue(FactionID);
    public IReadOnlyList<Assignment> Assignments => _assignments;
    public ResourceDatabase ResourceDatabase => GameController.Instance.StaticDatabases.ResourceDatabase;
    
    public Pop(string name, PopID id, int startingPopulation, AxialCoordinate startLocation, 
                CultureID culture, ReligionID religion)
    {
        Name = name;
        ID = id;
        WorkingPopulation = startingPopulation - 1; //subtract leader
        Location =  startLocation;
        CultureID = culture;
        ReligionID = religion;
        Stockpile = new ResourceStockpile(new ResourceCollection(), this);
        
        PersonsOfInterest = new List<PersonOfInterest>();
        PersonOfInterest leader = new PersonOfInterest("GenericLeader", PersonOfInterestRole.Leader);
        PersonsOfInterest.Add(leader);
    }

    public Pop(PopSaveData saveData)
    {
        Name =  saveData.Name;
        ID = saveData.ID;
        WorkingPopulation = saveData.WorkingPopulation;
        PersonsOfInterest = saveData.PersonsOfInterest;
        CultureID = saveData.CultureID;
        ReligionID = saveData.ReligionID;
        FactionID = saveData.FactionID;
        _assignments = saveData.Assignments;
        Stockpile = new ResourceStockpile(new ResourceCollection(saveData.Stockpile), this);
        _isStarving = saveData.IsStarving;
        _knownLocations = new SerializableDictionary<AxialCoordinate, LocationKnowledge>(saveData.KnownLocations);
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
                stockPreview.Contents.Get(resource) > 0f 
                && ResourceDatabase.IsA(resource, ResourceRefs.Food)).ToList();

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

    public bool KnowsLocation(AxialCoordinate coord)
    {
        return _knownLocations.ContainsKey(coord);
    }

    public void ObserveLocation(AxialCoordinate coord, TickInfo lastSeen)
    {
        _knownLocations[coord] = new LocationKnowledge(lastSeen);
    }
    
    // Should only be called from PopCollection
    internal void SetLocation(AxialCoordinate location)
    {
        Location = location;
    }

    public PopSaveData ToSaveData()
    {
        return new PopSaveData(Name, ID,  WorkingPopulation, PersonsOfInterest, CultureID, ReligionID, FactionID, 
            _assignments, Stockpile.Contents.Resources.ToSaveData(), _isStarving, _knownLocations.ToSaveData());
    }
}

[System.Serializable]
public class PopSaveData
{
    public string Name;
    public PopID ID;
    public int WorkingPopulation;
    public List<PersonOfInterest> PersonsOfInterest;
    public CultureID CultureID;
    public ReligionID ReligionID;
    public FactionID FactionID;
    public List<Assignment> Assignments;
    public SerializableDictionarySaveData<ResourceID, float> Stockpile;
    public bool IsStarving;
    public SerializableDictionarySaveData<AxialCoordinate, LocationKnowledge> KnownLocations;

    public PopSaveData(string name, PopID id, int workingPopulation, List<PersonOfInterest> personsOfInterest,
        CultureID cultureID, ReligionID religionID,
        FactionID factionID, List<Assignment> assignments,
        SerializableDictionarySaveData<ResourceID, float> stockpile,
        bool isStarving,
        SerializableDictionarySaveData<AxialCoordinate, LocationKnowledge> knownLocations)
    {
        Name = name;
        ID = id;
        WorkingPopulation = workingPopulation;
        PersonsOfInterest = personsOfInterest;
        CultureID = cultureID;
        ReligionID = religionID;
        FactionID = factionID;
        Assignments = assignments;
        Stockpile = stockpile;
        IsStarving = isStarving;
        KnownLocations = knownLocations;
    }
}