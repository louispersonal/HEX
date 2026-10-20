using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISavable<TSaveData>
{
    public TSaveData ToSaveData();
}
