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
    public EnemyAttackState AttackState { get; private set; }
    #endregion

    #region Animation Variables
    public Animator Anim { get; private set; }
    public string ClipName { get; set; }
    public AnimatorClipInfo[] CurrentClipInfo { get; set; }
    public float CurrentClipLength { get; set; }
    public float animStartTime { get; set; }
    public bool IsAttacking { get; set; }
    public float LastAttackTime { get; set; }

    public float idleTime;
    public float attackTime;
    public float jumpAttackTime;
    public float moveTime;
    public float deathTime;

    #endregion

    public EnemyData enemyData;
    public EnemyType EnemyTypeScript { get; private set; }

    public int currentEnemyType;

    public Rigidbody2D rb;

    public GameObject playerGO { get; private set; }
    public Transform target { get; private set; }

    public Path path { get; private set; }
    public int currentWaypoint { get; set; }
    public float nextWaypointDistance { get; private set; }
    public bool reachedEndOfPath { get; set; }
    public Seeker aiSeeker { get; private set; }

    private int FacingRight = 1; // -1 means facing left // 1 means facing right

    public float health;


    private void Awake()
    {
        //Initialise
        StateMachine = new EnemyStateMachine();
        IdleState = new EnemyIdleState(this, StateMachine, enemyData, "idle");
        MoveState = new EnemyMoveState(this, StateMachine, enemyData, "idle");
        AttackState = new EnemyAttackState(this, StateMachine, enemyData, "inAttack");
    }


    // Start is called before the first frame update
    void Start()
    {
        playerGO = GameObject.FindGameObjectWithTag("Player");
        target = playerGO.transform.Find("Player").transform;
        IsAttacking = false;

        //Set the animator
        Anim = GetComponentInChildren<Animator>();
        //Check if the Anim controller is there
        if(Anim == null)
        {
            Debug.Log("Could not find Animator!");
        }
        else
        {
            //Set the clip times for animations
            UpdateAnimClipTimes();
        }


        rb = GetComponent<Rigidbody2D>();
        reachedEndOfPath = false;
        currentWaypoint = 0;
        //Get the specific enemy script by getting a component in child of EnemyType,
        //Which is what each specific enemy script will derive from
        EnemyTypeScript = transform.GetComponentInChildren<EnemyType>();

        //Set the aiPath
        aiSeeker = transform.GetComponent<Seeker>();
        nextWaypointDistance = 3;

        InvokeRepeating("UpdatePath", 0f, 0.5f);

        //Initialise state machine in idle state
        StateMachine.Initialise(IdleState);
    }

    /// <summary>
    /// Function to start new path, if not currently looking for a path
    /// </summary>
    public void UpdatePath()
    {
        //If the seeker is not currently looking for a path
        if(aiSeeker.IsDone())
        {
            //Start new path
            aiSeeker.StartPath(rb.position, target.transform.position, OnPathComplete);
        }
    }


    void OnPathComplete(Path p)
    {
        //If no errors
        if(!p.error)
        {
            //Set current path to new path
            path = p;
            currentWaypoint = 0;
           
        }
    }

    // Update is called once per frame
    void Update()
    {
        StateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();

        //if (path == null)
        //{
        //    return;
        //}

        //if(currentWaypoint >= path.vectorPath.Count)
        //{
        //    reachedEndOfPath = true;
        //    return;
        //}
        //else
        //{
        //    reachedEndOfPath = false;
        //}

        ////Get direction of which way the enemy should move
        //Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        //Vector2 force = direction * enemyData.enemyMaxSpeed[currentEnemyType] * Time.deltaTime;

        //rb.AddForce(force);

        ////Distance from next waypoint
        //float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

       
        //if (distance < nextWaypointDistance)
        //{
        //    currentWaypoint++;
        //}
    }

    /// <summary>
    /// Check if the enemy sprite should flip
    /// </summary>
    public void CheckToFlip()
    {
        if((rb.velocity.x >= 0.01f && FacingRight < 0)
            || rb.velocity.x < 0.01f && FacingRight > 0)
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
        //health -= collision.gameObject.GetComponent<Bullet>().damage;
        health -= target.GetComponent<Player>().playerData.attackDamage;
        //Check if dead
        CheckDead();
    }


    //This is only for MELEE ATTACK!!!!!!!!!!!!!!!!!!!!!!!!!
    //Check if player inside attack radius
    public void OnTriggerStay2D(Collider2D collision)
    {
        
        //If gameobject has the tag of playerSprite
        if(collision.CompareTag("PlayerSprite") && !IsAttacking && 
            Time.time > LastAttackTime + enemyData.timeBtwAttack)
        {
            //Set IsAttacking to true
            IsAttacking = true;

            EnemyTypeScript.Attack(this);
            //Set state to attack
            StateMachine.ChangeState(AttackState);
        }
    }

    public void CheckAttackImpactRadius()
    {
        
        //If the player is within the enemy's attack radius
        if(Vector2.Distance(transform.position, target.transform.position) <= enemyData.enemyAttackImpactRadius[currentEnemyType])
        {
            //Choose damage value from range for current attack
            //int randDamage = Random.Range(enemyData.enemyAttackDamageRange[currentEnemyType, 0], enemyData.enemyAttackDamageRange[currentEnemyType, 1]);
            //Call takeDamage function for player, and feed in how much damage
            target.GetComponent<Player>().TakeDamage(enemyData.enemyAttackDamage[currentEnemyType]);
        }
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
            //Reduce amount of alive enemies
            GameManagement.enemiesLeftAliveOnFace--;
        }
    }

    /// <summary>
    /// Function to set the clip length of each animation
    /// </summary>
    public void UpdateAnimClipTimes()
    {
        //Create array of clips in animator controller
        AnimationClip[] clips = Anim.runtimeAnimatorController.animationClips;
        //Foreach loop, to go through every clip instance in the Clips array
        foreach(AnimationClip clip in clips)
        {
            //Switch case depending on name of clips
            switch(clip.name)
            {
                case "Idle":
                    idleTime = clip.length;
                    break;
                case "Move":
                    moveTime = clip.length;
                    break;
                case "Attack":
                    attackTime = clip.length;
                    break;
                case "JumpAttack":
                    jumpAttackTime = clip.length;
                    break;
                case "Death":
                    deathTime = clip.length;
                    break;
            }
        }
    }

}
