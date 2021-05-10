using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D[] cursorTextureArray;
    [SerializeField] private int frameCount;
    [SerializeField] private float frameRate;
    private int currentFrame;
    private float frameTimer;

    [SerializeField] private BeatDetection beatDetection;


    private void Start()
    {
        Cursor.SetCursor(cursorTextureArray[0], new Vector2(16, 16), CursorMode.Auto);
        beatDetection = GameObject.FindGameObjectWithTag("BeatDetector").GetComponent<BeatDetection>();
        frameRate = (60f / beatDetection.bpm) / frameCount;
    }

    private void Update()
    {
        if(beatDetection.SongIsStarted == true)
        {
            frameTimer -= Time.deltaTime;
            if (frameTimer <= 0f)
            {
                frameTimer += frameRate;
                currentFrame = (currentFrame + 1) % frameCount;
                Cursor.SetCursor(cursorTextureArray[currentFrame], new Vector2(16, 16), CursorMode.Auto);
            }
            //if(currentFrame == 29)
            //{
            //    Debug.Log("CORRECT 'On Beat Time' = " + (beatDetection.songPosInBeatsRounded % 1));
            //}
        }
    }

}
