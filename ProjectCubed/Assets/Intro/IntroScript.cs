using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class IntroScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private VideoClip[] videoClips;
    [SerializeField] private VideoPlayer videoPlayer;
    private int currentClip = 0;

    void Start()
    {
        currentClip = 0;
        videoPlayer.clip = videoClips[currentClip];
    }

    void FixedUpdate()
    {
        if(currentClip > 3)
        {
            //print("load scene");
            SceneManager.LoadScene("Planet_SwampStart"); // <-- Just add the scenename that the intro leads into
        }
        else
        {
            if (Mouse.current.leftButton.isPressed && videoPlayer.isPaused == true) // Could maybe add an indicator on screen when the player can click.
            {          
                print("mouse clicked");
                currentClip++;
                print("new currentClip int = " + currentClip);
                if (currentClip != 4)
                {
                    videoPlayer.clip = videoClips[currentClip];
                    videoPlayer.Play();
                }



            }
        }
    }
}
