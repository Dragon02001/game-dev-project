using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class Stamina : MonoBehaviour
{
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
        if(chara != null )
        {
            CharacterMovement characterMovement = chara.GetComponent<CharacterMovement>();
            currentStamina = characterMovement.playerStamina;

            Staminabar.fillAmount = currentStamina;

            Color blueStamina = new Color(0f, 0.25f, 1.0f, 1.0f);

            Staminabar.color = blueStamina;
        }
    }
}
