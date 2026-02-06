using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugView : MonoBehaviour
{
    float _timeInWindow = 0f;
    float _windowLength = 0.5f;
    int _framesInWindow = 0;

    [SerializeField] TextMeshProUGUI _fpsText;

    [SerializeField] TextMeshProUGUI _hexDebugText;

    public HexGrid HexGrid { get { return GameController.Instance.SessionManager.WorldData.Grid; } }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateFPSText();
        UpdateHexDebugText();
    }

    void UpdateFPSText()
    {
        if (_timeInWindow < _windowLength)
        {
            _timeInWindow += Time.deltaTime;
            _framesInWindow++;
        }
        else
        {
            float frameRate = _framesInWindow / _timeInWindow;
            string format = string.Format("{0:F2} FPS", frameRate);
            _fpsText.text = format;
            _timeInWindow = 0f;
            _framesInWindow = 0;
        }
    }

    void UpdateHexDebugText()
    {

    }
}
