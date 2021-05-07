using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementState : EnemyState
{
    public EnemyMovementState(Enemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData, string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (enemy.path == null)
        {
            return;
        }
        
        
        if (enemy.currentWaypoint >= enemy.path.vectorPath.Count)
        {
            //Enemy is at end of path, so change to IdleState
            enemy.reachedEndOfPath = true;
            //stateMachine.ChangeState(enemy.IdleState);
        }
        else
        {
            enemy.reachedEndOfPath = false;
        }

        
    }
}
