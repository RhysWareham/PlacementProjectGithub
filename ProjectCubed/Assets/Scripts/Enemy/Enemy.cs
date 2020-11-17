using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region EnemyStateMachine
    public EnemyStateMachine StateMachine { get; private set; }
    public EnemyIdleState IdleState { get; private set; }

    #endregion


    public EnemyData enemyData;
    
    public int currentEnemyType;

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
        //StateMachine.ChangeState()
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
