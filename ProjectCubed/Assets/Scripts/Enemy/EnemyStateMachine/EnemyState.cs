using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    protected Enemy enemy;
    protected EnemyStateMachine stateMachine;
    protected EnemyData enemyData;

    protected float startTime;

    private string animBoolName;
    protected bool isAnimationFinished;
    protected bool isExitingState;

    public EnemyState(Enemy enemy, EnemyStateMachine stateMachine, EnemyData enemyData, string animBoolName)
    {
        this.enemy = enemy;
        this.stateMachine = stateMachine;
        this.enemyData = enemyData;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        DoChecks();
        //Set the animation bool to true, to start the animation
        enemy.Anim.SetBool(animBoolName, true);
        //Get the start time of the animation
        startTime = Time.time;
        //Animation is not finished
        isAnimationFinished = false;
        //Not exiting state
        isExitingState = false;
    }

    public virtual void Exit()
    {
        //Set animation bool to false
        enemy.Anim.SetBool(animBoolName, false);
        isExitingState = true;
    }

    public virtual void LogicUpdate()
    {

    }

    public virtual void PhysicsUpdate()
    {
        DoChecks();
    }

    public virtual void DoChecks()
    {

    }

    public virtual void AnimationTrigger() { }
    public virtual void AnimationFinishTrigger() => isAnimationFinished = true;
}
