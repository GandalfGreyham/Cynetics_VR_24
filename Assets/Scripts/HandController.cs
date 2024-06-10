using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{

    

    //hand commands
    //public GameObject palmCollider_R;
    //public GameObject palmCollider_L;

    /*public GameObject thumbFinger_R;
    public GameObject indexFinger_R;
    public GameObject middleFinger_R;
    public GameObject ringFinger_R;
    public GameObject pinkyFinger_R;

    public GameObject thumbFinger_L;
    public GameObject indexFinger_L;
    public GameObject middleFinger_L;
    public GameObject ringFinger_L;
    public GameObject pinkyFinger_L;
    */
    public bool allFingersCollide_R;
    public bool allFingersCollide_L;
    public bool palmsTouch;

    public GameObject musicStick_R_Object;
    public MusicStick musicStick_R;
    public GameObject musicStick_L_Object;
    public MusicStick musicStick_L;

    //public Synthesizer synthesizer_R;
    //public Synthesizer synthesizer_L;
    //public Synthesizer globalEffectsSynthesizer;


    // Start is called before the first frame update
    void Start()
    {
        GameObject musicStick_R_Object = GameObject.FindWithTag("MusicStick_R");
        musicStick_R = musicStick_R_Object.GetComponent<MusicStick>();
        GameObject musicStick_L_Object = GameObject.FindWithTag("MusicStick_L");
        musicStick_L = musicStick_L_Object.GetComponent<MusicStick>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (musicStick_R.shouldRepeat == false) 
        {
            allFingersCollide_R = false;
        }
        if (musicStick_L.shouldRepeat == false)
        {
            allFingersCollide_R = false;
        }

    }

    public void FingerCollisionDetectionRight()
    {

        allFingersCollide_R = true;
        

    }
    public void FingerCollisionDetectionLeft()
    {

        allFingersCollide_L = true;

    }
    public void ClapPalmsTouch()
    {

        palmsTouch = true;
    }
    

}
