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
        enemy.EnemyTypeScript.UpdateMovement(enemy);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}