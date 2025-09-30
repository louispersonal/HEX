using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        BaseHexGrid.Instance.GenerateRectangularGrid(100, 50);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
