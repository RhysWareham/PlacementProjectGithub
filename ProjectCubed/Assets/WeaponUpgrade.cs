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



    public override void DoAction(Player player)
    {
        //Set the current Upgrade and Weapon type
        GameManagement.currentUpgradeType = thisUpgradeType;
        GameManagement.currentWeaponType = thisWeaponType;
        GameManagement.shotDistance = thisShotDistance;

    }
}
