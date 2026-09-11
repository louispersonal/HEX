using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationMenuController : SubMenu
{
    public void StartGame()
    {
        GameController.Instance.GoToScene(SceneNames.Game);
    }

    public void Simulate(int simulationLengthYears, LoadingPanel loadingPanel, Action updateView)
    {
        InitializeWorld();
        
        SeedSimulation();
        
        int simulationLength = simulationLengthYears * 365;
        StartCoroutine(SimulationCoroutine(simulationLength, loadingPanel, updateView));
    }

    private IEnumerator SimulationCoroutine(int simulationLength, LoadingPanel loadingPanel, Action updateView)
    {
        int chunkSize = 200 * 365;
        int chunkProgress = 0;
        while (GameController.Instance.SessionManager.GameData.Ticker.TickInfo.TickCount < simulationLength)
        {
            GameController.Instance.SessionManager.GameData.Ticker.ProgressTick();
            float tickCount = GameController.Instance.SessionManager.GameData.Ticker.TickInfo.TickCount;
            int year = Mathf.RoundToInt(tickCount / 365f);
            chunkProgress++;

            if (chunkProgress >= chunkSize)
            {
                loadingPanel.UpdateStatus(tickCount / (float) simulationLength, "Year " + year);
                chunkProgress = 0;
                updateView?.Invoke();
                yield return null;
            }
        }
    }

    private void InitializeWorld()
    {
        TickInfo newTickInfo =  new TickInfo();
        Ticker newTicker = new Ticker(newTickInfo);
        GameController.Instance.SessionManager.NewGameData();
        GameController.Instance.SessionManager.GameData.Ticker = newTicker;
    }

    private void SeedSimulation()
    {
        Pop seedPop = MakeSeedPop();
        
        //PopBrain seedPopBrain = new PopBrain(seedPop);
        //GameController.Instance.SessionManager.GameData.Ticker.Register(seedPopBrain);
        GameController.Instance.SessionManager.GameData.Ticker.Register(seedPop);
    }
    
    private Pop MakeSeedPop()
    {
        CultureID seedCultureID = new CultureID(0);
        ReligionID seedReligionID = new ReligionID(0);

        Culture seedCulture = new Culture(seedCultureID);
        seedCulture.Name = "Bogoma";

        Religion seedReligion = new Religion();
        seedReligion.Name = "Harmana";
        seedReligion.ID = seedReligionID;
        
        GameController.Instance.SessionManager.GameData.Cultures.Add(seedCultureID, seedCulture);
        GameController.Instance.SessionManager.GameData.Religions.Add(seedReligionID, seedReligion);
        
        AxialCoordinate seedLocation = FindSeedLocation();
        GameController.Instance.SessionManager.GameData.Pops.TryCreateNewPop("Bogoma", 30,
            seedLocation, seedCultureID, seedReligionID,
            out Pop seedPop);

        return seedPop;
    }
    
    private AxialCoordinate FindSeedLocation()
    {
        float optimumTemp = 0.7f;
        float optimumPrec = 0.5f;
        (float meanError, Hex hex) optimumHex = (float.MaxValue, null);
        foreach (var hexData in GameController.Instance.SessionManager.WorldData.Grid.GetValidHexes())
        {
            float tempError = hexData.ExtraData.Temperature - optimumTemp;
            float precError = hexData.ExtraData.Precipitation - optimumPrec;
            float euclidean = Mathf.Sqrt(Mathf.Pow(tempError, 2) + Mathf.Pow(precError, 2));

            if (euclidean <= optimumHex.meanError) optimumHex = (euclidean, hexData);
        }

        return optimumHex.hex.Coord;
    }
}
