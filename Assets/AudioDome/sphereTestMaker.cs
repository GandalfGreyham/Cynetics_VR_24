using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sphereTestMaker : MonoBehaviour
{
    public GameObject sphereInstrumentRipple;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Generate random x and z positions
            float randomX = Random.Range(-25f, 25f);
            float randomZ = Random.Range(-25f, 25f);

            // Set the y position to 5
            Vector3 spawnPosition = new Vector3(randomX, 20f, randomZ);

            // Instantiate the sphereInstrumentRipple at the calculated position
            Instantiate(sphereInstrumentRipple, spawnPosition, Quaternion.identity);

        }
    }
}
