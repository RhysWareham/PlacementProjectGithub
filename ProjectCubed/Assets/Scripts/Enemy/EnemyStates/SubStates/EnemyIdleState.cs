using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyMovementState
{
    public EnemyIdleState(Enemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData, string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("IdleState");
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(enemy.path == null)
        {
            return;
        }

        //if the currentWaypoint is less than the number of waypoints in the path
        if(enemy.currentWaypoint < enemy.path.vectorPath.Count && !isExitingState)
        {
            //Change to moveState
            stateMachine.ChangeState(enemy.MoveState);
        }

        
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();


    }
}
