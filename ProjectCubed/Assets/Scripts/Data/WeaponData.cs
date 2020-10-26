﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newWeaponData", menuName = "Data/Weapon Data/Base Data")]
public class WeaponData : ScriptableObject
{
    [Header("Jammed State")]
    public float cooldownTime = 2.0f;

    [Header("Shoot State")]
    public float beatDelay = 1f;
    public float bulletForce = 10f;
    public float shootTime = 0.5f;
}