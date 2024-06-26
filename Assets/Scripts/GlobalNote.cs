using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class GlobalNote
{
    public InstrumentData instrument;
    public float frequency;
    public float frequencyModifier = 1f;

    public double startTime;
    public double releaseTime;
    public bool beingHeld;

    public float volPrevFrame = 0f;
    public float volBeforeRelease = 0f;

    public float oscillationFreq = 1f;
    public float oscillationMag = 0f;

    public float holdTime;
    public bool fade;

    public GlobalNote(InstrumentData instrument, float frequency)
    {
        this.instrument = instrument;
        this.frequency = frequency;
        startTime = AudioSettings.dspTime;
        releaseTime = -1;
        beingHeld = true;

        holdTime = -1;
        fade = false;
    }
    public GlobalNote(InstrumentData instrument, float frequency, float holdTime, bool fade)
    {
        this.instrument = instrument;
        this.frequency = frequency;
        startTime = AudioSettings.dspTime;
        releaseTime = -1;
        beingHeld = true;

        this.holdTime = holdTime;
        this.fade = fade;
    }

    public void Release()
    {
        releaseTime = AudioSettings.dspTime;
        beingHeld = false;
    }

    public void ModifyOscillation(float freq, float mag)
    {
        oscillationFreq = freq;
        oscillationMag = mag;
    }

    public float getInstrumentVolume()
    {
        return instrument.volume;
    }
    public WaveformMix getWaveform()
    {
        return instrument.waveform;
    }
    public ADSR getADSR()
    {
        return instrument.adsr;
    }
    public float[,] getOvertones()
    {
        return instrument.overtones;
    }

    public void setVolBeforeRelease(float releaseVol)
    {
        volBeforeRelease = Mathf.Min(releaseVol, getADSR().sustain);
    }
}