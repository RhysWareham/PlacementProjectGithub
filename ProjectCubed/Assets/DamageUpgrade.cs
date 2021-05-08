using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageUpgrade : PassiveUpgrade
{
    public int damageIncreaseValue = 1;

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
        //Increase the attack damage
        player.playerData.attackDamage += damageIncreaseValue;
    }
}
