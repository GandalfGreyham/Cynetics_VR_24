using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class FingerCollisionManager : MonoBehaviour
{
    public HandController handController;
    public int counter_R;
    public int counter_L;

    private static HashSet<string> collidingFingers_Right = new HashSet<string>();
    private static HashSet<string> collidingFingers_Left = new HashSet<string>();

    //UI for testing
    public Text text_01;
    public Text text_02;
    public Text text_03;
    public Text text_04;


    private void Start()
    {
       GameObject handControllerObject = GameObject.FindWithTag("HandController");
       handController = handControllerObject.GetComponent<HandController>();

    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the other collider is one of the right hand fingers and add it to the set
        if (other.CompareTag("IndexFinger_R") || other.CompareTag("ThumbFinger_R") ||
            other.CompareTag("MiddleFinger_R") || other.CompareTag("RingFinger_R") ||
            other.CompareTag("PinkyFinger_R"))
        {
            collidingFingers_Right.Add(other.tag);
            CheckAllFingersColliding();
        }

        // Check if the other collider is one of the left hand fingers and add it to the set
        if (other.CompareTag("IndexFinger_L") || other.CompareTag("ThumbFinger_L") ||
            other.CompareTag("MiddleFinger_L") || other.CompareTag("RingFinger_L") ||
            other.CompareTag("PinkyFinger_L"))
        {
            collidingFingers_Left.Add(other.tag);
            CheckAllFingersColliding();
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Check if the other collider is one of the right hand fingers and remove it from the set
        if (other.CompareTag("IndexFinger_R") || other.CompareTag("ThumbFinger_R") ||
            other.CompareTag("MiddleFinger_R") || other.CompareTag("RingFinger_R") ||
            other.CompareTag("PinkyFinger_R"))
        {
            collidingFingers_Right.Remove(other.tag);
            counter_R = 0;
        }

        // Check if the other collider is one of the left hand fingers and remove it from the set
        if (other.CompareTag("IndexFinger_L") || other.CompareTag("ThumbFinger_L") ||
            other.CompareTag("MiddleFinger_L") || other.CompareTag("RingFinger_L") ||
            other.CompareTag("PinkyFinger_L"))
        {

            collidingFingers_Left.Remove(other.tag);
            counter_L = 0;
        }
    }

    private void CheckAllFingersColliding()
    {
        // Check if the set contains all the right hand finger tags
        if (collidingFingers_Right.Contains("IndexFinger_R") &&
            collidingFingers_Right.Contains("ThumbFinger_R") &&
            collidingFingers_Right.Contains("MiddleFinger_R") &&
            collidingFingers_Right.Contains("RingFinger_R") &&
            collidingFingers_Right.Contains("PinkyFinger_R") && handController.allFingersCollide_R == false)
        {
            //logic when all right hand fingers are colliding

            handController.FingerCollisionDetectionRight();
            Debug.Log("All right hand fingers are colliding!");
            StartCoroutine(TogetherCounter_R());
            counter_R = 0;
        }
        else
        {
            counter_R = 0;
            
        }

        // Check if the set contains all the left hand finger tags
        if (collidingFingers_Left.Contains("IndexFinger_L") &&
            collidingFingers_Left.Contains("ThumbFinger_L") &&
            collidingFingers_Left.Contains("MiddleFinger_L") &&
            collidingFingers_Left.Contains("RingFinger_L") &&
            collidingFingers_Left.Contains("PinkyFinger_L") && handController.allFingersCollide_L == false)
        {

            //logic when all left hand fingers are colliding

            handController.FingerCollisionDetectionLeft();
            Debug.Log("All left hand fingers are colliding!");
            StartCoroutine(TogetherCounter_L());
            counter_L = 0;

        }

        else
        {

            counter_L = 0;
        }


        /* // Check if the set contains all the right and left hand finger tags
         if (collidingFingers_Right.Contains("IndexFinger_R") &&
             collidingFingers_Right.Contains("ThumbFinger_R") &&
             collidingFingers_Right.Contains("MiddleFinger_R") &&
             collidingFingers_Right.Contains("RingFinger_R") &&
             collidingFingers_Right.Contains("PinkyFinger_R") &&
             collidingFingers_Left.Contains("IndexFinger_L") &&
             collidingFingers_Left.Contains("ThumbFinger_L") &&
             collidingFingers_Left.Contains("MiddleFinger_L") &&
             collidingFingers_Left.Contains("RingFinger_L") &&
             collidingFingers_Left.Contains("PinkyFinger_L"))
         {
             Debug.Log("All fingers on both hands are colliding!");
             // Additional logic when all fingers on both hands are colliding
         }*/
    }

    IEnumerator TogetherCounter_R()
    {
        counter_R = 0;

        while (counter_R < 3)
        {
            Debug.Log(counter_R); // Output the current count to the console
            yield return new WaitForSeconds(1); // Wait for 1 second
            counter_R++;
        }

        if (counter_R >= 3)
        {
            handController.musicStick_R.repeatDuration = 6;
        }

        
    }
    IEnumerator TogetherCounter_L()
    {
        counter_L = 0;

        while (counter_L < 3)
        {
            Debug.Log(counter_L); // Output the current count to the console
            yield return new WaitForSeconds(1); // Wait for 1 second
            counter_L++;
        }

        if (counter_L >= 3)
        {
            handController.musicStick_L.repeatDuration = 6;
        }
    }

    public void AddText(string additionalText)
    {
        text_01.text += additionalText;
    }

}

