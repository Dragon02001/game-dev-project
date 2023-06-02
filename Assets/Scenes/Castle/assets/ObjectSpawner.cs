using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject objectToSpawn; // The object to spawn
    public Transform spawnPoint1; // The position where the object will spawn
    public Transform spawnPoint2; // The position where the object will spawn
    public Transform spawnPoint3; // The position where the object will spawn
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
        Instantiate(objectToSpawn, spawnPoint1.position, spawnPoint1.rotation);
        Instantiate(objectToSpawn, spawnPoint2.position, spawnPoint2.rotation);
        Instantiate(objectToSpawn, spawnPoint3.position, spawnPoint3.rotation);
    }
}
