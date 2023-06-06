using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene_control : MonoBehaviour
{
    public GameObject player; // Reference to the player
    public GameObject cutscenecam; // Reference to the cutscene

    private void OnTriggerEnter(Collider other)
    {

            cutscenecam.SetActive(true);
            player.SetActive(false);
        
    }
}
