using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopView : MonoBehaviour
{
    public Pop Data;
    
    public void Initialize(Pop data)
    {
        Data = data;
        gameObject.transform.position = HexGridGeometry.AxialToScene(Data.Location);
    }

    public void Terminate()
    {
        
    }
}
