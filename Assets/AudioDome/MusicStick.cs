using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicStick : MonoBehaviour
{
   

    public Oscillator2 oscillator;

    public Note[,] NoteStorageA = new Note[6,8];
    public Note[,] NoteStorageB = new Note[6,8];
    public Note[,] NoteStorageC = new Note[6,8];
    public Note[,] NoteStorageD = new Note[6,8];
    private float[,] frequencies;

    public Instrument instrumentA;
    public Instrument instrumentB;
    public Instrument instrumentC;
    public Instrument instrumentD;

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
   
            //Matt added - change color on interaction
            Renderer renderer = col.GetComponent<Renderer>();
            renderer.material.SetFloat("_Highlighted", 1.0f);

            string tileName = col.gameObject.name;
            if (name.Substring(2).Equals("Top")) return;
            int num = Int32.Parse(tileName.Substring(2));

            int octaveIndex = 5 - (num / 8);
            int intervalIndex = num % 8;

            Instrument instrument;
            Note[,] storage;
            if (tileName[0].Equals('A'))
            {

                instrument = instrumentA;
                storage = NoteStorageA;
                frequency = storage[octaveIndex, intervalIndex].getFrequency();
                //color change
                renderer.material.SetFloat("_Instrument_A", 1.0f);
                renderer.material.SetFloat("_Instrument_Color_FrequencyBlend", frequency);
            }
            else if (tileName[0].Equals('B'))
            {
                instrument = instrumentB;
                storage = NoteStorageB;
                frequency = storage[octaveIndex, intervalIndex].getFrequency();
                //color change
                renderer.material.SetFloat("_Instrument_B", 1.0f);
                renderer.material.SetFloat("_Instrument_Color_FrequencyBlend", frequency);
            }
            else if (tileName[0].Equals('C'))
            {
                instrument = instrumentC;
                storage = NoteStorageC;
                frequency = storage[octaveIndex, intervalIndex].getFrequency();
                //color change
                renderer.material.SetFloat("_Instrument_C", 1.0f);
                renderer.material.SetFloat("_Instrument_Color_FrequencyBlend", frequency);
            } 
            else
            {
                instrument = instrumentD;
                storage = NoteStorageD;
                frequency = storage[octaveIndex, intervalIndex].getFrequency();
                //color change
                renderer.material.SetFloat("_Instrument_D", 1.0f);
                renderer.material.SetFloat("_Instrument_Color_FrequencyBlend", frequency);
            }

            

            storage[octaveIndex,intervalIndex] = oscillator.PlayNote(instrument, frequencies[octaveIndex,intervalIndex]);
            
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

            oscillator.ReleaseNote(storage[octaveIndex,intervalIndex]);
        }
     
    }

    
    // Start is called before the first frame update
    void Start()
    {
        frequencies = NoteMatrix(32.7f,6);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
