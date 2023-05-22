using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;


public class DayNightCycle : MonoBehaviour
{
    [SerializeField]
    private float timeMultiplier;

    [SerializeField]
    private float startHour;

    [SerializeField]
    private TextMeshProUGUI timeText;

    [SerializeField]
    private DateTime currentTime;

    [SerializeField]
    private Light sunLight;

    [SerializeField]
    private float sunRiseHour;

    [SerializeField]
    private float sunSetHour;

    private TimeSpan sunRise;
    private TimeSpan sunSet;
    // Start is called before the first frame update
    void Start()
    {
        currentTime = DateTime.Now.Date + TimeSpan.FromHours(startHour);

        sunRise = TimeSpan.FromHours(sunRiseHour);
        sunSet = TimeSpan.FromHours(sunSetHour);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTime();
        RotateSun();
    }

    private void UpdateTime()
    {
        currentTime = currentTime.AddSeconds(Time.deltaTime * timeMultiplier);
        
        if (timeText != null )
        {
            timeText.text = currentTime.ToString("HH:mm");
        }
    }

    private void RotateSun()
    {
        float sunLightRotation;

        if (currentTime.TimeOfDay > sunRise && currentTime.TimeOfDay < sunSet)
        {
            TimeSpan sunriseToSunsetDuration = CalculateTimeDiff(sunRise, sunSet);
            TimeSpan timeSinceSunrise = CalculateTimeDiff(sunRise, currentTime.TimeOfDay);

            double percentage = timeSinceSunrise.TotalMinutes / sunriseToSunsetDuration.TotalMinutes;

            sunLightRotation = Mathf.Lerp(0, 180, (float)percentage);
        }
        else
        {
            TimeSpan sunsetToSunriseDuration = CalculateTimeDiff(sunSet, sunRise);
            TimeSpan timeSinceSunset = CalculateTimeDiff(sunSet, currentTime.TimeOfDay);

            double percentage = timeSinceSunset.TotalMinutes / sunsetToSunriseDuration.TotalMinutes;

            sunLightRotation = Mathf.Lerp(180, 360, (float)percentage);
        }

        sunLight.transform.rotation = Quaternion.AngleAxis(sunLightRotation, Vector3.right);
    }

    private TimeSpan CalculateTimeDiff(TimeSpan fromTime, TimeSpan toTime)
    {
        TimeSpan differance = toTime - fromTime;

        if (differance.TotalSeconds < 0)
        {
            differance += TimeSpan.FromHours(24);
        }
        return differance;
    }
}
