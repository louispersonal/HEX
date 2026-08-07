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
        int simulationLength = simulationLengthYears * 365;
        TickInfo newTickInfo =  new TickInfo();
        Ticker newTicker = new Ticker(newTickInfo);
        GameController.Instance.SessionManager.NewGameData();
        GameController.Instance.SessionManager.GameData.Ticker = newTicker;

        Pop seedPop = new Pop();
        PlaceSeedPop(seedPop);
        PopBrain seedPopBrain = new PopBrain(seedPop);

        GameController.Instance.SessionManager.GameData.Pops.Add(seedPop.Location, seedPop);
        GameController.Instance.SessionManager.GameData.Ticker.Register(seedPopBrain);
        GameController.Instance.SessionManager.GameData.Ticker.Register(seedPop);
        
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
    
    private void PlaceSeedPop(Pop pop)
    {
        float optimumTemp = 0.7f;
        float optimumPrec = 0.5f;
        (float meanError, HexData hex) optimumHex = (float.MaxValue, null);
        foreach (var hexData in GameController.Instance.SessionManager.WorldData.Grid.GetValidHexes())
        {
            float tempError = hexData.ExtraData.Temperature - optimumTemp;
            float precError = hexData.ExtraData.Precipitation - optimumPrec;
            float euclidean = Mathf.Sqrt(Mathf.Pow(tempError, 2) + Mathf.Pow(precError, 2));

            if (euclidean <= optimumHex.meanError) optimumHex = (euclidean, hexData);
        }

        pop.Location = optimumHex.hex.Coord;
    }
}
