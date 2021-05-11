using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : PassiveUpgrade
{
    private int healthInceaseValue = 1;
    private bool notDone = false;

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
        notDone = true;
        player.heartManager.AddHeart();
        player.playerData.currentHealth++;
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        //If the player has collided with the item
        if (collision.CompareTag("PlayerSprite"))
        {
            if (!notDone)
            {
                //Call the function to apply upgrades
                DoAction(collision.gameObject.GetComponentInParent<Player>());
            }
            else
            {
                notDone = false;
            }
            //Destroy the item
            //Have a disappearing animation for item
            Destroy(gameObject);
        }
    }
}
