using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgeState : PlayerAbilityState
{
    //Variable for if the player can dodge
    public bool CanDodge { get; private set; }
    //Time since last dodge
    private float lastDodgeTime;
    private bool isPressed;

    private Vector2 dodgeDirection;
    private Vector2 dodgeDirectionInput;

    public PlayerDodgeState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        //Set CanDodge to false, once starting the dodge state
        CanDodge = false;

        isPressed = true;
        //Set the default dodgeDirection to the player's facing direction
        dodgeDirection = Vector2.right * player.FacingRight;

        player.CreateDust();
    }

    public override void Exit()
    {
        base.Exit();

        if(player.CurrentVelocity.y > 0)
        {
            player.SetVelocityY(player.CurrentVelocity.y * playerData.dodgeEndYMultiplier);
        }

        
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        //If not exiting a state
        if(!isExitingState)
        {
            //If dodge button has been pressed
            if(isPressed)
            {
                //Set isPressed to false, to stop from being able to hold down
                isPressed = false;

                //Get the dodgeDirectionInput from InputHandler
                dodgeDirectionInput = player.InputHandler.RawDodgeDirectionInput;

                //If dodgeDirectionInput is not zero
                if(dodgeDirectionInput != Vector2.zero)
                {
                    //Set the dodgeDirection and then normalize
                    dodgeDirection = dodgeDirectionInput;
                    dodgeDirection.Normalize();
                }

                
                
                Time.timeScale = 1f;
                startTime = Time.time;
                //Check if player legs need to flip
                player.CheckToFlipLegs(Mathf.RoundToInt(dodgeDirection.x));
                //Set the drag
                player.RB.drag = playerData.dodgeDrag;

                //Set velocity
                player.SetVelocityWithDirection(playerData.dodgeVel, dodgeDirection);

            }
            //If player has released the button, 
            else
            {
                //If the time of the dodge has finished, set drag back to default
                if(Time.time >= startTime + playerData.dodgeTime)
                {
                    player.RB.drag = playerData.moveDrag;
                    isAbilityDone = true;
                    //Set Last dodge time for countdown
                    lastDodgeTime = Time.time;
                    stateMachine.ChangeState(player.IdleState);
                }
                
            }
        }
    }


    /// <summary>
    /// Function which will return true if CanDodge is true, and the time since last dodge is greater than the cooldown
    /// </summary>
    /// <returns></returns>
    public bool CheckIfCanDodge()
    {
        return CanDodge && Time.time >= lastDodgeTime + playerData.dodgeCooldown;
    }

    /// <summary>
    /// Sets CanDodge to true
    /// </summary>
    public void ResetCanDodge() => CanDodge = true;

}
