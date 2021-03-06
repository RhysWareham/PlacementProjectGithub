﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeatDetection : MonoBehaviour
{

    private bool shotIsOnBeat;

    [SerializeField] private float songPosition;    // Current pos of song (in sec)
    [SerializeField] private float songPosInBeats;  // Current pos of song (in beats)
    [SerializeField] public float songPosInBeatsRounded; // Rounded to 2dp
    [SerializeField] private float secPerBeat;      // Duration of beat
    [SerializeField] private float dsptimesong;     // Time (in sec) passed since start of song

    [SerializeField] public float bpm;             // Beats per min of a song
    [SerializeField] private float onBeatTime = 0.5f;      // Time of correct beat - 0.5f for metronome
    [SerializeField] private float beatThreshold = 0.125f;   // Time leeway - 0.125 default
    [SerializeField] private float[] notes;         // Keep all the position-in-beats of notes in the song
    [SerializeField] private int nextIndex = 0;     // The index of the next note to be spawned

    public bool SongIsStarted = false;


    private int beatsShownInAdvance = 2;

    //[SerializeField] private Slider ComboSlider;
    [SerializeField] private GameObject Audience;
    [SerializeField] private GameObject AudienceHolder;
    [SerializeField] private PlayerData playerData;
    private Vector3 AudienceStartSpawnVector = new Vector3(-84, 0, 0);
    private Vector3 AudienceSpawnVectorIncrement = new Vector3(174, 0, 0);
    private GameObject[] AudienceArray;
    private float ShotCombo = 0;

    // Start is called before the first frame update
    void Start()
    {
        //ComboSlider = GameObject.FindGameObjectWithTag("ComboSlider").GetComponent<Slider>();
        AudienceHolder = GameObject.FindGameObjectWithTag("AudienceHolder");
        secPerBeat = 60f / bpm;                     // Calculate how many seconds in one beat
        dsptimesong = (float)AudioSettings.dspTime; // Record time when song starts
        GetComponent<AudioSource>().Play();         // Start the song
        SongIsStarted = true;

        AudienceArray = GameObject.FindGameObjectsWithTag("Audience");
        playerData.movementVel = 1.5f;
        RemoveAudience();

    }

    void AddAudience()
    {
        int IntShotCombo = Mathf.RoundToInt(ShotCombo);
        AudienceArray[IntShotCombo - 1].GetComponent<Image>().enabled = true;
        //Instantiate(Audience, AudienceStartSpawnVector + (AudienceSpawnVectorIncrement * ShotCombo) , Quaternion.identity, AudienceHolder.transform);
    }

    void RemoveAudience()
    {
        foreach(GameObject Audience in AudienceArray)
            Audience.GetComponent<Image>().enabled = false;

        //GameObject[] CurrentAudience = GameObject.FindGameObjectsWithTag("Audience");
        //foreach (GameObject Audience in CurrentAudience)          
        //    GameObject.Destroy(Audience);
    }

    void ComboBonus()
    {
        Debug.Log("COMBO METER 10x COMBO");
        // DO SOMETHING HERE, EITHER: +MovementSpeed || Big Bullet / Cool bullet with right click || More damage hit || Something upgradable.
        playerData.movementVel += 0.1f;
        Debug.Log("New Movement Velocity = " + playerData.movementVel);
    }

    void EndCombo()
    {
        Debug.Log("SHOT COMBO RESET \n : " + ShotCombo);
        playerData.movementVel = 1.5f;
        Debug.Log("Reset Movement Velocity To: " + playerData.movementVel);
        ShotCombo = 0;
        shotIsOnBeat = false;
        RemoveAudience();
    }

    // Update is called once per frame
    void Update()
    {
        songPosition = (float)(AudioSettings.dspTime - dsptimesong); // Calculate the position in seconds
        songPosInBeats = songPosition / secPerBeat;                  // Calculate the position in beats
        if (nextIndex < notes.Length && notes[nextIndex] < songPosInBeats + beatsShownInAdvance)
        {
            //initialize the fields of the music note
            nextIndex++;
        }

        //Debug.Log(songPosInBeats);

        songPosInBeatsRounded = Mathf.Round(songPosInBeats * 10f) / 10f;
        //ComboSlider.value = ShotCombo;
    }

    public bool CheckIfOnBeat()
    {
        print(songPosInBeatsRounded % 1);
        if (FastApproximately(songPosInBeatsRounded % 1, onBeatTime, beatThreshold))
        {
            ShotCombo += 0.5f;
            print("ShotCombo: " + ShotCombo);
            if(ShotCombo % 1 == 0)
            {
                AddAudience();
            }
            if (ShotCombo % 10 == 0)
            {
                ComboBonus();
            }
            if(ShotCombo >= 20)
            {
                ShotCombo = 0;
                RemoveAudience();
            }
            shotIsOnBeat = true;
        }
        else
        {
            EndCombo();
        }

        return shotIsOnBeat;
    }

    public static bool FastApproximately(float a, float b, float threshold)
    {
        return ((a - b) < 0 ? ((a - b) * -1) : (a - b)) <= threshold;
    }
}
