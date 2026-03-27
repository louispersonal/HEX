using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGameMenuController : SubMenu
{
    public void GenerateWorld(int width, int seed)
    {
        GameController.Instance.SessionManager.NewWorldData(seed, width);
    }
}
