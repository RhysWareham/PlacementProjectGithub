﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Slime Ice will have a different attack to Slime, but same movement
public class SlimeIce : Slime
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Using an override function in SlimeIce, means we can call this specific function, 
    //instead of the function in the parent class of Slime.
    public override void UpdateMovement(Enemy enemy)
    {
        enemy.transform.localPosition = new Vector2(5, 5);
    }

    public override void Attack() 
    { 
        
    }
}
