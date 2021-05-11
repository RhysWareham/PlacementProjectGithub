using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUpgrade : PassiveUpgrade
{
    public int healthIncreaseValue = 1;


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
        player.heartManager.IncreaseMaxHealth(healthIncreaseValue);
    }


}
