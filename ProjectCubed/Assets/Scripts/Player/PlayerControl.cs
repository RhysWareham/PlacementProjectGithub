using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField]
    private Camera camera;
    
    //Player variables
    private Rigidbody2D playerRB;
    private bool playerRightFacing = true;

    [SerializeField]
    private float moveSpeed;

    private Vector2 movement;
    private Vector2 mousePos;


    //Weapon variables
    [SerializeField]
    private Rigidbody2D weaponPivot;
    [SerializeField]
    private GameObject weapon;
    [SerializeField]
    private GameObject firepoint;
    private bool weaponRightFacing = true;


    [SerializeField]
    private GameObject shape;
    [SerializeField]
    private float rotateSpeed;
    private bool midTurning = false;




    // Start is called before the first frame update
    void Start()
    {
        playerRB = gameObject.GetComponent<Rigidbody2D>();
        //movement.z = 0;
    }

    // Update is called once per frame
    void Update()
    {
        ////GetAxisRaw works with WASD, Arrow keys and controller axis
        ////Returns a value between -1 and 1. -1 = Left, 1 = Right
        //movement.x = Input.GetAxisRaw("Horizontal");
        //movement.y = Input.GetAxisRaw("Vertical");


        ////Get the mouse position in relation to world
        //mousePos = camera.ScreenToWorldPoint(Input.mousePosition);

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
        //If moving right, and the player is not facing right, flip the player sprite on the X axis
        else if (movement.x > 0 && !playerRightFacing)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
            playerRightFacing = true;
        }
        //If moving left, and the player is facing right, flip the player sprite on the X axis
        else if(movement.x < 0 && playerRightFacing)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
            playerRightFacing = false;
        }
    }

    /// <summary>
    /// Function to check where the weapon is located, and adjust its sorting order or direction it is facing
    /// </summary>
    /// <param name="weaponAngle"></param>
    private void CheckGunPlacement(float weaponAngle)
    {
        //0-180 is left, 180-360 is right
        //If the weapon is on the right of the player, and the gun is not facing right, flip the gun sprite
        //Adjust the firepoint to be inline with the gun's barrel
        if(weaponAngle >= 180 && !weaponRightFacing)
        {
            weapon.GetComponent<SpriteRenderer>().flipY = false;
            AdjustFirepoint();
            weaponRightFacing = true;
        }
        //If the weapon is on the left of the player, and the gun is facing right, flip the gun sprite
        else if (weaponAngle < 180 && weaponRightFacing)
        {
            weapon.GetComponent<SpriteRenderer>().flipY = true;
            AdjustFirepoint();
            weaponRightFacing = false;
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

    /// <summary>
    /// Function to adjust the position of the firepoint when the weapon has been flipped
    /// </summary>
    private void AdjustFirepoint()
    {
        //Must adjust the localPosition, not global position
        firepoint.transform.localPosition = new Vector3(firepoint.transform.localPosition.x, 
                                                    firepoint.transform.localPosition.y * -1, 
                                                    firepoint.transform.localPosition.z);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //If player has collided with the boundaries
        if (collision.gameObject.layer == 9)
        {
            //Create a string variable to store direction
            if (Input.GetKey(KeyCode.E) && midTurning == false)
            {
                midTurning = true;
                int leftRightDirection = 0;
                int upDownDirection = 0;
                //Switch statement to only go through the correct type of shape
                switch(ShapeInfo.chosenShape)
                {
                    //If the shape is a CUBE...
                    case ShapeInfo.ShapeType.CUBE:
                        {
                            //Check if the collision is a left/right wall
                            if(collision.gameObject.GetComponent("LeftRightWall") != null)
                            {
                                if(collision.gameObject.transform.position.x < transform.position.x)
                                {
                                    leftRightDirection = -1;
                                }
                                else
                                {
                                    leftRightDirection = 1;
                                }
                            }
                            else if(collision.gameObject.GetComponent("UpDownWall") != null)
                            {
                                if (collision.gameObject.transform.position.y < transform.position.y)
                                {
                                    upDownDirection = 1;
                                }
                                else
                                {
                                    upDownDirection = -1;
                                }
                            }

                            //RotateShape(leftRightDirection, upDownDirection);
                            StartCoroutine(Rotate(leftRightDirection, upDownDirection));

                        break;
                        }
                }
                Debug.Log("turning");
                GameManagement.faceCorrectionComplete = false;
                GameManagement.shapeTurnPhase = true;
                GameManagement.shapeTurning = true;
            }
        }
    }

    //IEnumerator which rotates the cube to the correct face
    IEnumerator Rotate(int verticalRotation, int horizontalRotation)
    {
        float angle = ShapeInfo.anglesBtwFaces[(int)ShapeInfo.chosenShape];
        Quaternion finalRotation = Quaternion.Euler(angle * horizontalRotation, angle * verticalRotation, 0) * shape.transform.rotation;

        while(shape.transform.rotation != finalRotation)
        {
            shape.transform.rotation = Quaternion.Slerp(shape.transform.rotation, finalRotation, Time.deltaTime * rotateSpeed);
            yield return 0;

        }

        
        midTurning = false;
    }
}
