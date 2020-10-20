using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootState : PlayerAbilityState
{
    public PlayerShootState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    ////If dodge button has been pressed
    //        if(isHolding)
    //        {
    //            //Get the dodgeDirectionInput from InputHandler
    //            dodgeDirectionInput = player.InputHandler.RawDodgeDirectionInput;

    //            //If dodgeDirectionInput is not zero
    //            if(dodgeDirectionInput != Vector2.zero)
    //            {
    //                //Set the dodgeDirection and then normalize
    //                dodgeDirection = dodgeDirectionInput;
    //                dodgeDirection.Normalize();
    //            }

    //            //Returns the angle in degrees between the 2 vectors
    //            float angle = Vector2.SignedAngle(Vector2.right, dodgeDirection);
    //            player.weapon.rotation = Quaternion.Euler(0f, 0f, angle);
    //        }

}
