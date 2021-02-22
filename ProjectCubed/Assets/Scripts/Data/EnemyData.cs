using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newEnemyData", menuName = "Data/Enemy Data/Base Data")]
public class EnemyData : ScriptableObject
{
    public string[] enemyNames = 
    { "Slime",
      "SlimeIce",
      "SlimeFire"
    };

    public GameObject[] enemyType;
    public int[] enemyMaxHealth =
    {
        1, //Slime
        2, //Slime Ice
        1, //Slime Fire
        2
    };

    public int[] enemyAttackDamage =
    {
        1,
        1,
        1,
        1
    };

    public float[,] enemyAttackDamageRange =
    {
        { 10f, 20f },
        { 5f, 10f}
    };

    public float[] enemyMaxSpeed =
    {
        10f,
        10f,
        10f
    };

    public float[] enemyAttackImpactRadius =
    {
        0.6f,
        0.6f,
        0.6f
    };

    public float timeBtwAttack = 2.0f;
}

