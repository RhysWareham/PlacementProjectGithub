﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScriptNew : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Why isn't the button being clicked?????????????????????????????
    public void LoadLevelSelect()
    {
        SceneManager.LoadScene("PlanetSelector");
    }
}
