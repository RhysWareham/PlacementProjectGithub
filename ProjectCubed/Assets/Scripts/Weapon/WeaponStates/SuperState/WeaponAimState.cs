using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAimState : WeaponState
{
    private Vector2 weaponAim;
    private Vector2 weaponAimInput;

    private bool shootInput;

    public bool notJammed;

    

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


    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();


        //Get the AimDirection from InputHandler
        weaponAimInput = weapon.InputHandler.RawAimDirectionInput;
        shootInput = weapon.InputHandler.ShootInput;
        notJammed = weapon.InputHandler.NotJammed;

        #region WeaponAiming
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

        #endregion

        #region WeaponShooting

        //If the player has pressed the shoot button... 
        if (shootInput)
        {
            //If the weapon is not jammed, and the shot is on beat
            if(notJammed && weapon.ShootState.CheckIfOnBeat())
            {
                stateMachine.ChangeState(weapon.ShootState);
            }
            //If the weapon is not jammed, but the shot is not on beat
            else if (notJammed && !weapon.ShootState.CheckIfOnBeat())
            {
                //Change state to JamState
                stateMachine.ChangeState(weapon.JamState);
            }
            //If the weapon is Jammed 
            else if(!notJammed)
            {
                //Do Nothing
                return;
            }
        }

        #endregion

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
