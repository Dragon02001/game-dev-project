using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAOE : MonoBehaviour
{ 
public GameObject prefabToInstantiate;
public KeyCode keyToInstantiatePrefab;
public float timeToDestroy;

void Update()
{
    if (Input.GetKey(keyToInstantiatePrefab))
    {
        Vector3 offset = new Vector3(0.0f, 1f, 0.0f);
        Vector3 spawnPosition = transform.position + offset;
        GameObject newObject = Instantiate(prefabToInstantiate, spawnPosition, Quaternion.identity);
        Destroy(newObject, timeToDestroy);
    }
}
}