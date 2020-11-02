using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongManager : MonoBehaviour
{
    [SerializeField] private float songPosition;    // Current pos of song (in sec)
    [SerializeField] private float songPosInBeats;  // Current pos of song (in beats)
    [SerializeField] private float secPerBeat;      // Duration of beat
    [SerializeField] private float dsptimesong;     // Time (in sec) passed since start of song

    [SerializeField] private float bpm;             // Beats per min of a song
    [SerializeField] private float[] notes;         // Keep all the position-in-beats of notes in the song
    [SerializeField] private int nextIndex = 0;     // The index of the next note to be spawned

    private int beatsShownInAdvance = 2;
    [SerializeField] GameObject test;
    [SerializeField] private float songPosInBeatsRounded;

    [SerializeField] private Renderer renderer;
    [SerializeField] private Color colourEnd;
    [SerializeField] private Color colourStart;
    [SerializeField] private float rate = 1;
    private float i = 0;

    void Start()
    {
        secPerBeat = 60f / bpm;                     // Calculate how many seconds in one beat
        dsptimesong = (float)AudioSettings.dspTime; // Record time when song starts
        GetComponent<AudioSource>().Play();         // Start the song

        colourEnd = new Color(Random.value, Random.value, Random.value);
        colourStart = colourEnd;
        colourEnd.a = 0;
        renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        songPosition = (float)(AudioSettings.dspTime - dsptimesong); // Calculate the position in seconds
        songPosInBeats = songPosition / secPerBeat;                  // Calculate the position in beats
        if (nextIndex < notes.Length && notes[nextIndex] < songPosInBeats + beatsShownInAdvance)
        {
            Instantiate(test);
            //initialize the fields of the music note
            nextIndex++;
        }

        songPosInBeatsRounded = Mathf.Round(songPosInBeats * 10f) / 10f;
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);

        if (Input.GetMouseButtonDown(0))
        {
            print(songPosInBeatsRounded % 1);
            if (FastApproximately(songPosInBeatsRounded % 1, 0.5f, 0.125f))
            {
                Vector3 worldPos;
                Ray ray = Camera.main.ScreenPointToRay(mousePos);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 1000f))
                {
                    worldPos = hit.point;
                }
                else
                {
                    worldPos = Camera.main.ScreenToWorldPoint(mousePos);
                }
                Instantiate(test, worldPos, Quaternion.identity);
            }
        }

        i += Time.deltaTime * rate;
        renderer.material.color = Color.Lerp(colourStart, colourEnd, Mathf.PingPong(i * 2, 1));
        // If we've got to the current target colour, choose a new one
        if (songPosInBeatsRounded % 1 == 0.5f)
        {
            i = 0;
            colourEnd = new Color(Random.value, Random.value, Random.value);
            colourStart = colourEnd;
            colourEnd.a = 0;
        }
    }

    public static bool FastApproximately(float a, float b, float threshold)
    {
        return ((a - b) < 0 ? ((a - b) * -1) : (a - b)) <= threshold;
    }
}
