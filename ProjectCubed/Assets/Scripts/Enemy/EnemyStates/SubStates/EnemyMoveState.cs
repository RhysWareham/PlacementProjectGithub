using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveState : EnemyMovementState
{
    public EnemyMoveState(Enemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData, string animBoolName) : base(enemy, stateMachine, enemyData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("MoveState");
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        
        //Check if enemy sprite should flip
        enemy.CheckToFlip();

        //If the desired velocity is 0
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if(enemy.path == null)
        {
            return;
        }

        //If the enemy's waypoint is less than path count, then add force to enemy
        if(enemy.currentWaypoint < enemy.path.vectorPath.Count)
        {
            //This will call the UpdateMovement function of the specific enemy, through EnemyType, 
            //and feed in this instance of enemy
            enemy.EnemyTypeScript.UpdateMovement(enemy);

            ////Get direction of which way the enemy should move
            //Vector2 direction = ((Vector2)enemy.path.vectorPath[enemy.currentWaypoint] - enemy.rb.position).normalized;
            //Vector2 force = direction * enemyData.enemyMaxSpeed[enemy.currentEnemyType] * Time.deltaTime;

            //enemy.rb.AddForce(force);

            ////Distance from next waypoint
            //float distance = Vector2.Distance(enemy.rb.position, enemy.path.vectorPath[enemy.currentWaypoint]);

            //if (distance < enemy.nextWaypointDistance)
            //{
            //    enemy.currentWaypoint++;
            //}
            
        }

        //if(enemy.rb.velocity.x == 0 && enemy.rb.velocity.y == 0 && !isExitingState)
        //{
        //    //Change state to idle
        //    stateMachine.ChangeState(enemy.IdleState);
        //}

        //If the current waypoint is more than or equal to the number of waypoints in path

        //if(enemy.currentWaypoint + enemy.nextWaypointDistance >= enemy.path.vectorPath.Count)
        //{
        //    stateMachine.ChangeState(enemy.AttackState);
        //}
    }
}
