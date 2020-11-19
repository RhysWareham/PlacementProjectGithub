using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region EnemyStateMachine
    public EnemyStateMachine StateMachine { get; private set; }
    public EnemyIdleState IdleState { get; private set; }

    #endregion

    public Animator Anim { get; private set; }

    public EnemyData enemyData;
    public EnemyType EnemyTypeScript { get; private set; }

    public int currentEnemyType;

    public Player player { get; private set; }

    //How do i recieve the individual enemy script in this gameobject? As this will need to get the script, 
    //no matter the kind. So i couldn't get SlimeScript if the variable is stated as EnemyTypeScript or something

    //ENUM for Enemy Types?


    private void Awake()
    {
        //Initialise
        StateMachine = new EnemyStateMachine();
        IdleState = new EnemyIdleState(this, StateMachine, enemyData, "idle");

        //MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");
        //Instantiate(enemyType[currentEnemyType]);
    }


    // Start is called before the first frame update
    void Start()
    {
        Anim = GetComponentInChildren<Animator>();

        //Get the specific enemy script by getting a component in child of EnemyType,
        //Which is what each specific enemy script will derive from
        EnemyTypeScript = transform.GetComponentInChildren<EnemyType>();

        //Initialise state machine in idle state
        StateMachine.Initialise(IdleState);
    }

    // Update is called once per frame
    void Update()
    {
        StateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }
}
