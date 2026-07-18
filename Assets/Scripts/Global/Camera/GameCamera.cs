using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameCamera : MonoBehaviour
{
    public float PanSpeed;

    public float ZoomSpeed;

    public float MaxZPosition;
    
    public float MinZPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = new Vector2(
            (Input.GetKey(KeyCode.D) ? 1 : 0) - (Input.GetKey(KeyCode.A) ? 1 : 0),
            (Input.GetKey(KeyCode.W) ? 1 : 0) - (Input.GetKey(KeyCode.S) ? 1 : 0)
        );
        
        if (input.sqrMagnitude > 0f)
        {
            Vector3 delta = new Vector3(input.x, input.y, 0f) * (PanSpeed * Time.deltaTime);
            transform.position += delta;
        }
        
        float scroll = Input.mouseScrollDelta.y;
        if (Mathf.Abs(scroll) > 0.0001f)
        {
            Zoom(scroll);
        }
    }

    void Zoom(float scrollDelta)
    {
        float zDelta = scrollDelta * (ZoomSpeed * Time.deltaTime);
        float newZ = Mathf.Clamp(transform.position.z + zDelta, MinZPosition, MaxZPosition);
        transform.position = new Vector3(transform.position.x, transform.position.y, newZ);
    }
}
