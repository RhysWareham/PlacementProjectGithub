﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Tumbleweed : EnemyType
{


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //Using this override function, I am able to call this specific function through 
    //calling UpdateMovement() in the parent class EnemyType.
    //This is because the specific enemy script will change depending on the enemy, but this means I can
    //call a function as long as it is named the same and an override 
    public override void UpdateMovement(Enemy _enemy)
    {
        //Get direction of which way the enemy should move
        Vector2 direction = ((Vector2)enemy.path.vectorPath[enemy.currentWaypoint] - enemy.rb.position).normalized;
        Vector2 force = direction * enemy.enemyData.enemyMaxSpeed[enemy.currentEnemyType - 1] * Time.deltaTime;

        //Add the calculated force to the enemy rigidbody
        enemy.rb.AddForce(force);

        //Distance from next waypoint
        float distance = Vector2.Distance(enemy.rb.position, enemy.path.vectorPath[enemy.currentWaypoint]);

        if (distance < enemy.nextWaypointDistance)
        {
            enemy.currentWaypoint++;
        }
    }

    public override void Attack(Enemy enemy)
    {
        

    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        //If collision object is a playerProjectile
        if (collision.gameObject.CompareTag("PlayerProjectile"))
        {
            //Call OnHit function in EnemyScript
            enemy.OnHit(collision);


            //Destroy projectile
            Destroy(collision.gameObject);
        }
        //If hit player, make player take damage
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().TakeDamage(enemy.enemyData.enemyAttackDamage[enemy.currentEnemyType -1]);
        }
    }
}
