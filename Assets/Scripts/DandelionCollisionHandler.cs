using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class DandelionCollisionHandler : MonoBehaviour
{

    public bool stemBent;
    public GameObject mainCamera;
    
    public Quaternion initialStemRotation; // To store the initial rotation of the stem


    void Start()
    {

        initialStemRotation = transform.rotation; // Store the initial rotation of the stem
       
        mainCamera = GameObject.FindWithTag("MainCamera");
        
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wind"))
        {
            if (stemBent == false)
            {
                StartBend();
            }
           
        }
    }

    public void StartBend()
    {
        stemBent = true;
        Quaternion targetRotation = GetTargetRotation();
        StartCoroutine(BendStem(targetRotation, 1.5f));
    }

    public void StartUnbend()
    {
        StartCoroutine(UnbendStem(initialStemRotation, 3.2f));
        
    }

    public IEnumerator BendStem(Quaternion targetRotation, float duration)
    {
       
        Quaternion initialRotation = transform.rotation;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, t);
            yield return null;
        }

        // Ensure the final rotation is exactly the target rotation
        transform.rotation = targetRotation;

   
    }

    public IEnumerator UnbendStem(Quaternion targetRotation, float duration)
    {

        Quaternion initialRotation = transform.rotation;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, t);
            yield return null;
        }

        // Ensure the final rotation is exactly the target rotation
        transform.rotation = targetRotation;

        stemBent = false;

    }

    public Quaternion GetTargetRotation()
    {
        // Calculate direction from the GameObject to the camera
        Vector3 directionToCamera = mainCamera.transform.position - transform.position;
        directionToCamera.y = 0; // Ignore the Y axis to keep the Y rotation unchanged

        // Calculate the direction 90 degrees away on the X and Z plane
        Vector3 awayFromCamera = Quaternion.Euler(0, 90, 0) * directionToCamera.normalized;

        // Create a rotation that looks at the awayFromCamera direction
        Quaternion lookRotation = Quaternion.LookRotation(awayFromCamera);

        // Create a new quaternion that only changes the X and Z rotation
        Quaternion targetRotation = Quaternion.Euler(lookRotation.eulerAngles.x, transform.rotation.eulerAngles.y, lookRotation.eulerAngles.z);

        return targetRotation;
    }
}



