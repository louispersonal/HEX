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
    
    public Pawn PrimarySelection { get; private set; }

    public event Action OnPrimaryDeselected;
    public event Action<Pawn, Pawn> OnPrimarySelectionChanged;
    
    private void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;
        
        if (TryGetPawnSelection(out Pawn pawn))
        {
            if (PrimarySelection == null)
            {
                SetPrimarySelection(pawn);
                SelectPawnLocation(pawn);
            }
            else
            {
                ClearPrimarySelection();
            }
            return;
        }
        
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

    private void SelectPawnLocation(Pawn pawn)
    {
        if (pawn is not Pop pop)
            return;

        Hex hex = pop.CurrentHex;

        if (GameSceneController.Instance.HexGridView
            .TryGetLiveHex(hex.Coord, out HexView hexView))
        {
            SelectHex(hexView);
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
    
    public void SetPrimarySelection(Pawn pawn)
    {
        if (ReferenceEquals(PrimarySelection, pawn))
            return;

        Pawn previous = PrimarySelection;
        PrimarySelection = pawn;

        OnPrimarySelectionChanged?.Invoke(previous, PrimarySelection);
    }

    public void ClearPrimarySelection()
    {
        SetPrimarySelection(null);
        OnPrimaryDeselected?.Invoke();
    }
    
    private bool TryGetPawnSelection(out Pawn pawn)
    {
        pawn = null;

        Vector3 worldPoint =
            HexGridView.MouseToPlane(Camera.main, 0f);

        Collider2D hit = Physics2D.OverlapPoint(worldPoint);

        if (hit == null)
            return false;

        PopView popView = hit.GetComponentInParent<PopView>();

        if (popView == null || popView.Data == null)
            return false;

        pawn = popView.Data;
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