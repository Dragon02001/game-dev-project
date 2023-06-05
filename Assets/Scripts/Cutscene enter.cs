using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutsceneenter : MonoBehaviour
{
    GameObject player; // Reference to the player
    public GameObject cutscenecam; // Reference to the cutscene

    private void OnTriggerEnter(Collider other)
    {

            player.SetActive(false);
            cutscenecam.SetActive(true);
        
    }
}
