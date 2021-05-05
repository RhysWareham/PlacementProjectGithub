using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUpgrade : Upgrades
{
    public GameManagement.WeaponUpgrades thisWeaponType;
    public float thisShotDistance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        //If the player has collided with the Item
        if (collision.CompareTag("PlayerSprite"))
        {
            //Set the current Upgrade and Weapon type
            GameManagement.currentUpgradeType = thisUpgradeType;
            GameManagement.currentWeaponType = thisWeaponType;
            GameManagement.shotDistance = thisShotDistance;
            //Destroy the item
            //Want to add disappearing animation for item before destroying it
            Destroy(gameObject);
        }
    }
}
