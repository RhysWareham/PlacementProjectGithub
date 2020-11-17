using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private EnemyData enemyData;

    private Vector2 spawnPoint = new Vector2(0, 0);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SpawnEnemy()
    {
        //Pick random enemy from range
        int randEnemy = Random.Range(0, 3);
        //Instantiate new enemy gameobject, using the array of enemyTypes stored in the EnemyData script
        GameObject newEnemy = Instantiate(enemyData.enemyType[randEnemy], spawnPoint, Quaternion.identity);
        //Set the currentEnemyType variable in the newEnemy's Enemy script, so it knows what functions to use
        newEnemy.GetComponent<Enemy>().currentEnemyType = randEnemy;
    }
}
