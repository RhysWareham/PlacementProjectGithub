﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAimState : WeaponState
{
    private Vector2 weaponAim;
    private Vector2 weaponAimInput;

    private bool shootInput;

    public bool notJammed;

    private GameObject playerParent;
    private Player player;

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
        playerParent = weapon.gameObject.transform.parent.gameObject;
        player = playerParent.transform.Find("Player").GetComponent<Player>();
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

        //Call function to check weapon placement
        weapon.CheckWeaponPlacement();

        #endregion

        //if(GameManagement.debug == true)
        //{
        //  GameManagement.debug = false;
        //}

        #region PlayerRotation

        //Wrap the angle
        angle = PublicFunctions.UnwrapAngle(angle);

        //For i is less than the number of BodyAngles
        for(int i = 0; i < player.numOfBodyAngles; i++)
        {
        //If the angle is within a 36 degree gap...
            if(angle >= (0 + (36 * i)) && angle < (36 * (i + 1)))
            {
                //Set the new current body angle to the numOfBodyAngles minus (i + 1)
                GameManagement.currentHeadBodyAngle = player.numOfBodyAngles - (i + 1);
                
                //If currentHeadBody angle is not the same as the previous
                if(GameManagement.currentHeadBodyAngle != GameManagement.previousHeadBodyAngle)
                {
                    //Set the previousHeadBodyAngle to the new one
                    GameManagement.previousHeadBodyAngle = GameManagement.currentHeadBodyAngle;

                    //Then call the SetBodyAngle function, to update the sprite
                    player.SetBodyAngleActive();

                }
            }

            
        }

        #endregion





        #region WeaponShooting

        //If the player has pressed the shoot button... 
        if (shootInput)
        {
            //If the weapon is not jammed, and the shot is on beat
            if(notJammed && weapon.BeatDetector.CheckIfOnBeat())
            {
                stateMachine.ChangeState(weapon.ShootState);
            }
            //If the weapon is not jammed, but the shot is not on beat
            else if (notJammed && !weapon.BeatDetector.CheckIfOnBeat())
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

//Rhys Wareham