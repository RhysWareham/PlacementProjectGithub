using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatDetection : MonoBehaviour
{

    private bool shotIsOnBeat;

    [SerializeField] private float songPosition;    // Current pos of song (in sec)
    [SerializeField] private float songPosInBeats;  // Current pos of song (in beats)
    [SerializeField] private float songPosInBeatsRounded; // Rounded to 2dp
    [SerializeField] private float secPerBeat;      // Duration of beat
    [SerializeField] private float dsptimesong;     // Time (in sec) passed since start of song

    [SerializeField] private float bpm;             // Beats per min of a song
    [SerializeField] private float onBeatTime = 0.5f;      // Time of correct beat - 0.5f for metronome
    [SerializeField] private float beatThreshold = 0.125f;   // Time leeway - 0.125 default
    [SerializeField] private float[] notes;         // Keep all the position-in-beats of notes in the song
    [SerializeField] private int nextIndex = 0;     // The index of the next note to be spawned

    private int beatsShownInAdvance = 2;

    // Start is called before the first frame update
    void Start()
    {
        secPerBeat = 60f / bpm;                     // Calculate how many seconds in one beat
        dsptimesong = (float)AudioSettings.dspTime; // Record time when song starts
        GetComponent<AudioSource>().Play();         // Start the song
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

        songPosInBeatsRounded = Mathf.Round(songPosInBeats * 10f) / 10f;

    }

    public bool CheckIfOnBeat()
    {
        print(songPosInBeatsRounded % 1);
        if (FastApproximately(songPosInBeatsRounded % 1, onBeatTime, beatThreshold))
        {
            shotIsOnBeat = true;
        }
        else
        {
            shotIsOnBeat = false;
        }

        return shotIsOnBeat;
    }

    public static bool FastApproximately(float a, float b, float threshold)
    {
        return ((a - b) < 0 ? ((a - b) * -1) : (a - b)) <= threshold;
    }
}
