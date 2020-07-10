using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Rigidbody2D playerRB;
    [SerializeField]
    private Sprite leftArrow;
    [SerializeField]
    private Sprite rightArrow;

    [SerializeField]
    private float moveSpeed;

    private Vector2 movement;
    private Vector2 mousePos;

    [SerializeField]
    private Camera camera;

    [SerializeField]
    private Rigidbody2D weaponPivot;
    [SerializeField]
    private GameObject weapon;
    [SerializeField]
    private Sprite triangle;
    [SerializeField]
    private Sprite circle;




    // Start is called before the first frame update
    void Start()
    {
        playerRB = gameObject.GetComponent<Rigidbody2D>();
        //movement.z = 0;
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
        //transform.position = transform.position + Vector3.ClampMagnitude(movement, 1.0f) * moveSpeed * Time.deltaTime;
        playerRB.MovePosition(playerRB.position + Vector2.ClampMagnitude(movement, 1.0f) * moveSpeed * Time.deltaTime);

        //Check what direction the legs are facing
        CheckDirection();

        //Get the vector which the player needs to face
        Vector2 lookDirection = mousePos - playerRB.position;

        

        //Returns the angle from the player axis to the mouse vector. Using the SOHCAHTOA formula
        //Atan2 takes in the y before the x
        //Atan2 returns value in radians, so must convert to degrees
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
        weaponPivot.rotation = angle;

        //Call the checkgunplacement function, and feed in the localEulerAngle z axis from weaponPivot
        CheckGunPlacement(weaponPivot.transform.localEulerAngles.z);
    }

    /// <summary>
    /// Function to check which direction the player is facing
    /// Once using animations, use: https://www.youtube.com/watch?v=whzomFgjT50&t=982s
    /// </summary>
    private void CheckDirection()
    {
        //If the player has stopped moving, don't change which way they are facing.
        if(movement.x == 0)
        {
            return;
        }
        //If moving right
        else if(movement.x > 0)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = rightArrow;
        }
        //If moving left
        else
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = leftArrow;
        }
    }

    /// <summary>
    /// Function to check where the weapon is located, and adjust its sorting order or direction it is facing
    /// </summary>
    /// <param name="weaponAngle"></param>
    private void CheckGunPlacement(float weaponAngle)
    {
        //0-180 is left, 180-360 is right
        //If the weapon is on the right of the player, change the sprite to use the right facing weapon
        if(weaponAngle >= 180)
        {
            weapon.GetComponent<SpriteRenderer>().sprite = triangle;
        }
        //If the weapon is on the left of the player, change sprite to use the left facing sprite
        else
        {
            weapon.GetComponent<SpriteRenderer>().sprite = circle;
        }

        //Check the rotation of the weapon, if above the player's head...
        if(weaponAngle > 310 || weaponAngle < 50)
        {
            //Put weapon on layer behind player character
            weapon.GetComponent<SpriteRenderer>().sortingOrder = 0;
        }
        //If the weapon is not above the head...
        else
        {
            //Put weapon on layer in front of player character
            weapon.GetComponent<SpriteRenderer>().sortingOrder = 2;
        }
    }


}
