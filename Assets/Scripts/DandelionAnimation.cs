using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DandelionAnimation : MonoBehaviour
{
    //public Transform startPosition; // Initial position of the object
    public Vector3 targetPosition; // The first target position to move towards
    public Vector3 secondTargetPosition; // The second target position
    public Vector3 startPosition;

    public float firstDuration; // Duration to move to the first target position
    public float secondDuration; // Duration to move to the second target position
    //public float waitAtSecondTargetDuration = 10.0f; // Duration to wait at the second target position

    private float elapsedTime = 0f;

    public bool atStart;
    public bool isMovingTo_01;
    public bool isMovingTo_02;
    public bool isWaiting;
    public bool isFinished;

    public float heightReached;
    public float xReached;
    public float zReached;
    public Vector3 currentPosition;
   

    void Start()
    {
      

        startPosition = transform.position; // Start position as the current transform
        heightReached = Random.Range(3f, 7f);
        xReached = Random.Range(-6f, 6f);
        zReached = Random.Range(-6f, 6f);
        targetPosition = new Vector3(startPosition.x + xReached, startPosition.y + heightReached, startPosition.z + zReached);//targetPosition = new Vector3(xReached, heightReached, zReached);
        secondTargetPosition = new Vector3(startPosition.x + xReached, 0f - (startPosition.y + heightReached), startPosition.z + zReached);//secondTargetPosition = new Vector3(startPosition.position.x + xReached, 0f ,startPosition.position.z + zReached);

        firstDuration = Random.Range(0.5f, 1.5f);
        secondDuration = Random.Range(2.5f, 5f);
        
        atStart = true;

        

    }

    void Update()
    {
        if (isMovingTo_01)
        {
            MoveTowardsFirstTarget();
        }
        else if (isMovingTo_02)
        {
            MoveTowardsSecondTarget();
        }
        else if (isWaiting)
        {
            WaitAtSecondTarget();
        }
        else if (isFinished)
        {
            MoveBackToStart();
        }
    }

    public void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Wind") && atStart == true)
        {

            isMovingTo_01 = true;

        }

    }

    public void MoveTowardsFirstTarget()
    {

        atStart = false;

        Renderer renderer = gameObject.GetComponent<Renderer>();
        renderer.material.SetFloat("_DandelionHit", 1.0f);

        elapsedTime += Time.deltaTime;
        float t = elapsedTime / firstDuration;
        transform.position = Vector3.Lerp(startPosition, targetPosition, t);

        if (t >= 1.0f)
        {
            //elapsedTime = 0f;
            //secondTargetPosition = new Vector3(targetPosition.x, 0f, targetPosition.z);
            isMovingTo_01 = false;
            isMovingTo_02 = true;
            elapsedTime = 0f;

        }
    }

    public void MoveTowardsSecondTarget()
    {
        
        elapsedTime += Time.deltaTime;
        float t = elapsedTime / secondDuration;

        float horizontalMovement = Random.Range(0, 0.6f);
        float sideToSide = Mathf.Sin(t * Mathf.PI * 2) * horizontalMovement;

        currentPosition = Vector3.Lerp(targetPosition, secondTargetPosition, t);
        currentPosition.x += sideToSide;
        //currentPosition.z += sideToSide;

        transform.position = currentPosition;

        if (t >= 1.0f)
        {
            Renderer renderer = gameObject.GetComponent<Renderer>();
            renderer.material.SetFloat("_DandelionHit", 0.0f);
            
            isMovingTo_02 = false;
            isWaiting = true;
            elapsedTime = 0f;
        }
    }

    public void WaitAtSecondTarget()
    {
        

        elapsedTime += Time.deltaTime;

        float t = elapsedTime / 6f;

        if (t >= 1.0f)
        {
           
            isWaiting = false;
            isFinished = true;
            elapsedTime = 0f;
        }
    }

    public void MoveBackToStart()
    {

        isFinished = false;

        transform.position = startPosition;

        atStart = true;
            
        
    }
}


