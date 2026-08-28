using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardSprite : MonoBehaviour
{
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void LateUpdate()
    {
        Vector3 direction = transform.position - cam.transform.position;
        direction.x = 0f;

        transform.rotation = Quaternion.LookRotation(direction);
    }
}
