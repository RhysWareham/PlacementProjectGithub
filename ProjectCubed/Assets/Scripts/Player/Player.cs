using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private SpriteRenderer legs;
    private SpriteRenderer body;
    private SpriteRenderer head;

    #endregion

    #region PlanetRotation

    public Transform Planet { get; private set; }
    public bool midTurning;
    public bool rotationTriggerEntered;

    #endregion

    #region PlayerVariables
    public Vector2 CurrentVelocity { get; private set; }

    public bool alreadyMoving;

    [SerializeField]
    private PlayerData playerData;


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
    }

    private void Start()
    {
        Anim = GetComponent<Animator>();
        InputHandler = GetComponent<PlayerInputHandler>();
        RB = GetComponentInParent<Rigidbody2D>();
        Planet = GameObject.Find("PlanetHolder").transform.Find("Planet");

        legs = transform.Find("Legs").GetComponent<SpriteRenderer>();
        body = legs.transform.Find("Body").GetComponent<SpriteRenderer>();
        head = legs.transform.Find("Head").GetComponent<SpriteRenderer>();

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
        FacingRight *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
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
        if (InputHandler.InteractInput && GameManagement.enemiesLeftAliveOnFace <= 0 && GameManagement.enemySpawningComplete)
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
        GameManagement.canStartSpawning = true;
        GameManagement.forwardFaceChecked = false;
        InputHandler.InteractInput = false;
        rotationTriggerEntered = false;
        midTurning = false;
    }


    public void TakeDamage(float damage)
    {
        if(!startBlinking)
        {
            //Subtract damage from the player's health
            playerData.currentHealth -= damage;
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
            legs.enabled = true;   // according to 
            body.enabled = true;
            head.enabled = true;
            
            return;
        }

        spriteBlinkingTimer += Time.deltaTime;
        if (spriteBlinkingTimer >= spriteBlinkingMiniDuration)
        {
            spriteBlinkingTimer = 0.0f;
            if (legs.enabled == true)
            {
                legs.enabled = false;  
                body.enabled = false;
                head.enabled = false;
            }
            else
            {
                legs.enabled = true;   
                body.enabled = true;
                head.enabled = true;
            }
        }

    }
}
