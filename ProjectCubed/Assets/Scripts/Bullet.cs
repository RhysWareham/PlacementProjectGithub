using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public float shotDistance;
    private Vector2 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        //If the distance the bullet has travelled is more than the shot distance for the current shotType
        if (PublicFunctions.ReturnDistance(startPos, transform.position) > shotDistance)
        {
            //Play dissapear animation?
            Destroy(gameObject);
        }

        //If the bullet goes off screen, destroy the gameobject
        if(transform.position.x > 20 || transform.position.x < -20 ||
            transform.position.y > 20 || transform.position.y < -20)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //If the collision layer is Player, shape Boundary or enemyAICollision, return to allow the bullet through
        if(collision.gameObject.layer == 8 || collision.gameObject.layer == 9 || collision.gameObject.layer == 14)
        {
            return;
        }
        else
        {
            //Destroy the bullet on collision
            Destroy(gameObject);
        }
    }
}
