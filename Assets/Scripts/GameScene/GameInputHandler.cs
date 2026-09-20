using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInputHandler : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
        {
            GameController.Instance.SessionManager.SaveGameData();
        }
    }
}
