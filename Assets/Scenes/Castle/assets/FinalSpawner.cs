using UnityEngine;

public class FinalSpawner : MonoBehaviour
{
    public GameObject objectToSpawn; // The object to spawn
    public GameObject objectToSpawn2; // The object to spawn
    public Transform spawnPoint1; // The position where the object will spawn
    public Transform spawnPoint2; // The position where the object will spawn
    public Transform spawnPoint3; // The position where the object will spawn
    public Transform spawnPoint4; // The position where the object will spawn
    public Transform spawnPoint5; // The position where the object will spawn
    public Transform spawnPoint6; // The position where the object will spawn
 
    private bool hasSpawned = false; // Flag to track if object has spawned
    private bool hasEntered = false;
    private GameObject spawnedObject1; // Reference to the first spawned object
    private GameObject spawnedObject2; // Reference to the second spawned object
    private GameObject spawnedObject3; // Reference to the third spawned object
    private GameObject spawnedObject4; // Reference to the fourth spawned object
    private GameObject spawnedObject5; // Reference to the fifth spawned object
    private GameObject spawnedObject6; // Reference to the sixth spawned object
    private int spawnCount = 0;
    public Collider nonTriggerCollider1; // Reference to the non-trigger collider
    public Collider nonTriggerCollider2; // Reference to the non-trigger collider
    public Collider nonTriggerCollider3; // Reference to the non-trigger collider
    public Collider nonTriggerCollider4; // Reference to the non-trigger collider
    public Collider nonTriggerCollider5; // Reference to the non-trigger collider
    public Collider nonTriggerCollider6; // Reference to the non-trigger collider
    public Collider nonTriggerCollider7; // Reference to the non-trigger collider
    public Collider nonTriggerCollider8; // Reference to the non-trigger collider

    public Collider cutSceneCollider; // Refrence the trigger after the fight
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasSpawned)
        {
            SpawnObject();
            hasSpawned = true;
            hasEntered = true;
            // Disable the collider's "isTrigger" property
            nonTriggerCollider1.isTrigger = false;
            nonTriggerCollider2.isTrigger = false;
            nonTriggerCollider3.isTrigger = false;
            nonTriggerCollider4.isTrigger = false;
            nonTriggerCollider5.isTrigger = false;
            nonTriggerCollider6.isTrigger = false;
            nonTriggerCollider7.isTrigger = false;
            nonTriggerCollider8.isTrigger = false;
        }
    }

    private void SpawnObject()
    {
        spawnedObject1 = Instantiate(objectToSpawn, spawnPoint1.position, spawnPoint1.rotation);
        spawnedObject2 = Instantiate(objectToSpawn, spawnPoint2.position, spawnPoint2.rotation);
        spawnedObject3 = Instantiate(objectToSpawn, spawnPoint3.position, spawnPoint3.rotation);
        spawnedObject4 = Instantiate(objectToSpawn, spawnPoint4.position, spawnPoint4.rotation);
        spawnedObject5 = Instantiate(objectToSpawn, spawnPoint5.position, spawnPoint5.rotation);
        spawnedObject6 = Instantiate(objectToSpawn, spawnPoint6.position, spawnPoint6.rotation);
        spawnCount++;
    }
    private void SpawnObject2()
    {
        spawnedObject1 = Instantiate(objectToSpawn2, spawnPoint1.position, spawnPoint1.rotation);
        spawnedObject2 = Instantiate(objectToSpawn2, spawnPoint2.position, spawnPoint2.rotation);
        spawnedObject3 = Instantiate(objectToSpawn2, spawnPoint3.position, spawnPoint3.rotation);
        spawnedObject4 = Instantiate(objectToSpawn2, spawnPoint4.position, spawnPoint4.rotation);
        spawnedObject5 = Instantiate(objectToSpawn2, spawnPoint5.position, spawnPoint5.rotation);
        spawnedObject6 = Instantiate(objectToSpawn2, spawnPoint6.position, spawnPoint6.rotation);
        spawnCount++;
    }

    private bool AreAllObjectsDestroyed()
    {
        return spawnedObject1 == null && spawnedObject2 == null && spawnedObject3 == null &&
               spawnedObject4 == null && spawnedObject5 == null && spawnedObject6 == null;
    }

    private void Update()
    {
        if (hasEntered)
        {
            if (spawnCount == 1)
            {
                if (AreAllObjectsDestroyed())
                {
                    SpawnObject();
                }
            }
            else if (spawnCount == 2)
            {
                if (AreAllObjectsDestroyed())
                {
                    SpawnObject2();
                }
            }
            else if (spawnCount == 3) {
                if (AreAllObjectsDestroyed())
                {
                    cutSceneCollider.isTrigger = true;
                }
            }
        }
    }
}
