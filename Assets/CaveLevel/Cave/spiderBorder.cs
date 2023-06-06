using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spiderBorder : MonoBehaviour
{
    public GameObject objectToSpawn; // The object to spawn
    public Transform spawnPoint1; // The position where the object will spawn
    public fianlBorder finalBorderScript; // Reference to the fianlBorder script

    private bool hasSpawned = false; // Flag to track if object has spawned
    private bool hasEntered = false;
    private GameObject spawnedObject1; // Reference to the first spawned object

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasSpawned)
        {
            SpawnObject();
            hasSpawned = true;
            hasEntered = true;
        }
    }

    private void SpawnObject()
    {
        spawnedObject1 = Instantiate(objectToSpawn, spawnPoint1.position, spawnPoint1.rotation);
    }

    private bool AreAllObjectsDestroyed()
    {
        return spawnedObject1 == null;
    }

    private void Update()
    {
        if (hasEntered)
        {
            if (AreAllObjectsDestroyed())
            {
                finalBorderScript.Finished = true; // Trigger the Finished boolean in fianlBorder script
            }
        }
    }
}
