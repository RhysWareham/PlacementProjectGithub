using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Shaman : EnemyType
{
    private float timeBtwShot;
    private float maxTimeBtwShot = 7f;
    private float minTimeBtwShot = 3f;
    [SerializeField]
    private Transform FirePoint;
    [SerializeField]
    private GameObject projectile;
    public bool shoot = false;
    public float shotSpeed = 0.1f;
    public bool shotAllowed = false;
    public bool isNowDead = false;


    // Start is called before the first frame update
    void Start()
    {
        enemy.health = 3f;
        SetShotTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if(shoot && shotAllowed)
        {
            SpawnBullet();
            shotAllowed = false;
        }

        if(timeBtwShot > 0)
        {
            timeBtwShot -= Time.deltaTime;
        }
        else
        {
            shotAllowed = true;
            Attack(enemy);
            
        }

        if(isNowDead)
        {
            LevelManager.KillEnemy(enemy.gameObject);
        }
    }

    //Using this override function, I am able to call this specific function through 
    //calling UpdateMovement() in the parent class EnemyType.
    //This is because the specific enemy script will change depending on the enemy, but this means I can
    //call a function as long as it is named the same and an override 
    public override void UpdateMovement(Enemy _enemy)
    {
        //Get direction of which way the enemy should move
        Vector2 direction = ((Vector2)enemy.path.vectorPath[enemy.currentWaypoint] - enemy.rb.position).normalized;
        Vector2 force = direction * enemy.enemyData.enemyMaxSpeed[enemy.currentEnemyType] * Time.deltaTime;

        //NEED TO LIMIT THE FORCE/VELOCITY
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
        enemy.rb.velocity = new Vector2(0, 0);
        //Call base function last
        base.Attack(enemy);
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
    }

    public void SpawnBullet()
    {
        GameObject newBullet = Instantiate(projectile, FirePoint.position, FirePoint.rotation);
        Rigidbody2D bulletRB = newBullet.GetComponent<Rigidbody2D>();



        bulletRB.velocity = (enemy.target.transform.position - newBullet.transform.position).normalized * shotSpeed;
        shoot = false;
        SetShotTimer();
    }

    public void SetShotTimer()
    {
        timeBtwShot = Random.Range(minTimeBtwShot, maxTimeBtwShot);
    }
}
