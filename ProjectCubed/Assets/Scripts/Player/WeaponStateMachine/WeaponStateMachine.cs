using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStateMachine
{
    public WeaponState CurrentState { get; private set; }

    public void Initialise(WeaponState startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }

    public void ChangeState(WeaponState newState)
    {
        //Call the exit function of the current state
        CurrentState.Exit();
        //Set the current state to the new state
        CurrentState = newState;
        //Call the enter function of the new state
        CurrentState.Enter();
    }
}
