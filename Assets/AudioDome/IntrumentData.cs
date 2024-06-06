using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public enum Instrument
{
    Flute,
    Flute2,
    Vibraphone,
    Viola,
    Bell
}
public class InstrumentData
{
    public float volume;
    public WaveformMix waveform;
    public ADSR adsr;
    public float[,] overtones;

    public InstrumentData(Instrument instrument)
    {
        switch (instrument)
        {
            case Instrument.Flute:
            {
                volume = 1.5f;
                waveform = new WaveformMix(1f,0f,0f);
                adsr = new ADSR(0.1f, 0.1f, 0.5f, 0.3f);
                overtones = new float[,] {{1f,1f},{3f,0.3f},{4f,0.1f}};
                break;
            }
            case Instrument.Flute2:
            {
                volume = 1.5f;
                waveform = new WaveformMix(1f,0f,0f);
                adsr = new ADSR(0.1f, 0.1f, 0.5f, 0.7f);
                overtones = new float[,] {{1f,1f},{3f,0.3f},{4f,0.1f},{5f,0.2f},{6f,0.1f}};
                break;
            }
            case Instrument.Vibraphone:
            {
                volume = 5f;
                waveform = new WaveformMix(1f,0f,0f);
                adsr = new ADSR(0.01f, 0.04f, 0.1f, 1f);
                overtones = new float[,] {{1f,1f}};
                break;
            }
            case Instrument.Viola:
            {
                volume = 1.5f;
                waveform = new WaveformMix(0.1f,0f,0.9f);
                adsr = new ADSR(0.3f, 0.3f, 0.7f, 0.6f);
                overtones = new float[,] {{1f,1f},{2f,0.1f},{3f,0.1f}};
                break;
            }
            case Instrument.Bell:
            {
                volume = 5f;
                waveform = new WaveformMix(1f,0f,0f);
                adsr = new ADSR(0.01f, 0.05f, 0.1f, 2f);
                overtones = new float[,] {{1f,1f},{1.2f,0.2f},{1.5f,0.3f,},{1.6f,0.1f}};
                break;
            }
        }
    }
}

public class ADSR
{
    public float attack, decay, sustain, release;

    public ADSR(float a, float d, float s, float r)
    {
        attack = a;
        decay = d;
        sustain = s;
        release = r;
    }
}

public class WaveformMix
{
    public float sin, square, sawtooth;

    public WaveformMix(float sin, float square, float sawtooth)
    {
        this.sin = sin;
        this.square = square;
        this.sawtooth = sawtooth;
    }
}