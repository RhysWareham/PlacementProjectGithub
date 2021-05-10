using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    private Animator anim;
    private float startTime;
    private bool animStarted = false;
    public bool animFinished = false;

    private float beatTime;
    private float deathTime;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        UpdateAnimClipTimes();
    }

    // Update is called once per frame
    void Update()
    {
        //if(anim.GetBool("ExplodeHeart"))
        //{
        //    if (animStarted == false)
        //    {
        //        startTime = Time.time;
        //        animStarted = true;
        //    }

        //    if (Time.time > startTime + deathTime)
        //    {
        //        animStarted = false;
        //        anim.SetBool("ExplodeHeart", false);
        //        Destroy(gameObject);

        //    }
        //}
        if(animFinished)
        {
            Destroy(gameObject);
        }
    }

    public void UpdateAnimClipTimes()
    {
        //Create array of clips in animator controller
        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;
        //Foreach loop, to go through every clip instance in the Clips array
        foreach (AnimationClip clip in clips)
        {
            //Switch case depending on name of clips
            switch (clip.name)
            {
                case "Heart_Beat":
                    beatTime = clip.length;
                    break;
                case "Heart_Explode":
                    deathTime = clip.length;
                    break;
            }
        }
    }
}
