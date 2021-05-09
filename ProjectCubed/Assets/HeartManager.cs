using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartManager : MonoBehaviour
{
    public List<Image> heartList;
    [SerializeField]
    private Transform firstHeartPlace;
    [SerializeField]
    private GameObject heartPrefab;

    private Player player;
    private bool start = false;

    private Canvas canvas;

    // Start is called before the first frame update
    void Start()
    {
        //Get the player
        player = GameObject.Find("PlayerPrefab").transform.Find("Player").GetComponent<Player>();
        //Get the first heart place
        firstHeartPlace = transform.Find("FirstHeartSpawn").GetComponent<Transform>();

        start = true;

        //canvas = transform.parent.GetComponent<Canvas>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //If script has just started
        if(start == true)
        {
            //While heartlist is null or heartList does not equal the current health
            while (heartList.Count != player.playerData.currentHealth)
            {
                //Add heart to screen
                AddHeart();
            }

            start = false;
        }

    }

    public void FullHeath()
    {
        while(heartList.Count != player.playerData.maxHealth)
        {
            AddHeart();
            player.playerData.currentHealth++;
        }
    }

    public void IncreaseMaxHealth(int newLife)
    {
        //Set new max health
        player.playerData.maxHealth += newLife;

        //Call full health function
        FullHeath();

    }

    public void AddHeart()
    {
        //Instantiate new heart, make sure its a child of the heartContainer.
        //Make sure it is 120 points to the right of the final heart.
        //Add it to the list
        Transform newHeartPos;
        newHeartPos = firstHeartPlace;

        if (heartList.Count == 0 || heartList == null)
        {
            newHeartPos.position = new Vector2(firstHeartPlace.position.x + (120 * heartList.Count),
                                                    firstHeartPlace.position.y);

        }
        else
        {
            newHeartPos.position = new Vector2(heartList[heartList.Count - 1].transform.position.x + 120f, firstHeartPlace.position.y);

        }

        GameObject newHeart = Instantiate(heartPrefab, newHeartPos.position, Quaternion.identity);
        newHeart.transform.SetParent(transform);
        heartList.Add(newHeart.GetComponent<Image>());

    }

    public void RemoveHeart()
    {
        //Turn off the far right heart
        GameObject deadHeart = heartList[heartList.Count - 1].gameObject;
        Animator newAnim = deadHeart.GetComponent<Animator>();
        //Set heart animator bool to true, to play the explode animation
        newAnim.SetBool("ExplodeHeart", true);


        //heartList[heartList.Count - 1].gameObject.SetActive(false);

        //Remove the far right heart from the list.
        heartList.Remove(heartList[heartList.Count - 1]);
        //Destroy(deadHeart);

    }
}
