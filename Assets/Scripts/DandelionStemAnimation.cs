using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DandelionStemAnimation : MonoBehaviour
{ 
public bool stemBent;
public bool bendingStem;
public bool unbendingStem;
    public bool waiting;

    //public Vector3[] setRotations;
    public float yRotation;

    public float xPosition;
    public float zPosition;
    ///public GameObject mainCamera;

    //public Quaternion initialStemRotation; // To store the initial rotation of the stem
    //public Quaternion targetStemRotation;

    public Vector3 startRotation;
    public Vector3 downRotation;

public float elapsedTime;


    void Start()
{

     xPosition = transform.position.x;
     zPosition = transform.position.z;

        if (xPosition > 85f && zPosition > 85f)
        {
            yRotation = -135f;
        }
        else if (xPosition > 85f && zPosition < 85f && zPosition > -85f)
        {
            yRotation = -90f;
        }
        else if (xPosition > 85f && zPosition < -85f)
        {
            yRotation = -45f;
        }
        else if (xPosition < 85f && xPosition > -85f && zPosition < -85f)
        {
            yRotation = 0f;
        }
        else if (xPosition < -85f && zPosition < -85f)
        {
            yRotation = 85f;
        }
        else if (xPosition < -85f && zPosition < 85f && zPosition > -85f)
        {
            yRotation = 90f;
        }
        else if (xPosition < -85f && zPosition > 85f)// && zPosition > -28f)
        {
            yRotation = 135f;
        }
        else if (xPosition < 85f && xPosition > -85f && zPosition < 85f)
        {
            yRotation = 180f;
        }

        transform.eulerAngles = new Vector3(0f, yRotation, 0f);
        startRotation = new Vector3(0f, yRotation, 0f);
        downRotation = new Vector3(-90f, yRotation, 0f);
    
        //initialStemRotation = transform.rotation; // Store the initial rotation of the stem

        ///mainCamera = GameObject.FindWithTag("MainCamera");

        //targetStemRotation = GetTargetRotation();

    }


 void Update()
 {

   if (bendingStem == true)
        {
            BendStem(); 
        }
        else if (waiting == true)
        {
            Waiting();
        }

        else if (unbendingStem == true)
        {
            UnendStem();
        }

 }


    public void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("Wind"))
    {
        if (stemBent == false)
        {
                stemBent = true;
                bendingStem = true;

        }

    }
}


public void BendStem()
{

        //elapsedTime = 0f;
        elapsedTime += Time.deltaTime;
        float t = elapsedTime / 2.0f;
        transform.eulerAngles = Vector3.Lerp(startRotation, downRotation, t);

        if (t >= 1.0f)
        {
            
            bendingStem = false;
            waiting = true;

            elapsedTime = 0f;
        }


}

public void Waiting()
    {

        elapsedTime += Time.deltaTime;

        float t = elapsedTime / 5f;

        if (t >= 1.0f)
        {

            waiting = false;
            unbendingStem = true;
            elapsedTime = 0f;
        }

    }
    public void UnendStem()
    {

        //elapsedTime = 0f;
        elapsedTime += Time.deltaTime;
        float t = elapsedTime / 3.0f;
        transform.eulerAngles = Vector3.Lerp(downRotation, startRotation, t);

        if (t >= 1.0f)
        {
            
            
            stemBent = false;
            unbendingStem = false;

            elapsedTime = 0f;

        }



    }

}

