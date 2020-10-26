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
    //public PlayerShootState ShootState { get; private set; }
    public PlayerInteractState InteractState { get; private set; }

    #endregion

    #region PlayerComponents
    public Animator Anim { get; private set; }
    public PlayerInputHandler InputHandler { get; private set; }
    public Rigidbody2D RB { get; private set; }

    #endregion

    #region PlayerVariables
    public Vector2 CurrentVelocity { get; private set; }

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
        //ShootState = new PlayerShootState(this, StateMachine, playerData, "shoot");
    }

    private void Start()
    {
        Anim = GetComponent<Animator>();
        InputHandler = GetComponent<PlayerInputHandler>();
        RB = GetComponent<Rigidbody2D>();

        //Initialise statemachine
        StateMachine.Initialise(IdleState);
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

    private void SetWeaponRotation()
    {

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



}
