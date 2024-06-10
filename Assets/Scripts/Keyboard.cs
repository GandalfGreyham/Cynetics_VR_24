using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keyboard : MonoBehaviour
{
    public Synthesizer synth;

    public Instrument instrument1;
    public Instrument instrument2;
    public Instrument instrument3;

    public float[] scale1 = new float[8];
    public float[] scale2 = new float[8];
    public float[] scale3 = new float[8];
    public Note[] notes1 = new Note[8];
    public Note[] notes2 = new Note[8];
    public Note[] notes3 = new Note[8];

    // Start is called before the first frame update
    void Start()
    {
        float baseFrequency = 880;
        scale1[0] = baseFrequency;
        scale1[1] = baseFrequency * 1.125f;
        scale1[2] = baseFrequency * 1.25f;
        scale1[3] = baseFrequency * 1.333f;
        scale1[4] = baseFrequency * 1.5f;
        scale1[5] = baseFrequency * 1.666f;
        scale1[6] = baseFrequency * 1.875f;
        scale1[7] = baseFrequency * 2f;

        baseFrequency = 440;
        scale2[0] = baseFrequency;
        scale2[1] = baseFrequency * 1.125f;
        scale2[2] = baseFrequency * 1.25f;
        scale2[3] = baseFrequency * 1.333f;
        scale2[4] = baseFrequency * 1.5f;
        scale2[5] = baseFrequency * 1.666f;
        scale2[6] = baseFrequency * 1.875f;
        scale2[7] = baseFrequency * 2f;

        baseFrequency = 220;
        scale3[0] = baseFrequency;
        scale3[1] = baseFrequency * 1.125f;
        scale3[2] = baseFrequency * 1.25f;
        scale3[3] = baseFrequency * 1.333f;
        scale3[4] = baseFrequency * 1.5f;
        scale3[5] = baseFrequency * 1.666f;
        scale3[6] = baseFrequency * 1.875f;
        scale3[7] = baseFrequency * 2f;
    }

    // Update is called once per frame
    void Update()
    {
        KeyboardCheck(KeyCode.Q, instrument1, 0, scale1, notes1);
        KeyboardCheck(KeyCode.W, instrument1, 1, scale1, notes1);
        KeyboardCheck(KeyCode.E, instrument1, 2, scale1, notes1);
        KeyboardCheck(KeyCode.R, instrument1, 3, scale1, notes1);
        KeyboardCheck(KeyCode.T, instrument1, 4, scale1, notes1);
        KeyboardCheck(KeyCode.Y, instrument1, 5, scale1, notes1);
        KeyboardCheck(KeyCode.U, instrument1, 6, scale1, notes1);
        KeyboardCheck(KeyCode.I, instrument1, 7, scale1, notes1);

        KeyboardCheck(KeyCode.A, instrument2, 0, scale2, notes2);
        KeyboardCheck(KeyCode.S, instrument2, 1, scale2, notes2);
        KeyboardCheck(KeyCode.D, instrument2, 2, scale2, notes2);
        KeyboardCheck(KeyCode.F, instrument2, 3, scale2, notes2);
        KeyboardCheck(KeyCode.G, instrument2, 4, scale2, notes2);
        KeyboardCheck(KeyCode.H, instrument2, 5, scale2, notes2);
        KeyboardCheck(KeyCode.J, instrument2, 6, scale2, notes2);
        KeyboardCheck(KeyCode.K, instrument2, 7, scale2, notes2);

        KeyboardCheck(KeyCode.Z, instrument3, 0, scale3, notes3);
        KeyboardCheck(KeyCode.X, instrument3, 1, scale3, notes3);
        KeyboardCheck(KeyCode.C, instrument3, 2, scale3, notes3);
        KeyboardCheck(KeyCode.V, instrument3, 3, scale3, notes3);
        KeyboardCheck(KeyCode.B, instrument3, 4, scale3, notes3);
        KeyboardCheck(KeyCode.N, instrument3, 5, scale3, notes3);
        KeyboardCheck(KeyCode.M, instrument3, 6, scale3, notes3);
        KeyboardCheck(KeyCode.Comma, instrument3, 7, scale3, notes3);
    }

    public void KeyboardCheck(KeyCode key, Instrument instrument, int index, float[] scale, Note[] notes)
    {
        if(Input.GetKeyDown(key))
        {
            notes[index] = synth.PlayNote(instrument, scale[index]);
        }
        if (Input.GetKeyUp(key))
        {
            synth.ReleaseNote(notes[index]);
        }
    }
}
