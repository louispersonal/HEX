using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        BaseHexGrid.Instance.GenerateRectangularGrid(WorldGenController.Instance.Columns, WorldGenController.Instance.Rows);
        WorldGenController.Instance.SetElevations(BaseHexGrid.Instance);
        WorldGenController.Instance.SetTemperatures(BaseHexGrid.Instance);
        WorldGenController.Instance.SetPrecipitationVegetation(BaseHexGrid.Instance);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
