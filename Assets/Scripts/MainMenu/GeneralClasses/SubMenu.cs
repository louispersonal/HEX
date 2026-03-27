using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubMenu : MonoBehaviour
{
    [SerializeField] protected MainMenuSceneController _mainMenuSceneController;

    [SerializeField] protected SubMenuView _subMenuView;

    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        
    }

    public virtual void ToggleMenuView(bool enable)
    {
        _subMenuView.gameObject.SetActive(enable);
    }
}
