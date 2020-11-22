using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyType : MonoBehaviour
{
    protected Enemy enemy;

    private void Awake()
    {
        //Get the insatnce of enemy from parent
        enemy = transform.GetComponentInParent<Enemy>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Created a virtual function which will be called from the stateMachine, 
    //and then overridden by the function of the specific enemy.
    public virtual void UpdateMovement(Enemy enemy) { }

    public virtual void Attack() { }

    public virtual void OnCollisionEnter2D(Collision2D collision) { }
    
}
