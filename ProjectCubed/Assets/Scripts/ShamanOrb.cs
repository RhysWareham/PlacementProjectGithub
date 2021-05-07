using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShamanOrb : MonoBehaviour
{
    public int damage = 1;
    public float shotDistance;
    private Vector2 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        //If the bullet goes off screen, destroy the gameobject
        if (transform.position.x > 20 || transform.position.x < -20 ||
            transform.position.y > 20 || transform.position.y < -20)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //If the collision is Player
        if(collision.gameObject.tag == "Player")
        {
            //Call player takeDamage function
            collision.gameObject.GetComponent<Player>().TakeDamage(damage);
        }

        //Destroy the bullet on collision
        Destroy(gameObject);
        
    }
}
