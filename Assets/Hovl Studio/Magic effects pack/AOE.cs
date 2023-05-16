using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOE : MonoBehaviour
{
    public KeyCode keyToInstantiatePrefab1;
    public GameObject prefabToInstantiateA;
    public GameObject prefabToInstantiateB;
    public GameObject prefabToInstantiateC;

    public KeyCode keyToInstantiatePrefab2;
    public GameObject prefabToInstantiateD;
    public GameObject prefabToInstantiateE;
    public Vector3 offset;
    public Vector3 spawnPosition;

    public float timeToDestroy1;
    public float timeToDestroy2 = 0.5f;
    public int SideAbility;
    public int UltAbility;
    public GameObject newObject;
    void Update()
    {
        if (Input.GetKey(keyToInstantiatePrefab1))
        {
            if (UltAbility == 1)
            {
                position();
                newObject = Instantiate(prefabToInstantiateA, spawnPosition, Quaternion.identity);
                Destroy(newObject, timeToDestroy1);
            }
            else if (UltAbility == 2)
            {
                position();
                newObject = Instantiate(prefabToInstantiateB, spawnPosition, Quaternion.identity);
                Destroy(newObject, timeToDestroy1);
            }
            else if (UltAbility == 3)
            {
                position();
                newObject = Instantiate(prefabToInstantiateC, spawnPosition, Quaternion.identity);
                Destroy(newObject, timeToDestroy1);
            }
        }
        if (Input.GetKey(keyToInstantiatePrefab2))
        {
            if (SideAbility == 1)
            {
                position();
                Quaternion spawnRotation = transform.rotation;
                newObject = Instantiate(prefabToInstantiateD, spawnPosition, spawnRotation);

                move();

                Destroy(newObject, timeToDestroy2);
            }
            if (SideAbility == 2)
            {
                position();
                Quaternion spawnRotation = transform.rotation;
                newObject = Instantiate(prefabToInstantiateE, spawnPosition, spawnRotation);

                move();

                Destroy(newObject, timeToDestroy2);
            }
        }
    }
    private IEnumerator StopMovementAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
    void move()
    {

        Rigidbody rb = newObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            float moveDuration = 2.0f; // Adjust this value to set the duration of movement
            float moveSpeed = 10.0f; // Adjust this value to set the speed of movement

            rb.AddForce(newObject.transform.forward * moveSpeed, ForceMode.VelocityChange);

            // Stop the movement after the specified duration
            //StartCoroutine(StopMovementAfterDelay(newObject, moveDuration));
        }
    }
    void position()
    {
        offset = new Vector3(0.0f, 1f, 0.0f);
        spawnPosition = transform.position + offset;
    }

}