using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Linq;

public class SelectionManager : MonoBehaviour
{
    public LocationSelection LocationSelection { get; private set; }

    public event Action OnLocationDeselected;
    public event Action<LocationSelection> OnLocationSelectionChanged;
    
    private HexView _selectedHexView;
    
    private void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;
        
        if (TryGetHexSelection(out HexView hexView))
        {
            if (LocationSelection?.SelectedHex != hexView.Data)
            {
                SelectHex(hexView);
            }
            else
            {
                ClearLocationSelection();
            }
        }
        else
        {
            ClearLocationSelection();
        }
    }

    private void SelectHex(HexView hexView)
    {
        _selectedHexView?.SetDeselected();
        
        Hex hex = hexView.Data;

        Region region = GameController.Instance.SessionManager.WorldData
            .GetRegion(hex.ExtraData.RegionId);

        LocationSelection = new LocationSelection(hex, region);
        
        _selectedHexView = hexView;
        _selectedHexView.SetSelected();
        
        OnLocationSelectionChanged?.Invoke(LocationSelection);
    }
    
    private void ClearLocationSelection()
    {
        if (LocationSelection == null)
            return;
        
        _selectedHexView?.SetDeselected();
        _selectedHexView = null;
        
        LocationSelection = null;
        OnLocationDeselected?.Invoke();
    }
    
    private bool TryGetHexSelection(out HexView hexView)
    {
        hexView = null;
        HexGrid grid = GameController.Instance.SessionManager.WorldData.Grid;

        if (!HexGridGeometry.TryGetHexAtScenePoint( grid, HexGridView.MouseToPlane(Camera.main, 0f),
                out Hex target))
        {
            return false;
        }

        if (!GameSceneController.Instance.HexGridView.TryGetLiveHex(target.Coord, out hexView))
        {
            return false;
        }
        return true;
    }
}

public sealed class LocationSelection
{
    public Hex SelectedHex { get; }
    public Region SelectedRegion { get; }

    public LocationSelection(Hex hex, Region region)
    {
        SelectedHex = hex;
        SelectedRegion = region;
    }
}