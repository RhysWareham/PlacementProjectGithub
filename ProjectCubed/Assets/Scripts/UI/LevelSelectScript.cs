//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class LevelSelectScript : MonoBehaviour
//{
//    public GameObject levelSelectMenu;
//    public GameObject options;
//    public GameObject mainMenu;
//    public GameObject deathMenu;

//    [SerializeField]
//    private GameObject player;
//    [SerializeField]
//    private Transform playerStart;

//    private float timer = 3.0f;



//    public void LoadLevel(int levelNum)
//    {


//        LevelManager.chosenLevel = levelNum;
//        //Split up the strings of chunks for the chosen level
//        string[] chunkList = LevelManager.chunksInLevel[(levelNum-1)].Split(',');

//        //Go through the chunks within this level, and add them to the List
//        for(int i = 0; i < chunkList.Length; i++)
//        {
//            LevelManager.chunkNumbersInLevel.Add(int.Parse(chunkList[i]));
//            Debug.Log(int.Parse(chunkList[i]));
//        }

//        LevelManager.numOfChunks = LevelManager.chunkNumbersInLevel.Count;
//        LevelManager.levelCompleted = false;
//        LevelManager.isRunning = true;
//        LevelManager.startTimer = timer;
//        LevelManager.scoreUpdated = false;
//        LevelManager.score = 0;

        

//        Instantiate(player, playerStart.position, Quaternion.identity);


//        //Turn off level select canvas page.
//        levelSelectMenu.SetActive(false);
//    }
//}
