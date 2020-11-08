using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOnLandState : PlayerMovementState
{
    public PlayerOnLandState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {
            //If the x or y value for the movement input is not 0
            if (xyInput.x != 0 || xyInput.y != 0)
            {
                //Set the state to MoveState
                stateMachine.ChangeState(player.MoveState);
            }
            //If all animations are finished
            else if (isAnimationFinished)
            {
                //Set the state to Idle
                stateMachine.ChangeState(player.IdleState);
            }
        }

    }
}
