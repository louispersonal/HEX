using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationMenuController : SubMenu
{
    public void StartGame()
    {
        GameController.Instance.GoToScene(SceneNames.Game);
    }
}
