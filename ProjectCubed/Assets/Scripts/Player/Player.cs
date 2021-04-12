using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region PlayerStateMachine
    public PlayerStateMachine StateMachine { get; private set; }

    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerDodgeState DodgeState { get; private set; }
    public PlayerOnLandState OnLandState { get; private set; }
    public PlanetInteractState InteractState { get; private set; }

    #endregion

    #region PlayerComponents
    public Animator Anim { get; private set; }
    public PlayerInputHandler InputHandler { get; private set; }
    public Rigidbody2D RB { get; private set; }



    [SerializeField]
    private SpriteRenderer legs;
    [SerializeField]
    private SpriteRenderer[] angledBodyHeadArray;
    public int numOfBodyAngles;
    private bool playerVisible = true;

    [SerializeField]
    private SpriteRenderer weaponSprite;

    [SerializeField]
    private Transform playerHolder;

    #endregion

    #region UIStuff

    [SerializeField]
    public List<Image> heartList;


    #endregion

    #region PlanetRotation

    public Transform Planet { get; private set; }
    public bool midTurning;
    public bool rotationTriggerEntered;

    [SerializeField]
    private Transform xRightPlayerSpawn;
    [SerializeField]
    private Transform xLeftPlayerSpawn;
    [SerializeField]
    private Transform yTopPlayerSpawn;
    [SerializeField]
    private Transform yBottomPlayerSpawn;
    #endregion

    #region PlayerVariables
    public Vector2 CurrentVelocity { get; private set; }

    public bool alreadyMoving;

    [SerializeField]
    public PlayerData playerData;


    public int FacingRight = 1; //-1 - Left, 1 - Right

    private Vector2 workspace;

    #endregion

    public float spriteBlinkingTimer = 0.0f;
    public float spriteBlinkingMiniDuration = 0.1f;
    public float spriteBlinkingTotalTimer = 0.0f;
    public float spriteBlinkingTotalDuration = 1.0f;
    public bool startBlinking = false;

    private Transform currentPos;

    //Whenever the game starts, we will have a state machine created for the player
    private void Awake()
    {
        //Initialise
        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
        MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");
        DodgeState = new PlayerDodgeState(this, StateMachine, playerData, "inDodge");
        OnLandState = new PlayerOnLandState(this, StateMachine, playerData, "onLand");
        InteractState = new PlanetInteractState(this, StateMachine, playerData, "idle");

        GameManagement.playerAlive = true;
        //Set up Anim and bodySprite arrays
        //Anim = new Animator[angledBodyHeadArray.Length];
        //angledBodyHeadArray = new SpriteRenderer[angledLegsArray.Length];
    }

    private void Start()
    {
        //Changed this from GetComponent<Animator>();
        //Anim = transform.Find("Legs").GetComponent<Animator>();
        Anim = legs.gameObject.GetComponent<Animator>();
        numOfBodyAngles = 10;

        //Set the initial angle for the player to be facing right
        GameManagement.currentHeadBodyAngle = 0;
        GameManagement.previousHeadBodyAngle = 0;

        SetBodyAngleActive();

        //Ensure the correct angled sprites are enables
        angledBodyHeadArray[GameManagement.currentHeadBodyAngle].enabled = true;

        //Anim = transform.Find("Legs7").GetComponent<Animator>(); //Legs7 is Right direction
        InputHandler = GetComponent<PlayerInputHandler>();
        RB = GetComponentInParent<Rigidbody2D>();
        Planet = GameObject.Find("PlanetHolder").transform.Find("Planet");



        //SpriteRenderers for the blinking effect
        //legs = transform.Find("Legs").GetComponent<SpriteRenderer>();
        //body = legs.transform.Find("Body").GetComponent<SpriteRenderer>();
        //head = legs.transform.Find("Head").GetComponent<SpriteRenderer>();

        //Angle direction 7

        //legs7 = Anim.gameObject.GetComponent<SpriteRenderer>();
        //headBody7 = Anim.gameObject.transform.Find("headBody7").GetComponent<SpriteRenderer>();

        //Make if or for loop or something to idk what im doing now. ive drawn a blank

        //currentLegs = 1;//angledLegsArray[];

        //Set the current health to be starting health
        playerData.currentHealth = playerData.startingHealth;

        //Initialise statemachine
        StateMachine.Initialise(IdleState);

        alreadyMoving = false;
    }

    private void Update()
    {
        CurrentVelocity = RB.velocity;
        if (startBlinking)
        {
            SpriteBlinkingEffect();
        }
        StateMachine.CurrentState.LogicUpdate();
    }


    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }


    public void SetVelocityY(float velY)
    {
        workspace.Set(CurrentVelocity.x, velY);
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }

    public void SetVelocityXY(float velX, float velY)
    {
        workspace.Set(velX, velY);
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }

    public void SetVelocityWithDirection(float velocity, Vector2 direction)
    {
        workspace = direction * velocity;
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }

    /// <summary>
    /// Function to check if the player sprite is facing the correct direction
    /// </summary>
    /// <param name="xInput"></param>
    public void CheckToFlipLegs(float xInput)
    {
        //If moving right, and the player is not facing right, call the Flip function
        if ((xInput > 0 && FacingRight < 0) ||
                  xInput < 0 && FacingRight > 0)
        {
            Flip();
        }

    }

    public void Flip()
    {

        if (legs.flipX == false)
        {
            legs.flipX = true;

        }
        else
        {
            legs.flipX = false;
        }

        FacingRight *= -1;
        //transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    private void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger();

    private void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();


    /// <summary>
    /// Function to check if player is inside 
    /// </summary>
    /// <param name="collision"></param>
    public void OnTriggerStay2D(Collider2D collision)
    {
        //If player has clicked Interact, and no enemies are alive after enemy spawning is finished
        if ((InputHandler.InteractInput && GameManagement.enemiesLeftAliveOnFace <= 0 && GameManagement.enemySpawningComplete && GameManagement.PlanetCanRotate)
            || (GameManagement.UnlockRotation && InputHandler.InteractInput))
        {
            //If player has collided with the boundaries
            if (collision.gameObject.layer == 9)
            {
                rotationTriggerEntered = true;

                //call the rotation function in PlayerInteractState
                InteractState.RotatePlanet(collision);
            }
        }
        else
        {
            InputHandler.InteractInput = false;
        }
    }


    /// <summary>
    /// Function which slowly rotates the planet to the next chosen face
    /// </summary>
    /// <param name="verticalRotation"></param>
    /// <param name="horizontalRotation"></param>
    /// <returns></returns>
    public IEnumerator RotatePlanet(int verticalRotation, int horizontalRotation)
    {

        currentPos = transform;

        //Set player transparency to 0, to make player and weapon invisible
        ChangePlayerTransparency(0f);
        //Turn colliders off
        SetPlayerCollidersActive(false);


        float angle = ShapeInfo.anglesBtwFaces[(int)ShapeInfo.chosenShape];
        Quaternion finalRotation = Quaternion.Euler(angle * horizontalRotation, angle * verticalRotation, 0) * Planet.transform.rotation;

        while (Planet.transform.rotation != finalRotation)
        {
            Planet.transform.rotation = Quaternion.Slerp(Planet.transform.rotation, finalRotation, Time.deltaTime * ShapeInfo.rotateSpeed);
            yield return 0;

        }

        //Create a transform point to spawn player instead!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        float sideFaceSpawnX = 2f;

        //If right
        if (verticalRotation == 1)
        {
            
            xLeftPlayerSpawn.position = new Vector3(xLeftPlayerSpawn.position.x, currentPos.position.y, currentPos.position.z);
            currentPos.position = xLeftPlayerSpawn.position;
        }
        else if (verticalRotation == -1)
        {
            xRightPlayerSpawn.position = new Vector3(xRightPlayerSpawn.position.x, currentPos.position.y, currentPos.position.z);
            currentPos.position = xRightPlayerSpawn.position;
        }
        else if (horizontalRotation == 1)
        {
            yTopPlayerSpawn.position = new Vector3(currentPos.position.x, yTopPlayerSpawn.position.y, currentPos.position.z);
            currentPos.position = yTopPlayerSpawn.position;
        }
        else if (horizontalRotation == -1)
        {
            yBottomPlayerSpawn.position = new Vector3(currentPos.position.x, yBottomPlayerSpawn.position.y, currentPos.position.z);
            currentPos.position = yBottomPlayerSpawn.position;
        }

        ///Fix player position, Need to put this in separate function
        //If going right or left
        //if (verticalRotation == 1 || verticalRotation == -1)
        //{
        //    //Set x axis multiplied by -1
        //    currentPos.position = new Vector3(sideFaceSpawnX * (verticalRotation*-1), currentPos.position.y, currentPos.position.z);
        //}
        ////If going up or down
        //if (horizontalRotation == 1 || horizontalRotation == -1)
        //{
        //    //Set y axis multiplied by -1
        //    currentPos.position = new Vector3(currentPos.position.x, currentPos.position.y * -1, currentPos.position.z);

        //}

        //Set new position
        this.transform.position = currentPos.position;

        //Set player transparency back to 1
        ChangePlayerTransparency(1f);
        //Turn player collider back on
        SetPlayerCollidersActive(true);

        ShapeInfo.planetRotationCompleted = true;
        GameManagement.forwardFaceChecked = false;
        InputHandler.InteractInput = false;
        rotationTriggerEntered = false;
        midTurning = false;
    }


    public void TakeDamage(int damage)
    {
        if (!startBlinking)
        {
            //Subtract damage from the player's health
            playerData.currentHealth -= damage;
            UpdateHearts();

            //Check if player is dead
            if (CheckDead())
            {
                //Player is dead
                KillPlayer();
            }
            else
            {
                StunPlayer();
            }
        }

    }

    public void UpdateHearts()
    {
        //Turn off the far right heart
        heartList[heartList.Count - 1].gameObject.SetActive(false);
        //Remove the far right heart from the list.
        heartList.Remove(heartList[heartList.Count - 1]);
    }

    public void AddHeart()
    {
        //Instantiate new heart, make sure its a child of the heartContainer.
        //Make sure it is 120 points to the right of the final heart.
        //Add it to the list
    }

    public bool CheckDead()
    {
        if (playerData.currentHealth <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void KillPlayer()
    {
        GameManagement.playerAlive = false;
        Destroy(transform.parent.gameObject);
    }

    public void StunPlayer()
    {
        //Do the flashy thing here and add a timer so player cannot be damaged for a couple seconds
        startBlinking = true;
    }

    private void SpriteBlinkingEffect()
    {
        //Increase the spriteBlinkingTotal timer
        spriteBlinkingTotalTimer += Time.deltaTime;

        //If the timer has reached its total duration...
        if (spriteBlinkingTotalTimer >= spriteBlinkingTotalDuration)
        {
            //Set startBlinking to false
            startBlinking = false;

            //Set the timer back to 0
            spriteBlinkingTotalTimer = 0.0f;

            //Create a new temp variable to store the color info
            var tempColour = angledBodyHeadArray[0].color;
            //Set the alpha to 1, to be opague
            tempColour.a = 1f;

            //Set legs colour to temp colour
            legs.color = tempColour;

            //For loop going through all bodyHead sprites in array
            for (int i = 0; i < angledBodyHeadArray.Length; i++)
            {
                //Set current instance of bodyHead to tempColor.
                angledBodyHeadArray[i].color = tempColour;
            }

            //Set playerVisible to true
            playerVisible = true;

            return;
        }

        //Increase the spriteBlinking timer
        spriteBlinkingTimer += Time.deltaTime;
        //If the blinking timer has reached the interval times between turning on and off...
        if (spriteBlinkingTimer >= spriteBlinkingMiniDuration)
        {
            //Restart the timer
            spriteBlinkingTimer = 0.0f;

            //Create a new temp variable to store the color info
            var tempColour = angledBodyHeadArray[0].color;

            //If player is visible
            if (playerVisible)
            {
                //Set the alpha to 0, to be transparrent
                tempColour.a = 0f;

                //Set legs colour to temp colour
                legs.color = tempColour;

                //For loop going through all bodyHead sprites in array
                for (int i = 0; i < angledBodyHeadArray.Length; i++)
                {
                    //Set current instance of bodyHead to tempColor.
                    angledBodyHeadArray[i].color = tempColour;
                }

                //Set player visible to false
                playerVisible = false;
            }
            else
            {
                //Set the alpha to 1, to be opague
                tempColour.a = 1f;

                //Set legs colour to temp colour
                legs.color = tempColour;

                //For loop going through all bodyHead sprites in array
                for (int i = 0; i < angledBodyHeadArray.Length; i++)
                {
                    //Set current instance of bodyHead to tempColor.
                    angledBodyHeadArray[i].color = tempColour;
                }

                //Set player visible to true
                playerVisible = true;
            }
        }

    }


    public void SetBodyAngleActive()
    {
        //Disable all body sprites
        for (int i = 0; i < angledBodyHeadArray.Length; i++)
        {
            angledBodyHeadArray[i].enabled = false;
        }

        //Enable current body sprite
        angledBodyHeadArray[GameManagement.currentHeadBodyAngle].enabled = true;


    }


    public float UnwrapAngle(float angle)
    {
        if (angle >= 0)
            return angle;

        angle = -angle % 360;

        return 360 - angle;
    }

    /// <summary>
    /// Function to set the transparency of the player sprite
    /// </summary>
    /// <param name="alphaValue"></param>
    public void ChangePlayerTransparency(float alphaValue)
    {
        //Create a new temp variable to store the color info
        var tempColour = angledBodyHeadArray[0].color;
        //Set the alpha to alphaValue
        tempColour.a = alphaValue;

        //Set legs and weapon colour to temp colour
        legs.color = tempColour;
        weaponSprite.color = tempColour;

        //For loop going through all bodyHead sprites in array
        for (int i = 0; i < angledBodyHeadArray.Length; i++)
        {
            //Set current instance of bodyHead to tempColor.
            angledBodyHeadArray[i].color = tempColour;
        }
    }


    /// <summary>
    /// Function to set the player colliders active
    /// </summary>
    /// <param name="trueFalse"></param>
    public void SetPlayerCollidersActive(bool trueFalse)
    {
        this.GetComponent<BoxCollider2D>().enabled = trueFalse;
    }
}