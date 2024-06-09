using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicStick : MonoBehaviour
{
    //Quaternion worldRotation = transform.rotation;

    public Synthesizer synthesizer;

    public Note[,] NoteStorageA = new Note[6,8];
    public Note[,] NoteStorageB = new Note[6,8];
    public Note[,] NoteStorageC = new Note[6,8];
    public Note[,] NoteStorageD = new Note[6,8];
    private float[,] frequencies;

    public Instrument instrumentA;
    public Instrument instrumentB;
    public Instrument instrumentC;
    public Instrument instrumentD;

    float colorOctave;
    float colorInterval;

    //float groundColorSpeedChange;
    //float groundColor;//target ground color
    //float lastGroundColor;//holds the last target ground color
    //float currentGroundColor;//will change until it matches the target ground color
    //float newColor;
    //private bool colorTransitioning = false;

    public GameObject lumpyTerrain;


    public float wammyRotation;
    public float wammyRotationSet; 
    public float wammyEffect;
    public float wammyIntensity;
    public float wammySpeed;
   
    public bool wammyON;
    public GameObject objNotePrefab;
    public NotePrefab notePrefab;
    public Note note;
    public string notePrefabInstrumentName;
    public GameObject spawnPoint_A;


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

    void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.tag == "DomeTile")
        {
            float frequency;

            //wammyON = true;

            wammyRotation = transform.rotation.x + 0.01f;
            wammyRotationSet = wammyRotation;
            Debug.Log(wammyRotation + "," + wammyRotationSet);

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

            Instrument instrument;
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


            /////
            /////
            /////

            //GameObject newObjNotePrefab = Instantiate(objNotePrefab, spawnPoint_A.transform.position, Quaternion.identity);
            //NotePrefab notePrefab = objNotePrefab.GetComponent<NotePrefab>();
            //Synthesizer synthesizer = objNotePrefab.GetComponent<Synthesizer>();

            Instantiate(objNotePrefab, spawnPoint_A.transform.position, Quaternion.identity);
            notePrefab = objNotePrefab.GetComponent<NotePrefab>();
            note  = objNotePrefab.GetComponent<Note>();
            synthesizer = objNotePrefab.GetComponent<Synthesizer>();

            storage[octaveIndex, intervalIndex] = notePrefab.StoreInstrument(instrument, frequency);
            //storage[octaveIndex,intervalIndex] = synthesizer.PlayNote(instrument, frequencies[octaveIndex,intervalIndex]);
            //storage[octaveIndex, intervalIndex] = notePrefab.StoredInstrument(instrument, frequencies[octaveIndex, intervalIndex]);

            //notePrefab.instrument = instrument;
            //notePrefab.octaveIndexNotePrefab = octaveIndex;
            //notePrefab.intervalIndexPrefab = intervalIndex;
            //
            //notePrefab.storageNoteListRef = new string(storage);
            //notePrefab.StoredInstrument =  [octaveIndex, intervalIndex];

            //public Note[,] StoredInstrument;
            //public string storageInstrumentName;
            //public int octaveIndexNotePrefab;
            //public int intervalIndexNotePrefab;
        }
    }





    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "DomeTile")
        {
            //wammyON = false;
            wammyRotation = 0f;
            wammyRotationSet = wammyRotation;

            //Matt added - change color on interaction
            Renderer renderer = col.GetComponent<Renderer>();
            renderer.material.SetFloat("_Highlighted", 0.0f);

            string tileName = col.gameObject.name;
            if (name.Substring(2).Equals("Top")) return;


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

            int num = Int32.Parse(tileName.Substring(2));

            int octaveIndex = 5 - (num / 8);
            int intervalIndex = num % 8;


            synthesizer.ReleaseNote(storage[octaveIndex,intervalIndex]);
            Destroy(objNotePrefab);
            
        }
    
    }

    
    // Start is called before the first frame update
    void Start()
    {
        frequencies = NoteMatrix(32.7f,6);

        //currentGroundColor = groundColor;
        //lastGroundColor = groundColor;
    }

    // Update is called once per frame
    void Update()
    {

        if (wammyRotation != 0f)
        {

            wammyRotation = transform.rotation.x + 0.01f;
            wammyEffect = Mathf.Abs(wammyRotationSet - transform.rotation.x) / 360f;
            wammyIntensity = 2f;
            wammySpeed = Mathf.Lerp(0f, 100f, wammyEffect/wammyEffect*100f);

            notePrefab.effectFromMusicStick_01 = 1f + (wammyEffect * wammyIntensity);
            notePrefab.effectFromMusicStick_02 = wammySpeed;
            notePrefab.effectFromMusicStick_03 = wammyEffect;
            //currentNote.frequencyModifier = 1f + (wammyEffect * wammyIntensity);
            //currentNote.ModifyOscillation(wammySpeed, wammyEffect);

}
        else
        {

            wammyEffect = 0;  
            //wammyIntensity = 2f;
            wammySpeed = Mathf.Lerp(0f, 100f, wammyEffect / wammyEffect * 100f);
            //currentNote.frequencyModifier = 1f;
            //currentNote.ModifyOscillation(1f, 0f);
        }

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
