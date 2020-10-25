using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/Base Data")]
//Inheriting from ScriptableObject allows me to create an asset from this script
public class PlayerData : ScriptableObject
{
    [Header("Move State")]
    public float movementVel = 1.5f;
    public float moveDrag = 14f;

    [Header("Dodge State")]
    public float dodgeCooldown = 0.5f;
    public float dodgeVel = 30f;
    public float dodgeDrag = 30f;
    public float dodgeEndYMultiplier = 0.2f;
    public float dodgeTime = 0.1f;
    //Probably wont need this if Oli decides to do the weird leg stepping over dodge
    public float distBetweenAfterImages = 0.5f;
}
