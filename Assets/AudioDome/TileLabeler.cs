using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameBasedValueAssigner : MonoBehaviour
{
    //public float value; // This will be set based on the object's name

    void Start()
    {
        AssignValueBasedOnName();
    }

    private void AssignValueBasedOnName()
    {
        string objectName = gameObject.name;

        Renderer renderer = gameObject.GetComponent<Renderer>();
        

        if (objectName.StartsWith("A"))
        {
            renderer.material.SetFloat("_CoreInstrumentChange", 0f);
        }
        else if (objectName.StartsWith("B"))
        {
            renderer.material.SetFloat("_CoreInstrumentChange", 0.125f);
        }
        else if (objectName.StartsWith("C"))
        {
            renderer.material.SetFloat("_CoreInstrumentChange", 0.25f);
        }
        else if (objectName.StartsWith("D"))
        {
            renderer.material.SetFloat("_CoreInstrumentChange", 0.375f);
        }
        else
        {
            Debug.LogWarning("Object name does not match any expected prefix (A, B, C, D): " + objectName);
        }
    }
}

