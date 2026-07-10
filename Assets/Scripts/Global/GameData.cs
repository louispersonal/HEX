using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public Dictionary<AxialCoordinate, Pop> Pops = new Dictionary<AxialCoordinate, Pop>();
    
    public Dictionary<CultureID,  Culture> Cultures = new Dictionary<CultureID, Culture>();
    public Dictionary<ReligionID, Religion> Religions = new Dictionary<ReligionID, Religion>();
}
