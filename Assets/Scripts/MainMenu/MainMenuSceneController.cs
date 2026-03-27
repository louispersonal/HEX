using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuSceneController : MonoBehaviour
{
    [SerializeField] private List<SubMenu> _subMenus;

    private SubMenu _currentMenu;
    // Start is called before the first frame update
    void Start()
    {
        if (_subMenus != null && _subMenus.Count > 0) SwitchMenu(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchMenu(int newMenuIndex)
    {
        if (_currentMenu != null) _currentMenu.ToggleMenuView(false);
        _subMenus[newMenuIndex].ToggleMenuView(true);
        _currentMenu = _subMenus[newMenuIndex];
    }
}

public enum MainMenuSceneSubMenus
{
    Title,
    NewGame
}