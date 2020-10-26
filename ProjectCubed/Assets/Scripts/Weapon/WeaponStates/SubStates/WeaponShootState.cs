using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShootState : WeaponAimState
{
    public bool CanShoot { get; private set; }

    private float lastShotTime;
    private bool isPressed;

    public WeaponShootState(Weapon weapon, WeaponStateMachine stateMachine, WeaponData weaponData, string animBoolName) : base(weapon, stateMachine, weaponData, animBoolName)
    {

    }


    public override void Enter()
    {
        base.Enter();

        isPressed = true;
        CanShoot = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(!isExitingState)
        {
            //If player has clicked shoot
            if (isPressed)
            {
                //Set is pressed to false to prevent from holding shoot down
                isPressed = false;

                //Check if the shot is on beat
                CanShoot = CheckIfShotOnBeat();
                //Debug.Log(CanShoot);


                //If player can shoot
                if (CanShoot)
                {
                    //Call the spawn bullet function
                    weapon.SpawnBullet();
                    
                    Time.timeScale = 1f;
                    //Set lastShotTime to now
                    lastShotTime = Time.time;
                    animStartTime = Time.time;

                    //Set canShoot to false
                    CanShoot = false;
                    weapon.InputHandler.shot = false;
                }
                //If the player cannot shoot right now,
                else
                {
                    //Set state to Jammed State
                    stateMachine.ChangeState(weapon.JamState);
                }

            }
            else
            {
                //If the time is later than animation start time plus shoot time
                if(Time.time >= animStartTime + weaponData.shootTime)
                {
                    //Set isShootDone (animation) to true and change state to IDLE
                    isShotDone = true;
                    stateMachine.ChangeState(weapon.IdleState);
                }
            }
        }
    }

    /// <summary>
    /// Function returns true if notJammed is true, and the beat delay has passed
    /// </summary>
    /// <returns></returns>
    public bool CheckIfShotOnBeat()
    {
        return notJammed;
            //&& Time.time >= lastShotTime + weaponData.beatDelay;
    }

    

}
