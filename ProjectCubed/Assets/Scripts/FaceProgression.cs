using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceProgression : MonoBehaviour
{
    private GameObject[] enemies;
    public int enemiesAlive;


    // Start is called before the first frame update
    void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        enemiesAlive = enemies.Length;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
