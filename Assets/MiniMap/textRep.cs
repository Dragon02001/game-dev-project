using System.Collections;
using UnityEngine;
using TMPro;

public class textRep : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public float displayTime = 10f; // Time to display each text in seconds
    public float transitionTime = 2f; // Time for the transition effect in seconds
    public float intervalTime = 60f; // Time between text changes in seconds

    private string[] texts = {
        "Follow the lights",
        "Beware of lurking enemies",
        "Stay hidden and avoid detection",
        "Navigate the forest cautiously",
        "Survive the watchful eyes of your foes"
    };

    private void Start()
    {
        StartCoroutine(DisplayText());
    }

    private IEnumerator DisplayText()
    {
        while (true)
        {
            int randomIndex = Random.Range(0, texts.Length);
            string text = texts[randomIndex];
            textMeshPro.text = text;

            // Fade in
            yield return StartCoroutine(FadeTextAlpha(0f, 1f, transitionTime));

            yield return new WaitForSeconds(displayTime);

            // Fade out
            yield return StartCoroutine(FadeTextAlpha(1f, 0f, transitionTime));

            textMeshPro.text = ""; // Hide the text

            yield return new WaitForSeconds(intervalTime - displayTime);
        }
    }

    private IEnumerator FadeTextAlpha(float startAlpha, float targetAlpha, float duration)
    {
        float startTime = Time.time;
        Color startColor = textMeshPro.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, targetAlpha);

        while (Time.time < startTime + duration)
        {
            float t = (Time.time - startTime) / duration;
            textMeshPro.color = Color.Lerp(startColor, targetColor, t);
            yield return null;
        }

        textMeshPro.color = targetColor;
    }

}



