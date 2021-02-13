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

    //public SpriteRenderer legs;
    public SpriteRenderer body;
    public SpriteRenderer head;



    [SerializeField]
    private SpriteRenderer legs;
    [SerializeField]
    private SpriteRenderer[] angledBodyHeadArray;
    public int numOfBodyAngles;


    #endregion
    [SerializeField]
    public List<Image> heartList;

    #region PlanetRotation

    public Transform Planet { get; private set; }
    public bool midTurning;
    public bool rotationTriggerEntered;

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
        GameManagement.currentLegsBodyAngle = 0;
        GameManagement.previousLegsBodyAngle = 0;

        SetBodyAngleActive();

        //Ensure the correct angled sprites are enables
        angledBodyHeadArray[GameManagement.currentLegsBodyAngle].enabled = true;

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
        
        if(legs.flipX == false)
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
        if (InputHandler.InteractInput && GameManagement.enemiesLeftAliveOnFace <= 0 && GameManagement.enemySpawningComplete && GameManagement.PlanetCanRotate)
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
        float angle = ShapeInfo.anglesBtwFaces[(int)ShapeInfo.chosenShape];
        Quaternion finalRotation = Quaternion.Euler(angle * horizontalRotation, angle * verticalRotation, 0) * Planet.transform.rotation;

        while (Planet.transform.rotation != finalRotation)
        {
            Planet.transform.rotation = Quaternion.Slerp(Planet.transform.rotation, finalRotation, Time.deltaTime * ShapeInfo.rotateSpeed);
            yield return 0;

        }

        ShapeInfo.planetRotationCompleted = true;
        GameManagement.forwardFaceChecked = false;
        InputHandler.InteractInput = false;
        rotationTriggerEntered = false;
        midTurning = false;
    }


    public void TakeDamage(int damage)
    {
        if(!startBlinking)
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
        heartList[heartList.Count-1].gameObject.SetActive(false);
        //Remove the far right heart from the list.
        heartList.Remove(heartList[heartList.Count-1]);
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
        Destroy(transform.parent.gameObject);
    }

    public void StunPlayer()
    {
        //Do the flashy thing here and add a timer so player cannot be damaged for a couple seconds
        startBlinking = true;
    }

    private void SpriteBlinkingEffect()
    {
        spriteBlinkingTotalTimer += Time.deltaTime;
        if (spriteBlinkingTotalTimer >= spriteBlinkingTotalDuration)
        {
            startBlinking = false;
            spriteBlinkingTotalTimer = 0.0f;
            legs.enabled = true;
            legs.gameObject.SetActive(true);
            
            //for(int i = 0; i < angledBodyHeadArray.Length; i++)
            //{
            //    angledBodyHeadArray[i].enabled = true;
            //}

            return;
        }

        spriteBlinkingTimer += Time.deltaTime;
        if (spriteBlinkingTimer >= spriteBlinkingMiniDuration)
        {
            spriteBlinkingTimer = 0.0f;
            if (legs.enabled == true)
            {

                legs.enabled = false;
                legs.gameObject.SetActive(false);
                
            }
            else
            {

                legs.enabled = true;
                legs.gameObject.SetActive(true);
                
            }
        }

    }


    public void SetBodyAngleActive()
    {
        //Ensure the correct angled sprites are enables
        for(int i = 0; i < angledBodyHeadArray.Length; i++)
        {
            angledBodyHeadArray[i].enabled = false;
        }
        angledBodyHeadArray[GameManagement.currentLegsBodyAngle].enabled = true;


    }


    public float UnwrapAngle(float angle)
    {
        if (angle >= 0)
            return angle;

        angle = -angle % 360;

        return 360 - angle;
    }
}

//Rhys Wareham