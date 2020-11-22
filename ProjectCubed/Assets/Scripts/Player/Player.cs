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

        //Initialise statemachine
        StateMachine.Initialise(IdleState);

        alreadyMoving = false;
    }

    private void Update()
    {
        CurrentVelocity = RB.velocity;
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

    public void OnTriggerStay2D(Collider2D collision)
    {
        if(InputHandler.InteractInput)
        {
            //If player has collided with the boundaries
            if (collision.gameObject.layer == 9)
            {
                rotationTriggerEntered = true;

                //call the rotation function in PlayerInteractState
                InteractState.RotatePlanet(collision);
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        
    }

    public IEnumerator Rotate(int verticalRotation, int horizontalRotation)
    {
        float angle = ShapeInfo.anglesBtwFaces[(int)ShapeInfo.chosenShape];
        Quaternion finalRotation = Quaternion.Euler(angle * horizontalRotation, angle * verticalRotation, 0) * Planet.transform.rotation;

        while (Planet.transform.rotation != finalRotation)
        {
            Planet.transform.rotation = Quaternion.Slerp(Planet.transform.rotation, finalRotation, Time.deltaTime * ShapeInfo.rotateSpeed);
            yield return 0;

        }

        ShapeInfo.planetRotationCompleted = true;
        InputHandler.InteractInput = false;
        rotationTriggerEntered = false;
        midTurning = false;
    }
}
