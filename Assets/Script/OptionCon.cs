using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Rendering.PostProcessing;
public class OptionCon : MonoBehaviour
{
    public GameObject settingsCanvas;

    [Header("Back to main")]
    public string _backToMain;
    private string levelToLoad;
    

    [Header("Volume Setting")]
    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;

    [Header("Confirmation")]
    [SerializeField] private GameObject ConfirmationPrompt = null;

    public void BackToMainDialogYes()
    {
        SceneManager.LoadScene(_backToMain);
    }
    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        volumeTextValue.text = volume.ToString("0.0");

    }

    public void VolumeApply()
    {
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);

        //PlayerPrefs.SetFloat("masterBrightness", _brightnessLevel);
        StartCoroutine(ConfirmationBox());
    }

    public IEnumerator ConfirmationBox()
    {
        ConfirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(2);
        ConfirmationPrompt.SetActive(false);

    }

    

    public void CloseSettingsCanvas()
    {
        settingsCanvas.SetActive(false); // Hide the settings canvas
        Time.timeScale = 1f; // Resume the game
    }

   
    public void OpenSettingsCanvas()
    {
        if (!settingsCanvas.activeSelf)
        {
            Time.timeScale = 0f; // Pause the game
            settingsCanvas.SetActive(true); // Show the settings canvas
        }
    }

}
