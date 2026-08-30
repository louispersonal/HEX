using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverOverlayController : MonoBehaviour
{
    [SerializeField] SpriteRenderer[] _riverOverlays;

    public void InitializeOverlays(Hex hex)
    {
        DisableAll();
        if (!hex.WorldData.Rivers.TryGetObjectAt(hex.Coord, out River river)) return;
        
        foreach (AxialCardinalDirections direction in river.GetConnections(hex.Coord))
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
