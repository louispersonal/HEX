using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexPanel : Panel
{
    private Hex _hex;
    public void Initialize(Hex hex)
    {
        _hex = hex;
        Initialized = true;
        UpdatePanel();
    }

    public void UpdatePanel()
    {
        
    }

    public void Terminate()
    {
        Initialized = false;
    }
}
