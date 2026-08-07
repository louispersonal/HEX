using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flyout : MonoBehaviour
{
    [SerializeField]  Panel[] _panels;
    
    public void OpenFlyOut()
    {
        _panels[0].BumpToFront();
        gameObject.SetActive(true);
    }

    public void CloseFlyOut()
    {
        gameObject.SetActive(false);
    }
}
