using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrades : MonoBehaviour
{
    public GameManagement.UpgradeType thisUpgradeType;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        //If the player has collided with the item
        if(collision.CompareTag("PlayerSprite"))
        {
            //Call the function to apply upgrades
            DoAction(collision.gameObject.GetComponentInParent<Player>());
            //Destroy the item
            //Have a disappearing animation for item
            Destroy(gameObject);
        }
    }

    public virtual void DoAction(Player player)
    {

    }
}
