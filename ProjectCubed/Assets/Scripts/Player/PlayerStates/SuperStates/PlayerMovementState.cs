using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementState : PlayerState
{
    protected Vector2 xyInput;
    private bool dodgeInput;

    public PlayerMovementState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

        player.DodgeState.ResetCanDodge();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        //Move state and Idle state will now have access to this movement input variable
        xyInput = player.InputHandler.RawMovementInput;
        //xyInput = new Vector2(player.InputHandler.NormalisedInputX, player.InputHandler.NormalisedInputY);
        dodgeInput = player.InputHandler.DodgeInput;
        
        //If dodgeInput is true, and CheckIfCanDodge returns true...
        if(dodgeInput && player.DodgeState.CheckIfCanDodge())
        {
            //Change state to DodgeState
            stateMachine.ChangeState(player.DodgeState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
