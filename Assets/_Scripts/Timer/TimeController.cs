using System;
using UnityEngine;
using TMPro; 

public class TimeController : MonoBehaviour
{
    [SerializeField] float timeMultiplier;
    [SerializeField] float startHour;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] Light sunLight;
    [SerializeField] float sunriseHour;
    [SerializeField] float sunsetHour;

    private DateTime currentTime;
    private TimeSpan sunriseTime;
    private TimeSpan sunsetTime;

    private void Awake()
    {
        currentTime = DateTime.Now.Date + TimeSpan.FromHours(startHour);

        sunriseTime = TimeSpan.FromHours(sunriseHour);
        sunsetTime = TimeSpan.FromHours(sunsetHour); 
    }
    private void Update()
    {
        UpdateTimeOfDay();
        RotateSun();
    }
    private void UpdateTimeOfDay()
    {
        currentTime = currentTime.AddSeconds(Time.deltaTime * timeMultiplier);   // deltaTime will give the actual seconds passed

        if (timeText != null)
        {
            // display time in hours and minutes 
            timeText.text = currentTime.ToString("HH:mm");
        }

    }
    private void RotateSun()
    {
        float sunLightRotation;
        // Check if it is daytime 
        if (currentTime.TimeOfDay > sunriseTime && currentTime.TimeOfDay < sunsetTime)
        {
            // calculate total time between sunrise & sunset 
            TimeSpan sunriseToSunsetDuration = CalculateTimeDifference(sunriseTime, currentTime.TimeOfDay);
            TimeSpan timeSinceSunrise = CalculateTimeDifference(sunriseTime, currentTime.TimeOfDay);

            // find out the percentage of daytime that has passed 
            double percentage = timeSinceSunrise.TotalMinutes / sunriseToSunsetDuration.TotalMinutes;

            // Use the % to work out sun rotation
            sunLightRotation = Mathf.Lerp(0, 90, (float)percentage); 
        }
        else  // Handles nightime
        {
            TimeSpan sunsetToSunriseDuration = CalculateTimeDifference(sunsetTime, sunriseTime);
            TimeSpan timeSinceSunset = CalculateTimeDifference(sunsetTime, currentTime.TimeOfDay);
            double percentage = timeSinceSunset.TotalMinutes / sunsetToSunriseDuration.TotalMinutes;
            sunLightRotation = Mathf.Lerp(180, 360, (float)percentage);
        }

        // Apply sun rotation to sunlight
        sunLight.transform.rotation = Quaternion.AngleAxis(sunLightRotation, Vector3.right);
    }
    private TimeSpan CalculateTimeDifference(TimeSpan fromTime, TimeSpan toTime)
    {
        TimeSpan difference = toTime - fromTime;

        // if diff is negative means both time transitioned from 1 day to another
        if (difference.TotalSeconds < 0)    
        {
            // add 24 hours to the diff to get the correct value
            difference += TimeSpan.FromHours(24); 
        }
        return difference;
    }

}
