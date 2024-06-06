using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note
{
    public InstrumentData instrument;
    public float frequency;

    public double startTime;
    public double releaseTime;
    public bool beingHeld;

    public float volPrevFrame = 0f;
    public float volBeforeRelease = 0f;

    public Note(InstrumentData instrument, float frequency)
    {
        this.instrument = instrument;
        this.frequency = frequency;
        startTime = AudioSettings.dspTime;
        releaseTime = -1;
        beingHeld = true;
    }

    public void Release()
    {
        releaseTime = AudioSettings.dspTime;
        beingHeld = false;
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
