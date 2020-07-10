using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    private void Update()
    {
        //If the bullet goes off screen, destroy the gameobject
        if(transform.position.x > 20 || transform.position.x < -20 ||
            transform.position.y > 20 || transform.position.y < -20)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 8)
        {
            return;
        }
        //Destroy the bullet on collision
        Destroy(gameObject);
    }
}
