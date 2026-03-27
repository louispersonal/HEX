using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMenuController : SubMenu
{
    public void NewGame()
    {
        _mainMenuSceneController.SwitchMenu((int)MainMenuSceneSubMenus.NewGame);
    }

    public void LoadGame()
    {

    }

    public void ExitGame()
    {
        Debug.Log("Exiting Game");
        Application.Quit();
    }
}
