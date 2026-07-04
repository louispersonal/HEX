using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverOverlayController : MonoBehaviour
{
    [SerializeField] SpriteRenderer[] _riverOverlays;

    public void InitializeOverlays(HexData hexData)
    {
        DisableAll();
        if (!hexData.WorldData.Rivers.TryGetObjectAt(hexData.Coord, out River river)) return;
        
        foreach (AxialCardinalDirections direction in river.GetConnections(hexData.Coord))
        {
            _riverOverlays[(int)direction].gameObject.SetActive(true);
        }
    }

   private void DisableAll()
    {
        foreach(SpriteRenderer overlay in _riverOverlays)
        {
            overlay.gameObject.SetActive(false);
        }
    }
}
