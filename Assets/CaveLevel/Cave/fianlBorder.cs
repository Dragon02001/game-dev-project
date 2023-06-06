using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fianlBorder : MonoBehaviour
{

    private bool hasSpawned = false; // Flag to track if object has spawned

    public bool Finished = false;
    public Collider nonTriggerCollider1; // Reference to the non-trigger collider

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasSpawned)
        {

            // Disable the collider's "isTrigger" property
            nonTriggerCollider1.isTrigger = false;

        }
    }





    private void Update()
    {
        if (Finished)
        {
            nonTriggerCollider1.isTrigger = true;
        }
    }
}