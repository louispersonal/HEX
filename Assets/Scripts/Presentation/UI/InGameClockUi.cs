using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameClockUi : MonoBehaviour
{
    private InGameClock _clock => GameSceneController.Instance.GameClock;
    
    [SerializeField] private Toggle _pauseToggle;
    [SerializeField] private Toggle[] _speedToggles;

    private void Start()
    {
        Refresh();
    }

    public void OnPauseClicked()
    {
        _clock.Pause();
        Refresh();
    }

    public void OnSpeedClicked(int speedIndex)
    {
        _clock.SetSpeed(speedIndex);
        Refresh();
    }
    
    private void Refresh()
    {
        _pauseToggle.SetIsOnWithoutNotify(_clock.IsPaused);

        for (int i = 0; i < _speedToggles.Length; i++)
        {
            bool isSelected = !_clock.IsPaused && _clock.CurrentSpeedIndex == i;
            _speedToggles[i].SetIsOnWithoutNotify(isSelected);
        }
    }
}
