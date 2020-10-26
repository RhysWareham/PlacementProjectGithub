using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponIdleState : WeaponAimState
{
    public WeaponIdleState(Weapon weapon, WeaponStateMachine stateMachine, WeaponData weaponData, string animBoolName) : base(weapon, stateMachine, weaponData, animBoolName)
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
    }
}
