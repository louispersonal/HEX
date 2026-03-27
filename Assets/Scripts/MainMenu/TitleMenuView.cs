using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMenuView : SubMenuView
{
    TitleMenuController TitleMenu { get { return _subMenu as TitleMenuController; } }

    public void NewGameButton()
    {
        TitleMenu.NewGame();
    }

    public void LoadGameButton()
    {
        TitleMenu.LoadGame();
    }

    public void ExitGameButton()
    {
        TitleMenu.ExitGame();
    }
}
