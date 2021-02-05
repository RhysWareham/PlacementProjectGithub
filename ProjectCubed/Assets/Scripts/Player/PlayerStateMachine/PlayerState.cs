using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected Player player;
    protected PlayerStateMachine stateMachine;
    protected PlayerData playerData;

    protected float startTime;

    private string animBoolName;
    protected bool isAnimationFinished;
    protected bool isExitingState;

    public PlayerState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.playerData = playerData;
        this.animBoolName = animBoolName;
    }

    //virtual means this function can be overwritten by classes that inherit from this class
    //Called when player enters a specific state
    public virtual void Enter()
    {
        DoChecks();
        player.Anim.SetBool(animBoolName, true);

        //Make sure each animator has been updated.
        //for(int i = 0; i < player.Anim.Length; i++)
        //{
        //    player.Anim[i].SetBool(animBoolName, true);
        //}

        startTime = Time.time;
        isAnimationFinished = false;
        isExitingState = false;
    }

    //Called when player leaves a state
    public virtual void Exit()
    {
        //Make sure each animator has been updated, to keep all in sync
        //for (int i = 0; i < player.Anim.Length; i++)
        //{
        //    player.Anim[i].SetBool(animBoolName, false);
        //}

        player.Anim.SetBool(animBoolName, false);


        isExitingState = true;
    }

    //Called every frame
    public virtual void LogicUpdate()
    {

    }

    //Called every fixed update
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
