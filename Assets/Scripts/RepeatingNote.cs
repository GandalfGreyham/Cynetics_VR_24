using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatingNote : MonoBehaviour
{
    public Synthesizer synthesizer;

    public Instrument instrument;
    public float frequency;

    public float noteLength;
    public float repeatTime;
    public bool fadeNotes;

    private float startTime;
    public Note currentNote;

    // Start is called before the first frame update
    void Start()
    {
        startTime = (float)AudioSettings.dspTime;
    }

    // Update is called once per frame
    void Update()
    {
        float timePlaying = (float)AudioSettings.dspTime - startTime;
        if (timePlaying > repeatTime)
        {
            startTime = (float)AudioSettings.dspTime;
            currentNote = synthesizer.PlayNote(instrument, frequency, noteLength, fadeNotes);
        }
    }
}
