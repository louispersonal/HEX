using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flyout : MonoBehaviour
{
    [SerializeField] protected Panel[] _panels;
    
    public virtual void OpenFlyOut()
    {
        _panels[0].BumpToFront();
        gameObject.SetActive(true);
    }

    public virtual void CloseFlyOut()
    {
        gameObject.SetActive(false);
    }
}
