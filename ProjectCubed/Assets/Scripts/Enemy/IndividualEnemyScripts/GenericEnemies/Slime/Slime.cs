using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Slime : EnemyType
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
        //enemy.transform.localPosition = new Vector2(1, 1);
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        //If collision object is a playerProjectile
        if (collision.gameObject.tag == "PlayerProjectile")
        {
            //Call OnHit function in EnemyScript
            enemy.OnHit(collision);
            

            //Destroy projectile
            Destroy(collision.gameObject);
        }
    }
}
