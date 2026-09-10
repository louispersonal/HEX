using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPopController : MonoBehaviour
{
    [SerializeField] private SelectionManager _selectionManager;
    
    private bool _selectingMigrationDestination = false;
    
    public void OpenMigrationView()
    {
        _selectingMigrationDestination = true;
    }

    private void Update()
    {
        if (!_selectingMigrationDestination) return;
        
        if (Input.GetMouseButtonDown(0))
        {
            HexGrid grid = GameController.Instance.SessionManager.WorldData.Grid;
            if (!HexGridGeometry.TryGetHexAtScenePoint(grid, HexGridView.MouseToPlane(Camera.main, 0f),
                    out Hex target))
            {
                return;
            }
            _selectionManager.LocationSelection.SelectedPop.AddJob(CreateMoveJob(target.Coord));
            _selectingMigrationDestination = false;
        }
    }

    private MoveJob CreateMoveJob(AxialCoordinate destination)
    {
        return new MoveJob(_selectionManager.LocationSelection.SelectedPop, destination);
    }
}
