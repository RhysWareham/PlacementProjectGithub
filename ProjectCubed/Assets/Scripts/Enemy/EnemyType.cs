using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType : MonoBehaviour
{


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
}
