using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSpawn : MonoBehaviour

{
    public GameObject objectToSpawn; // The object to spawn
    public Transform spawnPoint1; // The position where the object will spawn
    private bool hasSpawned = false; // Flag to track if object has spawned

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasSpawned)
        {
            SpawnObject();
            hasSpawned = true;
        }
    }

    private void SpawnObject()
    {
        GameObject spawnedObject = Instantiate(objectToSpawn, spawnPoint1.position, spawnPoint1.rotation);
        Destroy(spawnedObject, 3f);
    }
}
