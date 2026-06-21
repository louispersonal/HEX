using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverOverlayController : MonoBehaviour
{
    [SerializeField] SpriteRenderer[] _riverOverlays;

    public void InitializeOverlays(HexData hexData)
    {
        DisableAll();
        if (!hexData.WorldData.Rivers.ContainsAt(hexData.Coord)) return;

        List<AxialCardinalDirections> neighborRivers = hexData.WorldData.GetRiverNeighbors(hexData);
        foreach (AxialCardinalDirections direction in neighborRivers)
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
