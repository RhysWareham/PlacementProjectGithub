using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    //any script with a reference to this playerstate, can get the state
    //But the state can only be set from within this script
    public PlayerState CurrentState { get; private set; }


    public void Initialise(PlayerState startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }

    public void ChangeState(PlayerState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }


}
