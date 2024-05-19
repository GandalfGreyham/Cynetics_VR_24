using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Timeline;

public enum Instrument
{
    Vibrophone,
    SquareSynth,
    Strings,
    Flute,
    Organ,
    TriangleTest
}

public class InstrumentData
{
    public Waveform waveform;
    public WaveformMix waveformMix;
    public ADSR adsr;
    public float[] harmonicStrengths;
    public float volumeMultiplier;

    public InstrumentData(Instrument instrument)
    {
        switch (instrument)
        {
            case Instrument.Vibrophone:
            {
                waveform = Waveform.Sin;
                waveformMix = new WaveformMix(1f,0f,0f);
                adsr = new ADSR();
                harmonicStrengths = new float[] {1.0f};
                volumeMultiplier = 2f;
                break;
            }
            case Instrument.SquareSynth:
            {
                waveform = Waveform.Square;
                waveformMix = new WaveformMix(0.2f,0.8f,0f);
                adsr = new ADSR();
                harmonicStrengths = new float[] {1.0f};
                volumeMultiplier = 0.9f;
                break;
            }
            case Instrument.Strings:
            {
                waveform = Waveform.Sawtooth;
                waveformMix = new WaveformMix(0.5f,0f,0.5f);
                adsr = new ADSR(0.2f,0.2f,0.6f,0.5f);
                harmonicStrengths = new float[] {1.0f,0.2f,0.3f};
                volumeMultiplier = 1;
                break;
            }
            case Instrument.TriangleTest:
            {
                waveform = Waveform.Triangle;
                waveformMix = new WaveformMix(1f,0f,0f);
                adsr = new ADSR();
                harmonicStrengths = new float[] {1.0f,0.3f,0.05f};
                volumeMultiplier = 1;
                break;
            }
            case Instrument.Flute:
            {
                waveformMix = new WaveformMix(1.2f,0f,0f);
                adsr = new ADSR(0.25f,0.2f,0.7f,0.5f);
                harmonicStrengths = new float[] {1.0f,0.1f,0.2f,0.1f};
                volumeMultiplier = 1.2f;
                break;
            }
            case Instrument.Organ:
            {
                waveformMix = new WaveformMix(0.9f,0f,0.1f);
                adsr = new ADSR();
                harmonicStrengths = new float[] {1.0f,0.2f,0.1f,0.3f};
                volumeMultiplier = 1;
                break;
            }
        }
    }
}

public enum Waveform
{
    Sin,
    Square,
    Triangle,
    Sawtooth
}

public class WaveformMix
{
    public float sin;
    public float square;
    public float sawtooth;

    public WaveformMix(float sin, float square, float sawtooth)
    {
        this.sin = sin;
        this.square = square;
        this.sawtooth = sawtooth;
    }
}

[System.Serializable]
public class ADSR
{
    public float attack, decay, sustain, release;
    public ADSR()
    {
        attack = 0.1f;
        decay = 0.1f;
        sustain = 0.5f;
        release = 0.3f;
    }
    public ADSR(float a, float d, float s, float r)
    {
        attack = a;
        decay = d;
        sustain = s;
        release = r;
    }
}





public class Note
{
    private InstrumentData instrument;
    private float frequency;
    private double startPlayTime;
    private double releaseTime;
    private bool beingHeld;
    private float volBeforeRelease = 0;
        
    public Note(InstrumentData instrument, float frequency)
    {
        this.instrument = instrument;
        this.frequency = frequency;
        startPlayTime = AudioSettings.dspTime;
        releaseTime = -1;
        beingHeld = true;
    }

    public void Release()
    {
        releaseTime = AudioSettings.dspTime;
        beingHeld = false;
    }

    public void setVolumeBeforeRelease(float currentVol)
    {
        volBeforeRelease = currentVol;
    }
    public float getVolumeBeforeRelease()
    {
        return volBeforeRelease;
    }

    public Waveform getWaveform()
    {
        return instrument.waveform;
    }
    public WaveformMix getWaveformMix()
    {
        return instrument.waveformMix;
    }
    public ADSR getADSR()
    {
        return instrument.adsr;
    }
    public float[] getHarmonics()
    {
        return instrument.harmonicStrengths;
    }
    public float getVolumeMultiplier()
    {
        return instrument.volumeMultiplier;
    }

    public float getFrequency()
    {
        return frequency;
    }
    public double getStartTime()
    {
        return startPlayTime;
    }
    public double getReleaseTime()
    {
        return releaseTime;
    }
    public bool isBeingHeld()
    {
        return beingHeld;
    }

    public void SetFrequency(float freq)
    {
        frequency = freq;
    }
}





public class Oscillator2 : MonoBehaviour
{

    public LinkedList<Note> ActiveNotes = new LinkedList<Note>();


    private float samplingFrequency = 48000.0f;
    public float gain = 0.05f;

    public Instrument KeyboardInstrument;

    private float currentFrequency;
    private Waveform currentWaveform;
    private float currentWeight;
    public ADSR adsr;
    private float[] currentHarmonics;
    private float currentVolumeMultiplier;
    
    public bool KeyboardOn = false;
    public float[] scale = new float[8];
    public Note[] keyboard = new Note[8];

    void Start()
    {
        float baseFrequency = 440;
        scale[0] = baseFrequency;
        scale[1] = baseFrequency * 1.125f;
        scale[2] = baseFrequency * 1.25f;
        scale[3] = baseFrequency * 1.333f;
        scale[4] = baseFrequency * 1.5f;
        scale[5] = baseFrequency * 1.666f;
        scale[6] = baseFrequency * 1.875f;
        scale[7] = baseFrequency * 2f;
    }

    void Update()
    {
        if (KeyboardOn)
        {
            KeyboardCheck(KeyCode.Q, 0);
            KeyboardCheck(KeyCode.W, 1);
            KeyboardCheck(KeyCode.E, 2);
            KeyboardCheck(KeyCode.R, 3);
            KeyboardCheck(KeyCode.T, 4);
            KeyboardCheck(KeyCode.Y, 5);
            KeyboardCheck(KeyCode.U, 6);
            KeyboardCheck(KeyCode.I, 7);
        }
    }

    public void KeyboardCheck(KeyCode key, int index)
    {
        if(Input.GetKeyDown(key))
        {
            keyboard[index] = PlayNote(KeyboardInstrument, scale[index]);
        }
        if (Input.GetKeyUp(key))
        {
            ReleaseNote(keyboard[index]);
        }
    }

    public float[,] NoteMatrix(float baseFrequency, int octaves)
    {
        float[,] matrix = new float[octaves,8];
        for (int i = 0; i < octaves; i++)
        {
            matrix[i,0] = baseFrequency          * Mathf.Pow(2,i);
            matrix[i,1] = baseFrequency * 1.125f * Mathf.Pow(2,i);
            matrix[i,2] = baseFrequency * 1.25f  * Mathf.Pow(2,i);
            matrix[i,3] = baseFrequency * 1.333f * Mathf.Pow(2,i);
            matrix[i,4] = baseFrequency * 1.5f   * Mathf.Pow(2,i);
            matrix[i,5] = baseFrequency * 1.666f * Mathf.Pow(2,i);
            matrix[i,6] = baseFrequency * 1.875f * Mathf.Pow(2,i);
            matrix[i,7] = baseFrequency * 2f     * Mathf.Pow(2,i);
        }
        return matrix;
    }

    //Plays a note of a given frequency
    public Note PlayNote(Instrument instrument, float frequency)
    {
        Note note = new Note(new InstrumentData(instrument) , frequency);
        ActiveNotes.AddLast(note);
        return note;
    }

    //Releases a note that is currently playing
    public void ReleaseNote(Note note)
    {
        note.Release();
    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        int j = 0;
        while (j < ActiveNotes.Count)
        {
            Note note = ActiveNotes.ElementAt(j);

            //ADSR
            adsr = note.getADSR();

            float timePlaying = (float)(AudioSettings.dspTime - note.getStartTime());

            float volumeModifier = adsr.sustain;

            if (timePlaying <= adsr.attack)
            {
                volumeModifier = Mathf.InverseLerp(0.0f, adsr.attack, timePlaying);
            }
            else if (timePlaying < adsr.decay + adsr.attack)
            {
                volumeModifier = Mathf.InverseLerp(adsr.attack, adsr.attack+adsr.decay, timePlaying);
                volumeModifier = Mathf.Lerp(1.0f, adsr.sustain, volumeModifier);
            }

            if (!note.isBeingHeld())
            {
                timePlaying = (float)(AudioSettings.dspTime - note.getReleaseTime());
                if (timePlaying > adsr.release)
                {
                    ActiveNotes.Remove(note);
                    continue;
                }

                volumeModifier = Mathf.InverseLerp(0.0f, adsr.release, timePlaying);
                //volumeModifier = Mathf.Lerp(adsr.sustain, 0.0f, volumeModifier);
                volumeModifier = Mathf.Lerp(note.getVolumeBeforeRelease(), 0.0f, volumeModifier);
            }
            else
            {
                note.setVolumeBeforeRelease(volumeModifier);
            }

            //Fill Data
            currentFrequency = note.getFrequency();
            currentWaveform = note.getWaveform();
            currentHarmonics = note.getHarmonics();
            currentVolumeMultiplier = note.getVolumeMultiplier();
            
            int currentDataStep = 0;
            for (int i = 0; i < data.Length; i++)
            {
                //Sin
                currentWaveform = Waveform.Sin;
                currentWeight = note.getWaveformMix().sin;
                if (currentWeight > 0f) data[i] += ReturnHarmonicSeries(currentDataStep, note.getStartTime()) * gain * volumeModifier;
                //Square
                currentWaveform = Waveform.Square;
                currentWeight = note.getWaveformMix().square;
                if (currentWeight > 0f) data[i] += ReturnHarmonicSeries(currentDataStep, note.getStartTime()) * gain * volumeModifier;
                //Sawtooth
                currentWaveform = Waveform.Sawtooth;
                currentWeight = note.getWaveformMix().sawtooth;
                if (currentWeight > 0f) data[i] += ReturnHarmonicSeries(currentDataStep, note.getStartTime()) * gain * volumeModifier;

                currentDataStep++;
                if (channels == 2)
                {
                    data[i+1] = data[i];
                    i++;
                }
            }

            j++;
        }
    }

    public float ReturnHarmonicSeries(int dataIndex, double audioTime)
    {
        float result = 0.0f;

        for (int i = 1; i <= currentHarmonics.Length; i++)
        {
            float harmonicFrequency = currentFrequency*i;

            float timeAtBeginning = (float)((AudioSettings.dspTime - audioTime) % (1.0 / (double)harmonicFrequency));
            float exactTime = timeAtBeginning + dataIndex / samplingFrequency;

            result += WaveFunction(exactTime * harmonicFrequency * 2f * Mathf.PI, currentWaveform, currentWeight) * currentHarmonics[i-1] * currentVolumeMultiplier;
        }

        return result;
    }

    public float WaveFunction(float input, Waveform wave, float weight)
    {
        switch (wave)
        {
            case Waveform.Sin:
            {
                return SinWave(input) * weight;
            }
            case Waveform.Square:
            {
                return SquareWave(input) * weight;
            }
            case Waveform.Triangle:
            {
                return TriangleWave(input) * weight;
            }
            case Waveform.Sawtooth:
            {
                return SawtoothWave(input/2) * weight;
            }
            default:
            {
                return 0.0f;
            }
        }
    }

    public float SinWave(float input)
    {
        return Mathf.Sin(input);
    }
    public float SquareWave(float input)
    {
        if (Mathf.Sin(input) >= 0) {
            return 0.6f;
        }
        else {
            return -0.6f;
        }
    }
    public float TriangleWave(float input)
    {
        //return Mathf.PingPong(input, 1.0f);
        return 1f - 4f*(float)Math.Abs(Math.Round(input/5-0.25f)-(input/5-0.25f));
    }
    public float SawtoothWave(float input)
    {
        return Mathf.Atan(Mathf.Tan(input)/2)/2;
    }
}
