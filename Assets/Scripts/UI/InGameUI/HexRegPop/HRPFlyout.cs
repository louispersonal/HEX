using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HRPFlyout : Flyout, IUITickable
{
    public HexPanel HexPanel => _panels[0] as HexPanel;
    public RegionPanel RegionPanel => _panels[1] as RegionPanel;
    public PopPanel PopPanel => _panels[2] as PopPanel;

    public override void OpenFlyOut()
    {
        GameController.Instance.SessionManager.GameData.Ticker.Register(this);
        base.OpenFlyOut();
    }

    public override void CloseFlyOut()
    {
        GameController.Instance.SessionManager.GameData.Ticker.Remove(this);
        HexPanel.Terminate();
        PopPanel.Terminate();
        RegionPanel.Terminate();
        base.CloseFlyOut();
    }

    public void SetSelection(Hex hex, Region region, Pop pop)
    {
        HexPanel.Initialize(hex);
        
        if (region != null) RegionPanel.Initialize(region);
        
        if (pop != null) PopPanel.Initialize(pop);
    }

    public void UITick(TickInfo tickInfo)
    {
        if (HexPanel.Initialized) HexPanel.UpdatePanel();
        if (RegionPanel.Initialized) RegionPanel.UpdatePanel();
        if (PopPanel.Initialized) PopPanel.UpdatePanel();
    }
}
