using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newWeaponData", menuName = "Data/Weapon Data/Base Data")]
public class WeaponData : ScriptableObject
{
    [Header("Jammed State")]
    private float cooldownTime = 2.0f;
}
