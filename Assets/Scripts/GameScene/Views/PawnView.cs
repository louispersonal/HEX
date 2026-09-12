using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnView : MonoBehaviour, ISelectable
{
    [SerializeField] protected GameObject _outline;
    
    public Pawn Data { get; private set; }
    
    protected void InitializePawn(Pawn data)
    {
        Data = data;
    }
    
    public virtual void OnSelected()
    {
        _outline.SetActive(true);
    }

    public virtual void OnDeselected()
    {
        _outline.SetActive(false);
    }
}
