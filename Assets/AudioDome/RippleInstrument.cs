using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RippleInstrument : MonoBehaviour
{
    public GameObject waterPlaneObject; // Reference to the plane GameObject
    public Material waterPlaneMaterial; // Reference to the material used
    private Vector3 waterPlaneSize;
    /*
    private float currentWaveDensity_01;
    private float currentWaveDensity_02;
    private float currentWaveDensity_03;
    private float currentWaveDensity_04;
    */

    private float[] currentWaveDensities;

    private const float waveDensityDecreaseRate = 0.12f;
    private const float waveDensityThreshold = 0.01f;

    void Start()
    {
        // Get the plane's size in world space
        MeshRenderer waterPlaneRenderer = waterPlaneObject.GetComponent<MeshRenderer>();
        if (waterPlaneRenderer != null)
        {
            waterPlaneSize = waterPlaneRenderer.bounds.size;
        }

        // Initialize wave densities
        /*
        currentWaveDensity_01 = waterPlaneMaterial.GetFloat("_WaveDensity");
        currentWaveDensity_02 = waterPlaneMaterial.GetFloat("_WaveDensity_02");
        currentWaveDensity_03 = waterPlaneMaterial.GetFloat("_WaveDensity_03");
        currentWaveDensity_04 = waterPlaneMaterial.GetFloat("_WaveDensity_04");
        */

        currentWaveDensities = new float[4];

        currentWaveDensities[0] = waterPlaneMaterial.GetFloat("_WaveDensity");
        currentWaveDensities[1] = waterPlaneMaterial.GetFloat("_WaveDensity_02");
        currentWaveDensities[2] = waterPlaneMaterial.GetFloat("_WaveDensity_03");
        currentWaveDensities[3] = waterPlaneMaterial.GetFloat("_WaveDensity_04");

    }

    void Update()
    {
        UpdateWaveDensity(ref currentWaveDensities[0], "_WaveDensity");
        UpdateWaveDensity(ref currentWaveDensities[1], "_WaveDensity_02");
        UpdateWaveDensity(ref currentWaveDensities[2], "_WaveDensity_03");
        UpdateWaveDensity(ref currentWaveDensities[3], "_WaveDensity_04");

        //Debug.Log("WD1 = " + currentWaveDensities[0] + " __WD2 = " + currentWaveDensities[1] + " __WD3 = " + currentWaveDensities[2] + " __WD4 = " + currentWaveDensities[3]);
    }

    void UpdateWaveDensity(ref float currentWaveDensity, string propertyName)
    {
        if (currentWaveDensity > 0f)
        {
            currentWaveDensity -= waveDensityDecreaseRate;
            if (currentWaveDensity < waveDensityThreshold)
            {
                currentWaveDensity = 0f;
            }
            waterPlaneMaterial.SetFloat(propertyName, currentWaveDensity);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("WaterPlane"))
        {
            int minRipple = 0;
            float minValue = 0f;
            for (int i = 0; i < 4; i++)
            {
                if (currentWaveDensities[i] < minValue)
                {
                    minRipple = i;
                    minValue = currentWaveDensities[i];
                }
            }

            string pName = "";
            switch (minRipple)
            {
                case 0:
                {
                    pName = "_WaveDensity";
                    break;
                }
                case 1:
                {
                    pName = "_WaveDensity_02";
                    break;
                }
                case 2:
                {
                    pName = "_WaveDensity_03";
                    break;
                }
                case 3:
                {
                    pName = "_WaveDensity_04";
                    break;
                }
            }
            Debug.Log(pName);
            CreateRipple(ref currentWaveDensities[minRipple], pName, other);

            /*
            // Create a ripple effect based on available wave density slots
            if (currentWaveDensity_01 <= 0f)
            {
                CreateRipple(ref currentWaveDensity_01, "_WaveDensity", other);
            }
            else if (currentWaveDensity_02 <= 0f)
            {
                CreateRipple(ref currentWaveDensity_02, "_WaveDensity_02", other);
            }
            else if (currentWaveDensity_03 <= 0f)
            {
                CreateRipple(ref currentWaveDensity_03, "_WaveDensity_03", other);
            }
            else if (currentWaveDensity_04 <= 0f)
            {
                CreateRipple(ref currentWaveDensity_04, "_WaveDensity_04", other);
            }
            else
            {
               // Debug.Log("Too many ripples");
            }
            */
        }
    }

    void CreateRipple(ref float currentWaveDensity, string propertyName, Collider other)
    {
        currentWaveDensity = 12f;
        waterPlaneMaterial.SetFloat(propertyName, currentWaveDensity);

        Vector3 contactPoint = other.ClosestPoint(transform.position);
        Vector2 collisionPoint2D = new Vector2(contactPoint.x, contactPoint.z);

        Vector2 normalizedPoint = new Vector2(
            (collisionPoint2D.x - waterPlaneObject.transform.position.x) / waterPlaneSize.x + 0.5f,
            (collisionPoint2D.y - waterPlaneObject.transform.position.z) / waterPlaneSize.z + 0.5f
        );

        normalizedPoint.x = Mathf.Clamp01(normalizedPoint.x);
        normalizedPoint.y = Mathf.Clamp01(normalizedPoint.y);

        string impactPointPropertyName = propertyName.Replace("WaveDensity", "WaterImpactPoint");
        waterPlaneMaterial.SetVector(impactPointPropertyName, normalizedPoint);

        Debug.Log($"Ripple created at {normalizedPoint} with density {currentWaveDensity} on property {propertyName}");
    }
}




/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RippleInstrument : MonoBehaviour
{
    public GameObject waterPlaneObject; // Reference to the plane GameObject
    public Material waterPlaneMaterial;
    private Vector3 waterPlaneSize;
    private float currentWaveDensity_01;
    private float currentWaveDensity_02;
    private float currentWaveDensity_03;
    private float currentWaveDensity_04;
    // Start is called before the first frame update
    void Start()
    {
       
        // Get the plane's size in world space
        MeshRenderer waterPlaneRenderer = waterPlaneObject.GetComponent<MeshRenderer>();
        if (waterPlaneRenderer != null)
        {
            waterPlaneSize = waterPlaneRenderer.bounds.size;
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        

        if (currentWaveDensity_01 >= 0f)
        {
            Renderer renderer = waterPlaneObject.GetComponent<MeshRenderer>();
            currentWaveDensity_01 = renderer.sharedMaterial.GetFloat("_WaveDensity") - 0.04f;
            renderer.sharedMaterial.SetFloat("_WaveDensity", currentWaveDensity_01);
        }
      
        if (currentWaveDensity_02 >= 0f)
        {
            Renderer renderer = waterPlaneObject.GetComponent<Renderer>();
            currentWaveDensity_02 = renderer.sharedMaterial.GetFloat("_WaveDensity_02") - 0.04f;
            renderer.sharedMaterial.SetFloat("_WaveDensity_02", currentWaveDensity_02);
        }
      
        if (currentWaveDensity_03 >= 0f)
        {
            Renderer renderer = waterPlaneObject.GetComponent<Renderer>();
            currentWaveDensity_03 = renderer.sharedMaterial.GetFloat("_WaveDensity_03") - 0.04f;
            renderer.sharedMaterial.SetFloat("_WaveDensity_03", currentWaveDensity_03);
        }
       
        if (currentWaveDensity_04 >= 0f)
        {
            Renderer renderer = waterPlaneObject.GetComponent<Renderer>();
            currentWaveDensity_04 = renderer.sharedMaterial.GetFloat("_WaveDensity_04") - 0.04f;
            renderer.sharedMaterial.SetFloat("_WaveDensity_04", currentWaveDensity_04);
        }
      


       Debug.Log( "WD1 = " + currentWaveDensity_01  + "__WD2 = "  +
     currentWaveDensity_02 + "__WD3 =" +
   currentWaveDensity_03  + "__WD4 = " +
     currentWaveDensity_04);
    }

   

    void OnTriggerEnter(Collider other)
    {
        // Check if the collided object has the tag "WaterPlane"
        if (other.gameObject.CompareTag("WaterPlane"))
        {
            Renderer renderer = other.GetComponent<Renderer>();
            /*float waveDensity_01 = renderer.material.GetFloat("_WaveDensity");
            float waveDensity_02 = renderer.material.GetFloat("_WaveDensity_02");
            float waveDensity_03 = renderer.material.GetFloat("_WaveDensity_03");
            float waveDensity_04 = renderer.material.GetFloat("_WaveDensity_04");*/

/* if (currentWaveDensity_01 < 1f)
 {
     renderer.material.SetFloat("_WaveDensity", 12f);
     currentWaveDensity_01 = 12f;
     // Get the collision point from the trigger interaction
     Vector3 contactPoint = other.ClosestPoint(transform.position);

     // Convert the contact point to Vector2
     Vector2 collisionPoint2D = new Vector2(contactPoint.x, contactPoint.z);

     // Normalize the collision point to a range from 0 to 1
     Vector2 normalizedPoint = new Vector2(
         (collisionPoint2D.x - waterPlaneObject.transform.position.x) / waterPlaneSize.x + 0.5f,
         (collisionPoint2D.y - waterPlaneObject.transform.position.z) / waterPlaneSize.z + 0.5f
     );

     // Clamp the values between 0 and 1 to avoid any out-of-bounds issues
     normalizedPoint.x = Mathf.Clamp01(normalizedPoint.x);
     normalizedPoint.y = Mathf.Clamp01(normalizedPoint.y);

     renderer.material.SetVector("_WaterImpactPoint", normalizedPoint);

 }
 else if (currentWaveDensity_02 < 1f)
 {
     renderer.material.SetFloat("_WaveDensity_02", 12f);
     currentWaveDensity_02 = 12f;
     // Get the collision point from the trigger interaction
     Vector3 contactPoint = other.ClosestPoint(transform.position);

     // Convert the contact point to Vector2
     Vector2 collisionPoint2D = new Vector2(contactPoint.x, contactPoint.z);

     // Normalize the collision point to a range from 0 to 1
     Vector2 normalizedPoint = new Vector2(
         (collisionPoint2D.x - waterPlaneObject.transform.position.x) / waterPlaneSize.x + 0.5f,
         (collisionPoint2D.y - waterPlaneObject.transform.position.z) / waterPlaneSize.z + 0.5f
     );

     // Clamp the values between 0 and 1 to avoid any out-of-bounds issues
     normalizedPoint.x = Mathf.Clamp01(normalizedPoint.x);
     normalizedPoint.y = Mathf.Clamp01(normalizedPoint.y);

     renderer.material.SetVector("_WaterImpactPoint_02", normalizedPoint);

 }
 else if (currentWaveDensity_03 < 1f)
 {
     renderer.material.SetFloat("_WaveDensity_03", 12f);
     currentWaveDensity_03 = 12f;
     // Get the collision point from the trigger interaction
     Vector3 contactPoint = other.ClosestPoint(transform.position);

     // Convert the contact point to Vector2
     Vector2 collisionPoint2D = new Vector2(contactPoint.x, contactPoint.z);

     // Normalize the collision point to a range from 0 to 1
     Vector2 normalizedPoint = new Vector2(
         (collisionPoint2D.x - waterPlaneObject.transform.position.x) / waterPlaneSize.x + 0.5f,
         (collisionPoint2D.y - waterPlaneObject.transform.position.z) / waterPlaneSize.z + 0.5f
     );

     // Clamp the values between 0 and 1 to avoid any out-of-bounds issues
     normalizedPoint.x = Mathf.Clamp01(normalizedPoint.x);
     normalizedPoint.y = Mathf.Clamp01(normalizedPoint.y);

     renderer.material.SetVector("_WaterImpactPoint_03", normalizedPoint);

 }
 else if (currentWaveDensity_04 < 1f)
 {
     renderer.material.SetFloat("_WaveDensity_04", 12f);
     currentWaveDensity_04 = 12f;
     // Get the collision point from the trigger interaction
     Vector3 contactPoint = other.ClosestPoint(transform.position);

     // Convert the contact point to Vector2
     Vector2 collisionPoint2D = new Vector2(contactPoint.x, contactPoint.z);

     // Normalize the collision point to a range from 0 to 1
     Vector2 normalizedPoint = new Vector2(
         (collisionPoint2D.x - waterPlaneObject.transform.position.x) / waterPlaneSize.x + 0.5f,
         (collisionPoint2D.y - waterPlaneObject.transform.position.z) / waterPlaneSize.z + 0.5f
     );

     // Clamp the values between 0 and 1 to avoid any out-of-bounds issues
     normalizedPoint.x = Mathf.Clamp01(normalizedPoint.x);
     normalizedPoint.y = Mathf.Clamp01(normalizedPoint.y);

     renderer.material.SetVector("_WaterImpactPoint_04", normalizedPoint);

 }
 else
 {
     Debug.Log("too many ripples");
 }




}
}
}*/
