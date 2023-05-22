using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class matChange : MonoBehaviour
{

    public Material newMaterial; // Assign the material you want to use in the Inspector

     Renderer objectRenderer;

    // Start is called before the first frame update
    private void Start()
    {
        // Get the Renderer component attached to the same GameObject
        objectRenderer = GetComponent<Renderer>();
        objectRenderer.enabled = true;
        // Change the material of the object's Renderer
        objectRenderer.material = newMaterial;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
