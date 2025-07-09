using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AxialCoordinate : MonoBehaviour
{
    [SerializeField] private Vector2Int _coords;

    public int Q { get { return _coords.x; } set { _coords.x = value; } }
    public int R { get { return _coords.y; } set { _coords.y = value; } }

    public AxialCoordinate(int q, int r)
    {
        _coords = new Vector2Int(q, r);
    }
}
