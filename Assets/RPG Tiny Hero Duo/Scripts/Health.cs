using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class Health : MonoBehaviour
{
    public float currentHealth;
    private static UnityEngine.UI.Image Healthbar;

    void Start()
    {
        Healthbar = GetComponent<UnityEngine.UI.Image>();
    }

    void Update()
    {
        GameObject character = GameObject.FindWithTag("Player"); //Jack prefab is tagged as player
        if (character != null)
        {
            CharacterMovement characterMovement = character.GetComponent<CharacterMovement>();
            currentHealth = characterMovement.playerHealth;

            Healthbar.fillAmount = currentHealth;

            Color greenHealth = new Color(0.6f, 1, 0.6f, 1);
            Color yellowHealth = new Color(1, 0.92f, 0.016f, 1);
            Color redHealth = new Color(1, 0.3f, 0.3f, 1);

            if (currentHealth >= 0.6f)
            {
                Healthbar.color = greenHealth;
            }
            else if (currentHealth >= 0.3f)
            {
                Healthbar.color = yellowHealth;
            }
            else
            {
                Healthbar.color = redHealth;
            }
        }
    }
}