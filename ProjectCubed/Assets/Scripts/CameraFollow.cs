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
    private bool ifDoneOnce = false;
    private void Start()
    {
        cam = GetComponent<Camera>();
        zoomedOutPos = transform;
        zoomedOutPos.position = new Vector3(0, 0, -10);
    }

    private void Update()
    {
        if(!GameManagement.zoomOut && !GameManagement.zoomIn && target != null)
        {
            target = GameObject.Find("Player").GetComponent<Transform>();
            transform.position = new Vector3(target.position.x, target.position.y, zAxis);

        }

        if(GameManagement.zoomOut && cam.orthographicSize < zoomOutDistance)
        {
            //target = zoomedOutPos;
            //this.transform.localPosition = Vector2.MoveTowards(transform.position, zoomedOutPos.position, 2f);
            cam.orthographicSize += (Time.deltaTime * 1.5f);
            ifDoneOnce = false;
            
        }
        else if(GameManagement.zoomOut && cam.orthographicSize >= zoomOutDistance)
        {
            GameManagement.zoomOut = false;
            GameManagement.zoomIn = true;
            
            //If not done
            if (!ifDoneOnce)
            {
                //Reposition player and turn colliders and sprite back on
                target.GetComponent<Player>().RepositionPlayer();
                target.GetComponent<Player>().TurnPlayerOn();
                target = GameObject.Find("Player").GetComponent<Transform>();
                transform.position = new Vector3(target.position.x, target.position.y, zAxis);
                ifDoneOnce = true;
            }
        }

        if (GameManagement.zoomIn && cam.orthographicSize > zoomInDistance)
        {
            cam.orthographicSize -= (Time.deltaTime * 1.5f);
            //this.transform.localPosition = Vector2.MoveTowards(transform.position, target.position, 2f);

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
