using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flyout : MonoBehaviour, IUITickable
{
    [SerializeField] protected Panel[] _panels;
    [SerializeField] private GameObject _content;
    
    public bool IsOpen { get; private set; }
    
    public virtual void OpenFlyOut()
    {
        if (IsOpen) return;
        
        GameController.Instance.SessionManager.GameData.Ticker.Register(this);
        _panels[0].BumpToFront();
        _content.SetActive(true);
        IsOpen = true;
    }

    public virtual void CloseFlyOut()
    {
        if (!IsOpen) return;
        
        GameController.Instance.SessionManager.GameData.Ticker.Remove(this);
        _content.SetActive(false);
        IsOpen = false;
    }
    
    public void UITick(TickInfo tickInfo)
    {
        if (IsOpen) UpdateFlyout();
    }

    protected virtual void UpdateFlyout()
    {
        
    }
    
    protected virtual void OnDestroy()
    {
        if (IsOpen)
        {
            GameController.Instance.SessionManager.GameData.Ticker.Remove(this);
        }
    }
}
