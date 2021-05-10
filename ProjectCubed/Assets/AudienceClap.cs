using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudienceClap : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private Sprite[] frames;
    [SerializeField] private Image CurrentAudienceSprite;
    [SerializeField] private int frameCount;
    [SerializeField] private float frameRate;
    private int currentFrame;
    private float frameTimer;

    [SerializeField] private BeatDetection beatDetection;

    void Start()
    {
        beatDetection = GameObject.FindGameObjectWithTag("BeatDetector").GetComponent<BeatDetection>();
        frameRate = (60f / beatDetection.bpm) / frameCount;
    }

    // Update is called once per frame
    void Update()
    {
        if (beatDetection.SongIsStarted == true)
        {
            frameTimer -= Time.deltaTime;
            if (frameTimer <= 0f)
            {
                frameTimer += frameRate;
                currentFrame = (currentFrame + 1) % frameCount;
                CurrentAudienceSprite.sprite = (frames[currentFrame]);
            }
        }
    }
}
