using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieWedge : MonoBehaviour
{
    [SerializeField] private Image _image;

    public void SetFill(float value)
    {
        _image.fillAmount = value;
    }

    public void SetAngle(float angle)
    {
        gameObject.transform.localEulerAngles = new Vector3(0, 0, angle);
    }

    public void SetColor(Color color)
    {
        _image.color = color;
    }
}
