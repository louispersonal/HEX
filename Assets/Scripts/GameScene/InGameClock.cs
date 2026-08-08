using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameClock : MonoBehaviour
{
    [SerializeField] private float[] GameSpeedTimeIntervals;

    private int _currentSpeedIndex = 0;
    
    public int CurrentSpeedIndex => _currentSpeedIndex;

    private bool _paused = true;
    
    public bool IsPaused => _paused;

    private float _currentInterval = 0f;
    
    void Start()
    {
        
    }
    
    private void Update()
    {
        if (_paused) return;

        _currentInterval += Time.deltaTime;

        // account for possibility that more than one tick progressed in a frame
        while (_currentInterval >= GameSpeedTimeIntervals[_currentSpeedIndex])
        {
            _currentInterval -= GameSpeedTimeIntervals[_currentSpeedIndex];
            GameController.Instance.SessionManager.GameData.Ticker.ProgressTick();
            Debug.Log("TICK!");
        }
    }
    
    public void Pause()
    {
        _paused = true;
    }

    public void SetSpeed(int speedIndex)
    {
        _currentSpeedIndex = speedIndex;
        _paused = false;
    }
}
