﻿using System.Collections;
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

        //If the desired velocity of the AI pathfinding script is not 0 for x and y
        if ((enemy.aiPath.desiredVelocity.x != 0 || enemy.aiPath.desiredVelocity.y != 0) && !isExitingState)
        {
            //Change state to moving
            stateMachine.ChangeState(enemy.MoveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
