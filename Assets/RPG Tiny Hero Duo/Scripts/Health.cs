using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    private CharacterMovement characterMovement;
    private UnityEngine.UI.Image healthbar;

    private void Start()
    {
        characterMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMovement>();
        healthbar = GetComponent<UnityEngine.UI.Image>();
    }

    private void Update()
    {
        if (characterMovement != null)
        {
            float currentHealth = characterMovement.Health;
            float maxHealth = characterMovement.MaxHealth;

            float healthPercentage = currentHealth / maxHealth;

            healthbar.fillAmount = healthPercentage;

            Color greenHealth = new Color(0.6f, 1, 0.6f, 1);
            Color yellowHealth = new Color(1, 0.92f, 0.016f, 1);
            Color redHealth = new Color(1, 0.3f, 0.3f, 1);

            if (healthPercentage >= 0.6f)
            {
                healthbar.color = greenHealth;
            }
            else if (healthPercentage >= 0.3f)
            {
                healthbar.color = yellowHealth;
            }
            else
            {
                healthbar.color = redHealth;
            }
        }
    }
}
