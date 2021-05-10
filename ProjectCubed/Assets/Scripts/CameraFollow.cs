using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    private Transform zoomedOutPos;
    private float zoomOutDistance = 3.2f;
    private float zoomInDistance = 1.4f;

    public float smoothSpeed = 0.125f;
    public float zAxis = -10.0f;

    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
        zoomedOutPos.position = new Vector3(0, 0, -10);
    }

    private void LateUpdate()
    {
        if(target != null)
        {
            target = GameObject.Find("Player").GetComponent<Transform>();
            transform.position = new Vector3(target.position.x, target.position.y, zAxis);

        }

        if(GameManagement.zoomOut && cam.orthographicSize < zoomOutDistance)
        {
            cam.orthographicSize += (Time.deltaTime * 1.5f);
            
        }
        else if(GameManagement.zoomOut && cam.orthographicSize >= zoomOutDistance)
        {
            GameManagement.zoomOut = false;
        }

        if (GameManagement.zoomIn && cam.orthographicSize > zoomInDistance)
        {
            cam.orthographicSize -= (Time.deltaTime * 1.5f);

        }
        else if (GameManagement.zoomIn && cam.orthographicSize <= zoomInDistance)
        {
            GameManagement.zoomIn = false;
        }
    }

    public void ZoomOut()
    {
        //cam.orthographicSize = zoomOutDistance;
    }

    public void ZoomIn()
    {
        //cam.orthographicSize = zoomInDistance;
    }
}
