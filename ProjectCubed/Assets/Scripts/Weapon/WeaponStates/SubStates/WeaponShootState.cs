using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShootState : WeaponAimState
{
    //private bool shotIsOnBeat;

    private bool isPressed;

    public WeaponShootState(Weapon weapon, WeaponStateMachine stateMachine, WeaponData weaponData, string animBoolName) : base(weapon, stateMachine, weaponData, animBoolName)
    {

    }


    public override void Enter()
    {
        base.Enter();

        isPressed = true;
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

                
                //Call the spawn bullet function
                weapon.SpawnBullet();
                weapon.InputHandler.bulletShot = true;
                
                Time.timeScale = 1f;
                animStartTime = Time.time;

            }
            else
            {
                //If the time is later than animation start time plus shoot time
                if(Time.time >= animStartTime + weaponData.shootTime)
                {
                    stateMachine.ChangeState(weapon.IdleState);
                }
            }
        }
    }



    /// <summary>
    /// Function to check if the shot is on beat
    /// </summary>
    /// <returns></returns>
    //public bool CheckIfOnBeat()
    //{
    //    int randNum;
    //    randNum = Random.Range(0, 2);
    //    if(randNum == 1)
    //    {
    //        shotIsOnBeat = true;

    //    }
    //    else
    //    {
    //        shotIsOnBeat = false;
    //    }

    //    return shotIsOnBeat;
    //}

    

}
