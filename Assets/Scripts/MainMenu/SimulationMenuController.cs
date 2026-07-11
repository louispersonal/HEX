using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationMenuController : SubMenu
{
    public void StartGame()
    {
        GameController.Instance.GoToScene(SceneNames.Game);
    }

    public void Simulate(int simulationLengthYears, LoadingPanel loadingPanel)
    {
        int simulationLength = simulationLengthYears * 365;
        TickInfo newTickInfo =  new TickInfo();
        Ticker newTicker = new Ticker(newTickInfo);
        GameController.Instance.SessionManager.NewGameData();
        GameController.Instance.SessionManager.GameData.Ticker = newTicker;

        Pop seedPop = PlaceSeedPop();
        PopBrain seedPopBrain = new PopBrain(seedPop);
        
        GameController.Instance.SessionManager.GameData.Ticker.Register(seedPopBrain);
        
        StartCoroutine(SimulationCoroutine(simulationLength, loadingPanel));
    }

    private IEnumerator SimulationCoroutine(int simulationLength, LoadingPanel loadingPanel)
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
                yield return null;
            }
        }
    }
    
    private Pop PlaceSeedPop()
    {
        WorldData world = GameController.Instance.SessionManager.WorldData;
        HexData currentOptimumHex = null;
        foreach (var hexData in world.Grid.GetValidHexes())
        {
            if (hexData.ExtraData.Biome == Biome.Savanna)
            {
                currentOptimumHex = hexData;
            }
        }
        
        return new Pop(currentOptimumHex.Coord, new CultureID(), new ReligionID());
    }
}
