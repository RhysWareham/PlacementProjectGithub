//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class MenuScript : LevelSelectScript
//{

//    public void LoadLevelSelect()
//    {
//        this.gameObject.SetActive(false);
//        levelSelectMenu.SetActive(true);
        
//    }

//    public void RetryLevel()
//    {
//        this.gameObject.SetActive(false);
//        Debug.Log(this.gameObject.activeSelf);
//        LoadLevel(LevelManager.chosenLevel);
//    }

//    public void OptionsMenu()
//    {
//        this.gameObject.SetActive(false);
//        options.SetActive(true);
//    }

//    public void LoadMainMenu()
//    {
//        this.gameObject.SetActive(false);
//        mainMenu.SetActive(true);
//    }

//    public void NextLevel()
//    {
//        if(LevelManager.chosenLevel + 1 > LevelManager.chunksInLevel.Length)
//        {
//            this.gameObject.SetActive(false);
//            LoadLevelSelect();
//        }
//        else
//        {
//            this.gameObject.SetActive(false);
//            LoadLevel(LevelManager.chosenLevel + 1);
//        }
        
//    }

//    public void ResetGame()
//    {
//        PlayerPrefs.DeleteAll();
//    }

//}
