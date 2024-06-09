using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotePrefab : MonoBehaviour
{
    public Note note;
    //public MusicStick musicStick;
    public Synthesizer synthesizer;
    public bool instrumentReady;
    public float frequencyPrefab;
    public Instrument instrumentPrefab;

    public float effectFromMusicStick_01;
    public float effectFromMusicStick_02;
    public float effectFromMusicStick_03;


    //public Note[,] StoredInstrument;
    //public int octaveIndexNotePrefab;
    //public int intervalIndexPrefab;
    //public string storageNoteListRef;




    //frequencyModifier = 1f + (wammyEffect* wammyIntensity);
    //currentNote.ModifyOscillation(wammySpeed, wammyEffect);

    // Start is called before the first frame update
    void Start()
    {
        synthesizer = GetComponent<Synthesizer>();
        note = GetComponent<Note>();
    }
    
    // Update is called once per frame
    void Update()
    {
        note.frequencyModifier = effectFromMusicStick_01; //1f + (musicStick.wammyEffect * musicStick.wammyIntensity);
        note.ModifyOscillation(effectFromMusicStick_02, effectFromMusicStick_03);//(musicStick.wammySpeed , musicStick.wammyEffect);

        

    }

    //public void StoredInstrument(Instrument instrument, float frequency)

    public Note StoreInstrument(Instrument instrument, float frequency)
    {
        Debug.Log(instrument + " , " + frequency);
        instrumentPrefab = instrument;
        frequencyPrefab = frequency;
        //return note;
        //instrumentReady = true;
        PlayNewNote();
        return note;
        //note.Note(InstrumentData instrument, float frequency) = (instrument, frequencies[octaveIndex, intervalIndex])

    //note(Instrument instrument, float frequency) = storedInstrument(Instrument instrument, float frequency);
    }

    public void PlayNewNote()
    {
        synthesizer.PlayNote(instrumentPrefab, frequencyPrefab);
    }
}
