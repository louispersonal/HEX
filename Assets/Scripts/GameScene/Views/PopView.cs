using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopView : MonoBehaviour, ISelectable
{
    [SerializeField] private GameObject _outline;
    
    public Pop Data;
    
    public void Initialize(Pop data)
    {
        Data = data;
        gameObject.transform.position = HexGridGeometry.AxialToScene(Data.Location);
    }

    public void MoveTo(AxialCoordinate newPosition)
    {
        gameObject.transform.position = HexGridGeometry.AxialToScene(newPosition);
    }
    
    public void Terminate()
    {
        
    }

    public void OnSelected()
    {
        _outline.SetActive(true);
    }

    public void OnDeselected()
    {
        _outline.SetActive(false);
    }
}
