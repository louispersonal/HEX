using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPopController : MonoBehaviour
{
    [SerializeField] private SelectionManager _selectionManager;
    
    private bool _selectingMigrationDestination = false;

    private Pop _movingPop;
    
    public void OpenMigrationView()
    {
        if (_selectionManager.PrimarySelection is not Pop pop)
            return;

        _movingPop = pop;
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
            _selectingMigrationDestination = false;
            CreateMoveJob(target.Coord);
        }
    }

    private MoveJob CreateMoveJob(AxialCoordinate destination)
    {
        return new MoveJob(_movingPop, destination);
    }
}
