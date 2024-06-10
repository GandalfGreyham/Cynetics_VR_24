using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicStick : MonoBehaviour
{


    public Synthesizer synthesizer;
    public GlobalEffectsSynthesizer globalEffectsSynthesizer;
    public Note currentNote;

    public Note[,] NoteStorageA = new Note[6, 8];
    public Note[,] NoteStorageB = new Note[6, 8];
    public Note[,] NoteStorageC = new Note[6, 8];
    public Note[,] NoteStorageD = new Note[6, 8];
    public GlobalNote[,] GlobalNoteStorageA = new GlobalNote[6, 8];
    public GlobalNote[,] GlobalNoteStorageB = new GlobalNote[6, 8];
    public GlobalNote[,] GlobalNoteStorageC = new GlobalNote[6, 8];
    public GlobalNote[,] GlobalNoteStorageD = new GlobalNote[6, 8];
    private float[,] frequencies;

    public Instrument instrumentA;
    public Instrument instrumentB;
    public Instrument instrumentC;
    public Instrument instrumentD;


    //color change stuff
    float colorOctave;
    float colorInterval;

    //wammy Stuff
    public float wammyRotation;
    public float wammyRotationSet;
    public float wammyEffect;
    public float wammyIntensity = 2f;
    public float wammySpeed;


    public GameObject handController;
    public bool shouldRepeat;
    public GlobalNote globalNote;
    public int repeatDuration;


    //float groundColorSpeedChange;
    //float groundColor;//target ground color
    //float lastGroundColor;//holds the last target ground color
    //float currentGroundColor;//will change until it matches the target ground color
    //float newColor;
    //private bool colorTransitioning = false;

    public GameObject lumpyTerrain;




    public float[,] NoteMatrix(float baseFrequency, int octaves)
    {
        float[,] matrix = new float[octaves, 8];
        for (int i = 0; i < octaves; i++)
        {
            matrix[i, 0] = baseFrequency * Mathf.Pow(2, i);
            matrix[i, 1] = baseFrequency * 1.125f * Mathf.Pow(2, i);
            matrix[i, 2] = baseFrequency * 1.25f * Mathf.Pow(2, i);
            matrix[i, 3] = baseFrequency * 1.333f * Mathf.Pow(2, i);
            matrix[i, 4] = baseFrequency * 1.5f * Mathf.Pow(2, i);
            matrix[i, 5] = baseFrequency * 1.666f * Mathf.Pow(2, i);
            matrix[i, 6] = baseFrequency * 1.875f * Mathf.Pow(2, i);
            matrix[i, 7] = baseFrequency * 2f * Mathf.Pow(2, i);
        }
        return matrix;
    }

    void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.tag == "DomeTile")
        {
            float frequency;

            //Matt added - change color on interaction
            Renderer renderer = col.GetComponent<Renderer>();
            renderer.material.SetFloat("_Highlighted", 1.0f);

            string tileName = col.gameObject.name;
            if (name.Substring(2).Equals("Top")) return;
            int num = Int32.Parse(tileName.Substring(2));

            int octaveIndex = 5 - (num / 8);
            int intervalIndex = num % 8;
            colorOctave = octaveIndex / 5f;
            colorInterval = intervalIndex / 7f;

            //wammy Stuff
            wammyRotation = 0.01f + (180f * (gameObject.transform.rotation.x / 1f));
            wammyRotationSet = wammyRotation;
            //Debug.Log(wammyRotation + "," + wammyRotationSet);

            Instrument instrument;

            ///////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////

            Note[,] storage;
            if (tileName[0].Equals('A'))
            {

                instrument = instrumentA;
                storage = NoteStorageA;
                frequency = frequencies[octaveIndex, intervalIndex];///2092;

                //color change
                renderer.material.SetFloat("_Instrument_A", 1.0f);
                renderer.material.SetFloat("_OctaveRow", colorOctave);
                renderer.material.SetFloat("_Instrument_Color_FrequencyBlend", colorInterval);

                //ground color change
                //groundColor = 0; 


                //Debug.Log(colorOctave);
                //Debug.Log(colorInterval);
            }
            else if (tileName[0].Equals('B'))
            {
                instrument = instrumentB;
                storage = NoteStorageB;
                frequency = frequencies[octaveIndex, intervalIndex];///2092;

                //color change
                renderer.material.SetFloat("_Instrument_B", 1.0f);
                renderer.material.SetFloat("_OctaveRow", colorOctave);
                renderer.material.SetFloat("_Instrument_Color_FrequencyBlend", colorInterval);
                //groundColor = 5;
                //Debug.Log(colorOctave);
                //Debug.Log(colorInterval);
            }
            else if (tileName[0].Equals('C'))
            {
                instrument = instrumentC;
                storage = NoteStorageC;
                frequency = frequencies[octaveIndex, intervalIndex];///2092;

                //color change
                renderer.material.SetFloat("_Instrument_C", 1.0f);
                renderer.material.SetFloat("_OctaveRow", colorOctave);
                renderer.material.SetFloat("_Instrument_Color_FrequencyBlend", colorInterval);
                //groundColor = 10;
                //Debug.Log(colorOctave);
                //Debug.Log(colorInterval);
            }
            else
            {
                instrument = instrumentD;
                storage = NoteStorageD;
                frequency = frequencies[octaveIndex, intervalIndex];///2092;

                //color change
                renderer.material.SetFloat("_Instrument_D", 1.0f);
                renderer.material.SetFloat("_OctaveRow", colorOctave);
                renderer.material.SetFloat("_Instrument_Color_FrequencyBlend", colorInterval);
                //groundColor = 20f;

                //Debug.Log(colorOctave);
                //Debug.Log(colorInterval);
            }

            ///////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////

            GlobalNote[,] storageGlobal;
            if (tileName[0].Equals('A'))
            {

                instrument = instrumentA;
                storageGlobal = GlobalNoteStorageA;
                frequency = frequencies[octaveIndex, intervalIndex];///2092;

            }
            else if (tileName[0].Equals('B'))
            {
                instrument = instrumentB;
                storageGlobal = GlobalNoteStorageB;
                frequency = frequencies[octaveIndex, intervalIndex];///2092;

            }
            else if (tileName[0].Equals('C'))
            {
                instrument = instrumentC;
                storageGlobal = GlobalNoteStorageC;
                frequency = frequencies[octaveIndex, intervalIndex];///2092;

            }
            else
            {
                instrument = instrumentD;
                storageGlobal = GlobalNoteStorageD;
                frequency = frequencies[octaveIndex, intervalIndex];///2092;


            }

            if (shouldRepeat == true)
            {
                storage[octaveIndex, intervalIndex] = synthesizer.PlayNote(instrument, frequency);
                currentNote = storage[octaveIndex, intervalIndex];
            }
            else
            {
                storageGlobal[octaveIndex, intervalIndex] = globalEffectsSynthesizer.PlayNote(instrument, frequency);
                globalNote = storageGlobal[octaveIndex, intervalIndex];
                storage[octaveIndex, intervalIndex] = synthesizer.PlayNote(instrument, frequency);
                currentNote = storage[octaveIndex, intervalIndex];
            }

        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "DomeTile")
        {
            //Matt added - change color on interaction
            Renderer renderer = col.GetComponent<Renderer>();
            renderer.material.SetFloat("_Highlighted", 0.0f);

            //wammy stuff
            wammyRotation = 0f;
            wammyRotationSet = 0f;

            string tileName = col.gameObject.name;
            if (name.Substring(2).Equals("Top")) return;

            ///////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////

            Note[,] storage;
            if (tileName[0].Equals('A'))
            {
                storage = NoteStorageA;

                //color change
                renderer.material.SetFloat("_Instrument_A", 0.0f);
            }
            else if (tileName[0].Equals('B'))
            {
                storage = NoteStorageB;

                //color change
                renderer.material.SetFloat("_Instrument_B", 0.0f);
            }
            else if (tileName[0].Equals('C'))
            {
                storage = NoteStorageC;

                //color change
                renderer.material.SetFloat("_Instrument_C", 0.0f);
            }
            else
            {
                storage = NoteStorageD;

                //color change
                renderer.material.SetFloat("_Instrument_D", 0.0f);
            }

            ///////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////

            GlobalNote[,] storageGlobal;
            if (tileName[0].Equals('A'))
            {

                storageGlobal = GlobalNoteStorageA;

            }
            else if (tileName[0].Equals('B'))
            {

                storageGlobal = GlobalNoteStorageB;

            }
            else if (tileName[0].Equals('C'))
            {

                storageGlobal = GlobalNoteStorageC;

            }
            else
            {
                storageGlobal = GlobalNoteStorageD;

            }

            ///////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////

            int num = Int32.Parse(tileName.Substring(2));
            int octaveIndex = 5 - (num / 8);
            int intervalIndex = num % 8;

            if (shouldRepeat == true)
            {
                //repeatDuration = 3;
                synthesizer.ReleaseNote(storage[octaveIndex, intervalIndex]);
                StartCoroutine(NoteRepeats());
                globalEffectsSynthesizer.ReleaseNote(storageGlobal[octaveIndex, intervalIndex]);
                repeatDuration = 3;


            }
            else
            {
                synthesizer.ReleaseNote(storage[octaveIndex, intervalIndex]);
                globalEffectsSynthesizer.ReleaseNote(storageGlobal[octaveIndex, intervalIndex]);

            }

        }

    }
    IEnumerator NoteRepeats()
    {
        handController.transform.position = Vector3.zero;

        int currentCount = repeatDuration;

        while (currentCount > 0)
        {
            Debug.Log(currentCount); // Output the current count to the console
            yield return new WaitForSeconds(1); // Wait for 1 second
            currentCount--; // Decrease the count
        }

        handController.transform.position = new Vector3(100f,100f,100f);
        shouldRepeat = false;
        
        Debug.Log("Countdown finished!"); // Output when the countdown is done
    }
 


    // Start is called before the first frame update
    void Start()
    {
        frequencies = NoteMatrix(32.7f, 6);

        //currentGroundColor = groundColor;
        //lastGroundColor = groundColor;
    }

    // Update is called once per frame
    void Update()
    {

        if (wammyRotation != 0f)
        {

            wammyRotation = 0.01f + (180f * (gameObject.transform.rotation.x / 1f));
            wammyEffect = Mathf.Abs(wammyRotationSet - wammyRotation) / 180f;
            wammySpeed = wammyEffect;
            wammyIntensity = (wammyEffect + wammySpeed);


            if (wammyRotation > 15f || wammyRotation < -15)
            {
                currentNote.frequencyModifier = 1f + wammyIntensity;
                currentNote.ModifyOscillation(wammySpeed, wammyEffect * 2f);
                globalNote.frequencyModifier = 1f + wammyIntensity;
                globalNote.ModifyOscillation(wammySpeed, wammyIntensity);

            }
            else
            {
                currentNote.frequencyModifier = 1f;
                currentNote.ModifyOscillation(1f, 0f);
                globalNote.frequencyModifier = 1f;// + wammyIntensity;
                globalNote.ModifyOscillation(1f, 0f);

                //Debug.Log("wammyRotation: " + wammyRotation + ", wammyIntensity: " + wammyIntensity + ", wammyEffect: " + wammyEffect +", wammyIntensity: " + wammySpeed);
                //notePrefab.effectFromMusicStick_01 = 1f + (wammyEffect * wammyIntensity);
                //notePrefab.effectFromMusicStick_02 = wammySpeed;
                //notePrefab.effectFromMusicStick_03 = wammyEffect;
                //currentNote.frequencyModifier = 1f + (wammyEffect * wammyIntensity);
                //currentNote.ModifyOscillation(wammySpeed, wammyEffect);

            }
        }
        else
        {
            //resets all the wammy effects

            wammyEffect = 0f;
            wammyIntensity = 2f;
            wammySpeed = 0f;

            //currentNote.frequencyModifier = 1f;
            //currentNote.ModifyOscillation(1f, 0f);


            /*if (newColor != groundColor)
            {
                lastGroundColor = groundColor;
                groundColor = newColor;
                groundColorSpeedChange = Mathf.Abs(groundColor - lastGroundColor);
                currentGroundColor = Mathf.Lerp(lastGroundColor, groundColor, Time.deltaTime/groundColorSpeedChange);
                Renderer groundRenderer = lumpyTerrain.GetComponent<Renderer>();
                groundRenderer.material.SetFloat("_GroundColor", currentGroundColor);

                Debug.Log(currentGroundColor);

            }*/



        }
    }
}
