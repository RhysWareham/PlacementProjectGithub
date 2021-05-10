using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Pathfinding;

public class LevelManager : MonoBehaviour
{
    private ShapeCubeManager cubeManager;

    private GameObject pathfinder;

    [SerializeField]
    private EnemyData enemyData;

    private int numOfEnemiesSpawned = 0;

    private List<Transform> levelSpawnPoints;

    private bool moveToNextLevel = false;

    private Vector3 spawnPoint = new Vector3(0, 0, -3);

    private float timer = 5;


    [SerializeField]
    private GameObject gameOverMenu;
    [SerializeField]
    private GameObject levelCompleteMenu;

    [SerializeField]
    private MenuScriptNew menuSystem;

    private bool levelStart;
    private float startGameTimer;
    private float startGameMaxTimer = 2f;

    public bool unlockPlanets = false;
    public static List<GameObject> listOfEnemies = new List<GameObject>();

    private void Awake()
    {
        cubeManager = GameObject.Find("Cube").GetComponent<ShapeCubeManager>();
        //AstarPath.active.Scan();
        //pathfinder.GetComponent<AStarPath>()
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManagement.playerAlive = true;
        levelCompleteMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        

        levelSpawnPoints = new List<Transform>();


        switch(ShapeInfo.chosenShape)
        {
            case 0: //CUBE
                cubeManager.SetSpawnPoints(ref levelSpawnPoints);
                break;
        }

        //Reset all faces to incomplete
        for (int i = 0; i < cubeManager.faceComplete.Length; i++)
        {
            cubeManager.faceComplete[i] = false;
        }

        cubeManager.currentFace = 0;

        GameManagement.enemySpawningComplete = false;
        GameManagement.canStartSpawning = true;
        numOfEnemiesSpawned = 0;
        Time.timeScale = 1f;
        levelStart = true;
        startGameTimer = startGameMaxTimer;
    }

    // Update is called once per frame
    void Update()
    {
        //If unlockPlanets has been ticked, and enemies are not dead
        if (unlockPlanets)
        {
            GameManagement.UnlockRotation = true;
            
            if(GameManagement.enemiesLeftAliveOnFace > 0)
            {
                //Call the function to destroy all enemies
                DestroyAllEnemies(listOfEnemies);

            }
        }
        //If unlock planets is not ticked
        else if(!unlockPlanets)
        {
            //Set unlock rotation to false
            GameManagement.UnlockRotation = false;
        }

        //If start of level, start a timer for the starting text
        if (levelStart == true)
        {
            menuSystem.planetStartText.SetActive(true);
            if (startGameTimer > 0)
            {
                startGameTimer -= Time.deltaTime;
            }
            else
            {
                menuSystem.planetStartText.SetActive(false);
                levelStart = false;
                
            }
        }
       



        //if the current face is not complete
        if(!cubeManager.faceComplete[(int)cubeManager.currentFace] && !GameManagement.clearedTextChecked)
        {
            menuSystem.SetFaceClearedText(false);
            GameManagement.clearedTextChecked = true;
        }

        if(GameManagement.playerAlive == false)
        {
            GameOver();
        }

        //If starting newPlanet
        if(GameManagement.newPlanet == true)
        {
            GameManagement.newPlanet = false;
            GameManagement.currentPlanet++;

        }

        //If number of enemies spawned is less than the max for the current face, and canStartSpawning is true
        if(numOfEnemiesSpawned < GameManagement.maxNumOfEnemiesForFace && GameManagement.canStartSpawning)
        {
            //Set enemySpawningComplete to false
            GameManagement.enemySpawningComplete = false;
            if(timer < 0)
            {
                SpawnEnemy();
                timer = Random.Range(2f, 5f);
                //timer = 3f;
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
        if(GameManagement.enemySpawningComplete && GameManagement.enemiesLeftAliveOnFace <= 0 && !GameManagement.faceChecked)
        {
            FaceCompleteActions();

            
        }

        //LevelComplete();

        //If ready to move to next level, (after the timer has finished on the levelCleared menu)
        if(moveToNextLevel)
        {
            menuSystem.LoadLevelSelect();
        }
    }


    public void SpawnEnemy()
    {
        //Pick random enemy from range
        int randEnemy = Random.Range(0, enemyData.enemyType.Length);
        //Instantiate new enemy gameobject, using the array of enemyTypes stored in the EnemyData script
        int randSpawnPoint = Random.Range(0, levelSpawnPoints.Count);
        while(levelSpawnPoints[randSpawnPoint].GetComponent<SpawnPoint>().canSpawn == false)
        {
            randSpawnPoint = Random.Range(0, levelSpawnPoints.Count);
        }

        GameObject newEnemy = Instantiate(enemyData.enemyType[randEnemy], levelSpawnPoints[randSpawnPoint].position, Quaternion.identity);
        //Set the currentEnemyType variable in the newEnemy's Enemy script, so it knows what functions to use
        newEnemy.GetComponent<Enemy>().enabled = true;
        newEnemy.GetComponent<Enemy>().currentEnemyType = randEnemy + 1;
        //Set the health of the Enemy
        newEnemy.GetComponent<Enemy>().health = enemyData.enemyMaxHealth[randEnemy];

        /////////////////////////////////////SOEMTHING IS WEIRD WITH THIS
        //Add the new enemy to the list of enemies
        listOfEnemies.Add(newEnemy);

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
        menuSystem.LoadMenu(menuSystem.levelClearedMenu);

        //The timer coroutine won't run if timescale is at 0
        Time.timeScale = 1f;

        //Give reward

        //Go to planet selection scene
        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        float timer2 = 5f;
        while(timer2 >= 0f)
        {
            timer2 -= Time.deltaTime;
            yield return 0;
        }

        moveToNextLevel = true;
    }

    public void GameOver()
    {
        menuSystem.LoadMenu(gameOverMenu);
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

        GameManagement.faceChecked = true;

        //Signify the face is complete
        Debug.Log("Face Clear");



        //If all faces are not complete
        if (!cubeManager.CheckAllFacesAreComplete())
        {
            //Start coroutine to signify the face being cleared
            menuSystem.SetFaceClearedText(true);

        }
        else
        { 
            //If all faces are complete, the level is complete
            LevelComplete();
        }
    }

    public void DestroyAllEnemies(List<GameObject> listOfEnemies)
    {
        foreach (GameObject go in listOfEnemies)
        {
            KillEnemy(go);
        }

    }

    public static void KillEnemy(GameObject enemy)
    {
        
        listOfEnemies.Remove(enemy);
        Destroy(enemy);
        GameManagement.enemiesLeftAliveOnFace--;
    }

}


//Rhys Wareham