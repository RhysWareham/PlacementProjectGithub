using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private ShapeCubeManager cubeManager;

    [SerializeField]
    private EnemyData enemyData;

    private int numOfEnemiesSpawned = 0;

    private List<Transform> levelSpawnPoints;



    private Vector3 spawnPoint = new Vector3(0, 0, -3);

    private float timer = 5;

    private void Awake()
    {
        cubeManager = GameObject.Find("Cube").GetComponent<ShapeCubeManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        levelSpawnPoints = new List<Transform>();

        switch(ShapeInfo.chosenShape)
        {
            case 0: //CUBE
                cubeManager.SetSpawnPoints(ref levelSpawnPoints);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //If number of enemies spawned is less than the max for the current face, and canStartSpawning is true
        if(numOfEnemiesSpawned < GameManagement.maxNumOfEnemiesForFace && GameManagement.canStartSpawning)
        {
            //Set enemySpawningComplete to false
            GameManagement.enemySpawningComplete = false;
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
        else
        {
            GameManagement.canStartSpawning = false;
            GameManagement.enemySpawningComplete = true;
        }

        //If enemy spawning is complete, and there's no enemies left alive
        if(GameManagement.enemySpawningComplete && GameManagement.enemiesLeftAliveOnFace <= 0)
        {
            FaceCompleteActions();

            
        }
    }


    public void SpawnEnemy()
    {
        //Pick random enemy from range
        int randEnemy = Random.Range(0, enemyData.enemyType.Length);
        //Instantiate new enemy gameobject, using the array of enemyTypes stored in the EnemyData script
        int randSpawnPoint = Random.Range(0, levelSpawnPoints.Count);
        GameObject newEnemy = Instantiate(enemyData.enemyType[randEnemy], levelSpawnPoints[randSpawnPoint].position, Quaternion.identity);
        //Set the currentEnemyType variable in the newEnemy's Enemy script, so it knows what functions to use
        newEnemy.GetComponent<Enemy>().enabled = true;
        newEnemy.GetComponent<Enemy>().currentEnemyType = randEnemy;
        //Set the health of the Enemy
        newEnemy.GetComponent<Enemy>().health = enemyData.enemyMaxHealth[randEnemy];

        GameManagement.enemiesLeftAliveOnFace++;
        numOfEnemiesSpawned++;
    }


    /// <summary>
    /// Function for end of level stuff
    /// </summary>
    public void LevelComplete()
    {
        //Inform player they have completed a planet
        Debug.Log("Level Complete!! Well Done!");

        //Give reward

        //Go to planet selection scene

    }


    public void FaceCompleteActions()
    {
        //Set enemies alive to 0, just in case
        GameManagement.enemiesLeftAliveOnFace = 0;
        //Set the face completed to true
        cubeManager.faceComplete[(int)cubeManager.currentFace] = true;
        //Allow the planet to rotate
        GameManagement.PlanetCanRotate = true;
        //Reset the number of enemies spawned to 0
        numOfEnemiesSpawned = 0;

        //Signify the face is complete
        Debug.Log("Face Clear");

        //If all faces are now complete
        if (cubeManager.CheckAllFacesAreComplete())
        {
            LevelComplete();
        }
    }
}


//Rhys Wareham