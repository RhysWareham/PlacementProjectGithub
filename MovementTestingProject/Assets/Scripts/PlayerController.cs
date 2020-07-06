using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //BottomHalf of Player variables    
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private Rigidbody2D bottomRB;

    private Vector2 movement;
    private Vector2 mousePos;

    //TopHalf of Player variables
    [SerializeField]
    private GameObject playerTop;
    [SerializeField]
    private Rigidbody2D topRB;

    //Camera variables
    [SerializeField]
    private Camera camera;



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
        bottomRB.MovePosition(bottomRB.position + movement * moveSpeed * Time.fixedDeltaTime);
        topRB.position = gameObject.transform.position;

        //Check what direction the legs are facing
        CheckLegDirection();

        Vector2 lookDirection = mousePos - bottomRB.position;

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
            //Sets the rb rotation to 0
            bottomRB.rotation = 0;

            //Sets the gameobject rotation Z axis to 0.
            gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, 0);

            //What is better?
        }
        else if (movement.x == 1 && movement.y == 1) //If Moving UP-RIGHT
        {
            gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, -45);
        }
        else if (movement.x == 1 && movement.y == 0) //If Moving RIGHT
        {
            gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, -90);
        }
        else if (movement.x == 1 && movement.y == -1) //If Moving DOWN-RIGHT
        {
            gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, -135);
        }
        else if (movement.x == 0 && movement.y == -1) //If Moving DOWN
        {
            gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, -180);
        }
        else if (movement.x == -1 && movement.y == -1) //If Moving DOWN_LEFT
        {
            gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, 135);
        }
        else if (movement.x == -1 && movement.y == 0) //If Moving LEFT
        {
            gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, 90);
        }
        else if (movement.x == -1 && movement.y == 1) //If Moving UP-LEFT
        {
            gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, 45);
        }
    }
}


//Rhys Wareham