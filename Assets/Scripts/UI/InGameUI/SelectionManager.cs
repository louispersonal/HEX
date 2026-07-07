using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    private ISelectable _currentSelection;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            
        }
    }

    private bool TryGetHexSelection(out HexView hexView)
    {
        HexGrid grid = GameController.Instance.SessionManager.WorldData.Grid;
        HexData target = HexGridGeometry.GetHexAtScenePoint(grid, HexGridView.MouseToPlane(Camera.main, 0f));
        return GameSceneController.Instance.HexGridView.TryGetLiveHex(target.Coord, out hexView);
    }
}
