using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class Stamina : MonoBehaviour
{
    private float pulseFrequency = 5.0f; // the frequency of the pulse
    private float pulseAmplitude = 0.01f; // the amplitude of the pulse

    public float currentStamina;
    private static UnityEngine.UI.Image Staminabar;

    // Start is called before the first frame update
    void Start()
    {
        Staminabar = GetComponent<UnityEngine.UI.Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
        GameObject chara = GameObject.FindWithTag("Player"); //Jack prefab is tagged as player
        if (chara != null)
        {
            CharacterMovement characterMovement = chara.GetComponent<CharacterMovement>();
            currentStamina = characterMovement.playerStamina;

            Staminabar.fillAmount = currentStamina;

            Color blueStamina = new Color(0f, 0.25f, 1.0f, 1.0f);
            Color yellowStamina = new Color(1, 0.92f, 0.016f, 1);
            Color redStamina = new Color(1, 0.3f, 0.3f, 1);

            if (currentStamina >= 0.6f)
            {
                Staminabar.color = blueStamina;
            }
            else if (currentStamina >= 0.3f)
            {
                Staminabar.color = yellowStamina;
            }
            else
            {
                Staminabar.color = redStamina;

            }
        }
        
    }
    public void pulse()
    {
        float pulse = Mathf.Sin(Time.time * pulseFrequency) * pulseAmplitude + 1.0f;
        Staminabar.transform.localScale = new Vector3(pulse, pulse, 1.0f);
    }
}
