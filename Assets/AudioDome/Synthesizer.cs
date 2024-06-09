using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Synthesizer : MonoBehaviour
{
    public List<Note> ActiveNotes = new List<Note>();
    public List<GameObject> ObjNotePrefabs = new List<GameObject>();
    public List<Note> noteComponents = new List<Note>();
    public Note note;

    private float samplingFrequency = 48000.0f;
    public float gain = 0.05f;
    //public int activeNotes = 0;

    public InstrumentData[] instruments = new InstrumentData[Enum.GetNames(typeof(Instrument)).Length];

    void Start()
    {

        note = GetComponent<Note>();
        //Creates references to all predefined intruments in the Instrument enum
        for (int i = 0; i < instruments.Length; i++)
        {
            instruments[i] = new InstrumentData((Instrument)i);
        }



        ObjNotePrefabs.Add(gameObject);

        foreach (GameObject gameObject in ObjNotePrefabs)
        {
           Note noteComponent = gameObject.GetComponent<Note>();
           noteComponents.Add(noteComponent);

            foreach (Note note in noteComponents)
            {
                ActiveNotes.Add(note);
            }
        }



}
    public Note PlayNote(Instrument instrument, float frequency)
    {
        //note Note(instruments[(int)instrument] , frequency);
        //List<Note>ActiveNotes(instruments[(int)instrument] , frequency);

        //ActiveNotes.AddLast(note);
        //activeNotes = 1;
        return note;
    }
    public Note PlayNote(InstrumentData instrument, float frequency)
    {
        Note note = new Note(instrument, frequency);
        //ActiveNotes.AddLast(note);
        //activeNotes = 1;
        return note;
    }
    public Note PlayNote(Instrument instrument, float frequency, float holdTime, bool fade)
    {
        Note note = new Note(instruments[(int)instrument] , frequency, holdTime, fade);
        //ActiveNotes.AddLast(note);
        //activeNotes = 1;
        return note;
    }
    public Note PlayNote(InstrumentData instrument, float frequency, float holdTime, bool fade)
    {
        Note note = new Note(instrument, frequency, holdTime, fade);
        //ActiveNotes.AddLast(note);
        //activeNotes = 1;
        return note;
    }

    public void ReleaseNote(Note note)
    {
        if (note.holdTime < 0f)
        {
            note.Release();
        }
    }

    void OnAudioFilterRead(float[] data, int channels)
    {

        int j = 0;
        while (j < ActiveNotes.Count)
        {
            Note note = ActiveNotes.ElementAt(j);

            float volumeModifier = CalculateADSR(note);
            if (volumeModifier < 0f) continue;

            volumeModifier *= CalculateOscillation(note);

            float volPrevFrame = note.volPrevFrame;
            float volThisFrame = volumeModifier;

            float baseFrequency = note.frequency * note.frequencyModifier;
            float baseVolume = note.getInstrumentVolume();
            WaveformMix waveform = note.getWaveform();
            float[,] overtones = note.getOvertones();

            int currentDataStep = 0;
            for (int i = 0; i < data.Length; i++)
            {
                volumeModifier = Mathf.Lerp(volPrevFrame, volThisFrame, (float)i/data.Length);

                data[i] += gain * baseVolume * volumeModifier * ReturnOvertoneSeries(baseFrequency, overtones, waveform, currentDataStep, note.startTime);

                currentDataStep++;
                if (channels == 2)
                {
                    data[i+1] = data[i];
                    i++;
                }
            }

            note.volPrevFrame = volThisFrame;
            j++;
        }
    }

    private float CalculateADSR(Note note)
    {
        ADSR adsr = note.getADSR();
        float timePlaying = (float)(AudioSettings.dspTime - note.startTime);

        float volumeModifier = adsr.sustain;

        if (timePlaying <= adsr.attack)
        {
            volumeModifier = Mathf.InverseLerp(0.0f, adsr.attack, timePlaying);
        }
        else if (timePlaying < adsr.attack + adsr.decay)
        {
            volumeModifier = Mathf.InverseLerp(adsr.attack, adsr.attack+adsr.decay, timePlaying);
            volumeModifier = Mathf.Lerp(1.0f, adsr.sustain, volumeModifier);
        }

        if (note.holdTime > 0f && note.beingHeld)
        {
            if (note.fade) volumeModifier *= 1 - timePlaying/note.holdTime;
            if (timePlaying > note.holdTime)
            {
                note.Release();
                Destroy(gameObject);
            }
        }

        if (!note.beingHeld)
        {
            timePlaying = (float)(AudioSettings.dspTime - note.releaseTime);
            if (timePlaying > adsr.release)
            {
                ActiveNotes.Remove(note);
                //activeNotes = 0;
                return -1f;
            }

            volumeModifier = Mathf.InverseLerp(0.0f, adsr.release, timePlaying);
            volumeModifier = Mathf.Lerp(note.volBeforeRelease, 0.0f, volumeModifier);
        }
        else
        {
            note.setVolBeforeRelease(volumeModifier);
        }
        
        return volumeModifier;
    }

    private float CalculateOscillation(Note note)
    {
        float volumeModifier = 1f;

        float timePlaying = (float)(AudioSettings.dspTime - note.startTime);
        float oscillation = SinWave(timePlaying * note.oscillationFreq)*0.5f + 0.5f;

        oscillation *= note.oscillationMag;
        volumeModifier -= oscillation;

        return volumeModifier;
    }

    private float ReturnOvertoneSeries(float baseFrequency, float[,] overtones, WaveformMix waveform, int dataIndex, double audioTime)
    {
        float result = 0f;

        for (int i = 0; i < overtones.GetLength(0); i++)
        {
            float overtoneFrequency = baseFrequency*overtones[i,0];
            float overtoneWeight = overtones[i,1];

            float timeAtBeginning = (float)((AudioSettings.dspTime - audioTime) % (1.0 / (double)overtoneFrequency));
            float exactTime = timeAtBeginning + dataIndex / samplingFrequency;

            float input = exactTime*overtoneFrequency*2f*Mathf.PI;
            if (waveform.sin * overtoneWeight > 0.03f) result += WaveFunction(input, Waveform.Sin) * waveform.sin * overtoneWeight;
            if (waveform.square * overtoneWeight > 0.03f) result += WaveFunction(input, Waveform.Square) * waveform.square * overtoneWeight;
            if (waveform.sawtooth *overtoneWeight > 0.03f) result += WaveFunction(input, Waveform.Sawtooth) * waveform.sawtooth * overtoneWeight;
        }

        return result;
    }

    enum Waveform
    {
        Sin,
        Square,
        Sawtooth
    }
    private float WaveFunction(float input, Waveform wave)
    {
        switch (wave)
        {
            case Waveform.Sin:
            {
                return SinWave(input);
            }
            case Waveform.Square:
            {
                return SquareWave(input);
            }
            case Waveform.Sawtooth:
            {
                return SawtoothWave(input);
            }
            default:
            {
                return 0f;
            }
        }
    }
    private float SinWave(float input)
    {
        return Mathf.Sin(input);
    }
    private float SquareWave(float input)
    {
        return Mathf.Sign(Mathf.Sin(input));
    }
    private float SawtoothWave(float input)
    {
        return Mathf.Atan(Mathf.Tan(input/(2f))/2)/Mathf.PI;
    }

}
