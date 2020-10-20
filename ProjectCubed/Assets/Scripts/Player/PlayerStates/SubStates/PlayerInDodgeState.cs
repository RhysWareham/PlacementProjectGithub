using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInDodgeState : PlayerState
{
    public PlayerInDodgeState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }
}
