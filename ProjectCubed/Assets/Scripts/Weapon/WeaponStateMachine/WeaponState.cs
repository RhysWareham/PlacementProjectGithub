using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponState
{
    protected Weapon weapon;
    protected WeaponStateMachine stateMachine;
    protected WeaponData weaponData;

    protected float animStartTime;

    private string animBoolName;
    protected bool isAnimationFinished;
    protected bool isExitingState;
    protected bool notJammed;

    public WeaponState(Weapon weapon, WeaponStateMachine stateMachine, WeaponData weaponData, string animBoolName)
    {
        this.weapon = weapon;
        this.stateMachine = stateMachine;
        this.weaponData = weaponData;
        this.animBoolName = animBoolName;
    }

    //virtual means this function can be overwritten by classes that inherit from this class
    //Called when player enters a specific state
    public virtual void Enter()
    {
        DoChecks();
        weapon.Anim.SetBool(animBoolName, true);
        animStartTime = Time.time;
        //Debug.Log(animBoolName);
        isAnimationFinished = false;
        isExitingState = false;
        notJammed = true;
    }

    //Called when player leaves a state
    public virtual void Exit()
    {
        weapon.Anim.SetBool(animBoolName, false);
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
