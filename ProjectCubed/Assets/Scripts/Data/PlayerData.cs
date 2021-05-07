﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/Base Data")]
//Inheriting from ScriptableObject allows me to create an asset from this script
public class PlayerData : ScriptableObject
{
    [Header("General Variables")]
    public int currentHealth = 3;
    public int startingHealth = 3;
    //This can increase during the game if gaining health Ups or something.
    //At beginning of game, set to 100
    public int maxHealth = 3;
    public int attackDamage = 1;
    public int startingDamage = 1;

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
