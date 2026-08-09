using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HRPFlyout : Flyout
{
    public HexPanel HexPanel => _panels[0] as HexPanel;
    public RegionPanel RegionPanel => _panels[1] as RegionPanel;
    public PopPanel PopPanel => _panels[2] as PopPanel;
    
    public void SetSelection(HexData hex, Region region, Pop pop)
    {
        HexPanel.Populate(hex);
        
        if (region != null) RegionPanel.Populate(region);
        
        if (pop != null) PopPanel.Populate(pop);
    }
}
