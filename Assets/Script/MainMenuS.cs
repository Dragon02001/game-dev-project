using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Rendering.PostProcessing;


public class MainMenuS : MonoBehaviour
{

    [Header("Level To Load")]
    public string _newGameLevel;
    private string levelToLoad;
    [SerializeField] private GameObject noSavedGameDialog = null;

    [Header("Confirmation")]
    [SerializeField] private GameObject ConfirmationPrompt = null;

    [Header("Volume Setting")]
    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;

    //[Header("Graphics Setting")]
    ////[SerializeField] private Slider brightnessSlider = null;
    ////[SerializeField] private TMP_Text brightnessTextValue = null;
    ////private float _brightnessLevel;





    public void NewGameDialogYes()
    {
        SceneManager.LoadScene(_newGameLevel);
    }
    public void LoadGameDialogYes()
    {
        if (PlayerPrefs.HasKey("SavedLevel"))
        {
            levelToLoad = PlayerPrefs.GetString("SavedLevel");
            SceneManager.LoadScene(levelToLoad);
        }
        else
        {
            noSavedGameDialog.SetActive(true);
        }
    }
    public void ExitButtion()
    {
        Application.Quit();
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

    //public void SetBrightness(float brightness)
    //{
    //    _brightnessLevel = brightness;
    //   brightnessTextValue.text = brightness.ToString("0.0");

    //}
}
