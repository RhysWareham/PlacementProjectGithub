using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameObject playerWhole;
    private Rigidbody2D playerRB;

    //BottomHalf of Player variables    
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private GameObject playerBottom;


    private Vector3 movement;
    private Vector2 mousePos;

    //TopHalf of Player variables
    [SerializeField]
    private GameObject playerTop;
    [SerializeField]
    private Rigidbody2D topRB;

    //Camera variables
    [SerializeField]
    private Camera camera;

    private void Start()
    {
        //playerWhole = gameObject.GetComponent<GameObject>();
        playerRB = gameObject.GetComponent<Rigidbody2D>();
        movement.z = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //GetAxisRaw works with WASD, Arrow keys and controller axis
        //Returns a value between -1 and 1. -1 = Left, 1 = Right
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        

        //Get the mouse position in relation to world
        mousePos = camera.ScreenToWorldPoint(Input.mousePosition);

    }

    private void FixedUpdate()
    {
        //Movement
        transform.position = transform.position + Vector3.ClampMagnitude(movement, 1.0f) * moveSpeed * Time.deltaTime;

        //Check what direction the legs are facing
        CheckLegDirection();

        //Get the vector which the player needs to face
        Vector2 lookDirection = mousePos - playerRB.position;

        //Returns the angle from the player axis to the mouse vector. Using the SOHCAHTOA formula
        //Atan2 takes in the y before the x
        //Atan2 returns value in radians, so must convert to degrees
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
        topRB.rotation = angle;
    }

    /// <summary>
    /// Check the direction of the player's movement, and rotate the legs sprite to face the correct direction
    /// </summary>
    private void CheckLegDirection()
    {
        if(movement.x == 0 && movement.y == 1) //If Moving UP
        {
            //Sets the gameobject rotation Z axis to 0.
            playerBottom.transform.eulerAngles = new Vector3(playerBottom.transform.eulerAngles.x, playerBottom.transform.eulerAngles.y, 0);
        }
        else if (movement.x == 1 && movement.y == 1) //If Moving UP-RIGHT
        {
            playerBottom.transform.eulerAngles = new Vector3(playerBottom.transform.eulerAngles.x, playerBottom.transform.eulerAngles.y, -45);
        }
        else if (movement.x == 1 && movement.y == 0) //If Moving RIGHT
        {
            playerBottom.transform.eulerAngles = new Vector3(playerBottom.transform.eulerAngles.x, playerBottom.transform.eulerAngles.y, -90);
        }
        else if (movement.x == 1 && movement.y == -1) //If Moving DOWN-RIGHT
        {
            playerBottom.transform.eulerAngles = new Vector3(playerBottom.transform.eulerAngles.x, playerBottom.transform.eulerAngles.y, -135);
        }
        else if (movement.x == 0 && movement.y == -1) //If Moving DOWN
        {
            playerBottom.transform.eulerAngles = new Vector3(playerBottom.transform.eulerAngles.x, playerBottom.transform.eulerAngles.y, -180);
        }
        else if (movement.x == -1 && movement.y == -1) //If Moving DOWN_LEFT
        {
            playerBottom.transform.eulerAngles = new Vector3(playerBottom.transform.eulerAngles.x, playerBottom.transform.eulerAngles.y, 135);
        }
        else if (movement.x == -1 && movement.y == 0) //If Moving LEFT
        {
            playerBottom.transform.eulerAngles = new Vector3(playerBottom.transform.eulerAngles.x, playerBottom.transform.eulerAngles.y, 90);
        }
        else if (movement.x == -1 && movement.y == 1) //If Moving UP-LEFT
        {
            playerBottom.transform.eulerAngles = new Vector3(playerBottom.transform.eulerAngles.x, playerBottom.transform.eulerAngles.y, 45);
        }
    }
}


//Rhys Wareham