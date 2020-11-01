using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponJamState : WeaponAimState
{
    private float jammedTimer;

    public WeaponJamState(Weapon weapon, WeaponStateMachine stateMachine, WeaponData weaponData, string animBoolName) : base(weapon, stateMachine, weaponData, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        weapon.InputHandler.notJammed = false;
        //Start timer for jammed weapon
        //This may change to need a right click in order to unjam. We might put in reloading, and make 
        //it only work on beat too
        jammedTimer = weaponData.cooldownTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        //If the timer has not reached 0 yet, subtract time from it
        if(jammedTimer > 0)
        {
            jammedTimer -= Time.deltaTime;
        }
        //If the jammed timer has finished, change state to idle
        else
        {
            weapon.InputHandler.notJammed = true;
            stateMachine.ChangeState(weapon.IdleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
