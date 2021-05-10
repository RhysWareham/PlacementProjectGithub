using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public bool canSpawn = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Water") || collision.CompareTag("Obstacle"))
        {
            canSpawn = false;
        }
        
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        canSpawn = true;
    }
}
