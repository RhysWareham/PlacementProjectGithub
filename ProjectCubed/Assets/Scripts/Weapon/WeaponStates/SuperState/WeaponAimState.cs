using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAimState : WeaponState
{
    private Vector2 weaponAim;
    private Vector2 weaponAimInput;

    protected bool isShotDone;

    public bool CanShoot { get; private set; }

    public WeaponAimState(Weapon weapon, WeaponStateMachine stateMachine, WeaponData weaponData, string animBoolName) : base(weapon, stateMachine, weaponData, animBoolName)
    {
    }


    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

        isShotDone = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        //If the shoot animation is complete, return to idle state
        if(isShotDone)
        {
            stateMachine.ChangeState(weapon.IdleState);
        }


        //Get the AimDirection from InputHandler
        weaponAimInput = weapon.InputHandler.RawAimDirectionInput;

        //If weaponAimInput is not zero
        //If we're not getting any input, this if statement won't run.
        if (weaponAimInput != Vector2.zero)
        {
            //Set the weaponAim and then normalize
            weaponAim = weaponAimInput;
            weaponAim.Normalize();
        }

        //Returns the angle in degrees between the 2 vectors
        float angle = Vector2.SignedAngle(Vector2.right, weaponAim);
        //Subtract 90 degrees from the angle to get correct angle to face the mouse
        angle -= 90f;
        //Set the new rotation of the weapon, to point towards the mouse
        weapon.WeaponPivotPoint.rotation = Quaternion.Euler(0f, 0f, angle);

        weapon.CheckWeaponPlacement();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
