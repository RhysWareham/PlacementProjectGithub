using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour
{
    #region EnemyStateMachine
    public EnemyStateMachine StateMachine { get; private set; }
    public EnemyIdleState IdleState { get; private set; }
    public EnemyMoveState MoveState { get; private set; }
    #endregion

    public Animator Anim { get; private set; }

    public EnemyData enemyData;
    public EnemyType EnemyTypeScript { get; private set; }

    public int currentEnemyType = 0;

    public Player player { get; private set; }

    public AIPath aiPath { get; private set; }

    private int FacingRight = 1; // -1 means facing left // 1 means facing right

    public float health;


    private void Awake()
    {
        //Initialise
        StateMachine = new EnemyStateMachine();
        IdleState = new EnemyIdleState(this, StateMachine, enemyData, "idle");
        MoveState = new EnemyMoveState(this, StateMachine, enemyData, "move");
    }


    // Start is called before the first frame update
    void Start()
    {
        //Set the animator
        Anim = GetComponentInChildren<Animator>();

        //Get the specific enemy script by getting a component in child of EnemyType,
        //Which is what each specific enemy script will derive from
        EnemyTypeScript = transform.GetComponentInChildren<EnemyType>();

        //Set the aiPath
        aiPath = transform.GetComponent<AIPath>();

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

    /// <summary>
    /// Check if the enemy sprite should flip
    /// </summary>
    public void CheckToFlip()
    {
        if((aiPath.desiredVelocity.x >= 0.01f && FacingRight < 0)
            || aiPath.desiredVelocity.x < 0.01f && FacingRight > 0)
        {
            Flip();
        }
    }

    /// <summary>
    /// Rotate the sprite 180 degrees, to flip facing direction
    /// </summary>
    public void Flip()
    {
        FacingRight *= -1;
        transform.Rotate(0f, 180f, 0f);
    }

    //On bullet collision with enemy
    public void OnHit(Collision2D collision)
    {
        //Reduce health
        health -= collision.gameObject.GetComponent<Bullet>().damage;
        //Check if dead
        CheckDead();
    }

    /// <summary>
    /// Check if the enemy is dead
    /// </summary>
    public void CheckDead()
    {
        //If health is less than 0
        if (health <= 0)
        {
            //Destroy the enemy
            Destroy(gameObject);
        }
    }
}
