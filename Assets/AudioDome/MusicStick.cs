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
    public float wammyTiming;
    public int frameCounter = 0;
    public float newNote;
    public bool wammyON;
    

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

            wammyRotation = transform.rotation.x;

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

            

            storage[octaveIndex,intervalIndex] = synthesizer.PlayNote(instrument, frequencies[octaveIndex,intervalIndex]);
            
        }
    }


    void OnTriggerStay(Collider col)
    {
       
        if (col.gameObject.tag == "DomeTile")
        {
            frameCounter++;

            float wammyComparison = Mathf.Abs(wammyRotation - transform.rotation.x);

            //Debug.Log(wammyRotation + " , " + transform.rotation.x + " === WC " + wammyComparison);

            float wammyEffect = wammyComparison / 360f;
            float wammySpeed = Mathf.Lerp(120, 20, wammyEffect); //45, 15
            float wammyIntensity = 2f;

            //Debug.Log("WC" + wammyComparison + "WE" + wammyEffect + "WS" + wammySpeed + "WI" + wammyIntensity);
           
            if (frameCounter >= wammySpeed)
            {
                frameCounter = 0; // Reset the counter
                foreach (Note note in synthesizer.ActiveNotes)
                {

                    note.frequencyModifier = 1 + (wammyIntensity * wammyEffect);

                    newNote = note.frequencyModifier;
                    Debug.Log("newNote set to" + newNote + ", oldNote is " + note.frequencyModifier);

                    wammyTiming = wammySpeed/60f;

                    Debug.Log("updated Wammy");

                    
                }

                StopCoroutine(UnWammyBar(newNote, 1f, wammyTiming));
                wammyON = false;

            }
            else if (frameCounter < wammySpeed && wammyON == false)
            {

                
                StartCoroutine(UnWammyBar(newNote, 1f, wammyTiming));

            }

        }

    }

    public IEnumerator UnWammyBar(float from, float to, float duration)
    {
        wammyON = true;
        foreach (Note note in synthesizer.ActiveNotes)
        {  
            float elapsedTime = 0f;
            newNote = from;
            Debug.Log(newNote + "in UnWammy");
            //duration = wammyTiming;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                note.frequencyModifier = Mathf.Lerp(from, to, elapsedTime / wammyTiming);
                //Debug.Log("UnWammyING");
                yield return null; // Wait for the next frame
            }

            // Ensure the final value is set to the target value
            //newNote = to;
            //Debug.Log("Final Value: " + newNote);
        }
    }


    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "DomeTile")
        {
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

            wammyRotation = 0f;

            synthesizer.ReleaseNote(storage[octaveIndex,intervalIndex]);
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
