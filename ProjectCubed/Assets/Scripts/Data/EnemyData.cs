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
    public float[] enemyMaxHealth =
    {
        100, //Slime
        100, //Slime Ice
        100, //Slime Fire
        200
    };




}

