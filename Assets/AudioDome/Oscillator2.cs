using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Timeline;

public enum Instrument
{
    Basic,
    SquareWave,
    Strings_WIP
}

public class InstrumentData
{
    public Waveform waveform;
    public ADSR adsr;
    public float[] harmonicStrengths;

    public InstrumentData(Instrument instrument)
    {
        switch (instrument)
        {
            case Instrument.Basic:
            {
                waveform = Waveform.Sin;
                adsr = new ADSR();
                harmonicStrengths = new float[] {1.0f,0.2f,0.1f,0.05f,0.02f,0.01f,0.005f,0.002f,0.001f};
                break;
            }
            case Instrument.SquareWave:
            {
                waveform = Waveform.Square;
                adsr = new ADSR();
                harmonicStrengths = new float[] {1.0f,0.3f,0.05f};
                break;
            }
            case Instrument.Strings_WIP:
            {
                waveform = Waveform.Sawtooth;
                adsr = new ADSR(0.2f,0.2f,0.6f,0.5f);
                harmonicStrengths = new float[] {1.0f,0.3f,0.2f,0.1f,0.05f,0.03f,0.01f,0.005f,0.003f,0.002f,0.002f};
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

    public Waveform getWaveform()
    {
        return instrument.waveform;
    }
    public ADSR getADSR()
    {
        return instrument.adsr;
    }
    public float[] getHarmonics()
    {
        return instrument.harmonicStrengths;
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
    public float gain = 0.1f;

    public Instrument KeyboardInstrument;

    private float currentFrequency;
    private Waveform currentWaveform;
    public ADSR adsr;
    private float[] currentHarmonics;
    
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
                volumeModifier = Mathf.Lerp(adsr.sustain, 0.0f, volumeModifier);
            }

            currentFrequency = note.getFrequency();
            currentWaveform = note.getWaveform();
            currentHarmonics = note.getHarmonics();
            
            int currentDataStep = 0;
            for (int i = 0; i < data.Length; i++)
            {
                data[i] += ReturnHarmonicSeries(currentDataStep, note.getStartTime()) * gain * volumeModifier;
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

            result += WaveFunction(exactTime * harmonicFrequency * 2f * Mathf.PI, currentWaveform) * currentHarmonics[i-1];
        }

        return result;
    }

    public float WaveFunction(float input, Waveform wave)
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
            case Waveform.Triangle:
            {
                return TriangleWave(input);
            }
            case Waveform.Sawtooth:
            {
                return SawtoothWave(input/2);
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
        return Mathf.PingPong(input, 1.0f);
    }
    public float SawtoothWave(float input)
    {
        return Mathf.Atan(Mathf.Tan(input)/2)/2;
    }
}
