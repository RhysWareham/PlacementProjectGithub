using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public float smoothSpeed = 0.125f;
    public float zAxis = -10.0f;

    private void LateUpdate()
    {
        transform.position = new Vector3(target.position.x, target.position.y, zAxis);
    }
}
