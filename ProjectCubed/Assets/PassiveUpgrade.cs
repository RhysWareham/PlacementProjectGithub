using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveUpgrade : Upgrades
{
    public enum ChangeableStats
    {
        HEALTH,
        DAMAGE,
        SPEED,
        LUCK
    }

    public ChangeableStats currentStatChange;

    public override void DoAction(Player player)
    {
        
    }


}
