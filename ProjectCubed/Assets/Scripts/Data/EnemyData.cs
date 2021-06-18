using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newEnemyData", menuName = "Data/Enemy Data/Base Data")]
public class EnemyData : ScriptableObject
{
    public string[] enemyNames = 
    { "Slime",
      "SlimeIce",
      "Shaman",
      "Tumbleweed",
      "Sk8rKid"
    };

    public GameObject[] enemyType;
    public int[] enemyMaxHealth =
    {
        1, //Slime
        2, //Slime Ice
        3, //Shaman
        4, //Tumbleweed
        2  //Sk8rKid
    };

    public int[] enemyAttackDamage =
    {
        1,
        1,
        1,
        1,
        1
    };

    //public float[,] enemyAttackDamageRange =
    //{
    //    { 10f, 20f },
    //    { 5f, 10f},

    //};

    public float[] enemyMaxSpeed =
    {
        7f,
        7f,
        4f,
        8f,
        13f
    };

    public float[] enemyAttackImpactRadius =
    {
        0.6f,
        0.6f,
        0.6f,
        0.0f,
        0f
    };

    public float timeBtwAttack = 2.0f;
}

