using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerMovementState
{
    public PlayerMoveState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Move state");
    }

    public override void Exit()
    {
        base.Exit();
        //player.alreadyMoving = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();


        player.SetVelocityXY(playerData.movementVel * xyInput.x, 
                            playerData.movementVel * xyInput.y);

        player.CheckToFlipLegs(xyInput.x);

        //If the x value is 0 from the input variable, change state to the idle state
        if (xyInput.x == 0 && xyInput.y == 0 && !isExitingState && player.InputHandler.moveInput == false && player.alreadyMoving)
        {
            Debug.Log("x = " + xyInput.x + "y = " + xyInput.y);
            player.alreadyMoving = false;
            stateMachine.ChangeState(player.IdleState);
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
