using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class GemScript : MonoBehaviour
{
    private bool isCollected = false;
    public Text gemsText;
    public AudioSource GemSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CollectGem();
        }
    }

    private void CollectGem()
    {
        if (!isCollected)
        {
            isCollected = true;
            gameObject.SetActive(false);

            int gemsCollected = 0;

            if (int.TryParse(gemsText.text.Replace("Gems: ", ""), out gemsCollected))
            {
                gemsCollected++;
                gemsText.text = "Gems: " + gemsCollected.ToString();
                GemSound.Play();
            }
            else
            {
                UnityEngine.Debug.LogError("Gems text format is invalid");
            }
        }
    }
}
