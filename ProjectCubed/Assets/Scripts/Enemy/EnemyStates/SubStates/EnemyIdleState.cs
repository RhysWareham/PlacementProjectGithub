using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyState
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
        enemy.UpdatePath();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        //This will call the UpdateMovement function of the specific enemy, through EnemyType, 
        //and feed in this instance of enemy


        if(enemy.path == null)
        {
            enemy.UpdatePath();
        }

        if(enemy.currentWaypoint >= enemy.path.vectorPath.Count - enemy.nextWaypointDistance)
        {
            stateMachine.ChangeState(enemy.AttackState);
        }
        else if(enemy.currentWaypoint < enemy.path.vectorPath.Count && !isExitingState)
        {
            stateMachine.ChangeState(enemy.MoveState);
        }

        //Find way to check distance between player and enemy. If close enough, do attack, if far, start new path
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();


    }
}
