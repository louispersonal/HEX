using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    private ISelectable _currentSelection;

    private UiView _uiView => GameSceneController.Instance.UiView;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (TryGetHexSelection(out ISelectable selectionAttempt) && selectionAttempt != null &&
                selectionAttempt != _currentSelection)
            {
                Select(selectionAttempt);
            }
            else ClearSelection();
        }
    }

    private void Select(ISelectable selectable)
    {
        if (_currentSelection == selectable) return;

        _currentSelection?.OnDeselected();

        _currentSelection = selectable;
        _currentSelection.OnSelected();

        UpdateSelectionUI();
    }

    private void ClearSelection()
    {
        _currentSelection?.OnDeselected();
        _currentSelection = null;
    }
    
    private bool TryGetHexSelection(out ISelectable selection)
    {
        selection = null;
        HexGrid grid = GameController.Instance.SessionManager.WorldData.Grid;

        if (!HexGridGeometry.TryGetHexAtScenePoint( grid, HexGridView.MouseToPlane(Camera.main, 0f),
                out HexData target))
        {
            return false;
        }

        if (!GameSceneController.Instance.HexGridView.TryGetLiveHex(target.Coord, out HexView hexView))
        {
            return false;
        }

        selection = hexView;
        return true;
    }

    private void UpdateSelectionUI()
    {
        if (_currentSelection is HexView hexView)
        {
            Region currentRegion = GameController.Instance.SessionManager.WorldData.GetRegion(hexView.Data.ExtraData.RegionId);
            _uiView.OpenFlyOut(hexView.Data, currentRegion, null);
        }
        else _uiView.CloseFlyOut();
    }
}
