using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathState : EnemyState
{
    public EnemyDeathState(Enemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData, string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        //Stop enemy moving
        enemy.rb.velocity = new Vector2(0, 0);
        enemy.animStartTime = Time.time;
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
        if(isAnimationFinished)
        {
            LevelManager.KillEnemy(enemy.gameObject);
        }
        //If the time is later than the animStartTime + currentClipLength
        //if (Time.time >= enemy.animStartTime + enemy.deathTime)
        //{
        //    //Call enemy destroy function
        //    LevelManager.KillEnemy(enemy.gameObject);
        //}


    }
}
