using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private EnemyData enemyData;

    private Vector3 spawnPoint = new Vector3(0, 0, -3);

    private float timer = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(timer < 0)
        {
            SpawnEnemy();
            timer = 5f;
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }


    public void SpawnEnemy()
    {
        //Pick random enemy from range
        int randEnemy = Random.Range(0, enemyData.enemyType.Length);
        //Instantiate new enemy gameobject, using the array of enemyTypes stored in the EnemyData script
        GameObject newEnemy = Instantiate(enemyData.enemyType[randEnemy], spawnPoint, Quaternion.identity);
        //Set the currentEnemyType variable in the newEnemy's Enemy script, so it knows what functions to use
        newEnemy.GetComponent<Enemy>().enabled = true;
        newEnemy.GetComponent<Enemy>().currentEnemyType = randEnemy;
        //Set the health of the Enemy
        newEnemy.GetComponent<Enemy>().health = enemyData.enemyMaxHealth[randEnemy];
    }
}
