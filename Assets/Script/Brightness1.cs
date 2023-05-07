using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Brightness1 : MonoBehaviour
{
    [Header("Graphics Setting")]
    public Slider brightnessSlider;
    public PostProcessLayer layer;
    public PostProcessProfile brightness;
    AutoExposure exposure;
    [SerializeField] private TMP_Text BrightnessTextValue = null;

    // Start is called before the first frame update
    void Start()
    {
        brightness.TryGetSettings(out exposure);
        AdjustBrightness(brightnessSlider.value);
    }


    public void AdjustBrightness(float value)
    {
        if (value != null)
        {
            exposure.keyValue.value = value;
            
            BrightnessTextValue.text = value.ToString("0.0"); // Assign the formatted string to the text property


        }
        else
        {
            exposure.keyValue.value = .05f;
        }

    }

    
}
