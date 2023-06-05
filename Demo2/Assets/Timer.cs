using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float timeRemaining = 490;
    public TMP_Text clockText;
    public AudioSource indicatorSource;
    public AudioClip pauseClip;
    public AudioClip resumeClip;
    public AudioClip successClip;
    public bool reproduce = true;
    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            DisplayTime(timeRemaining);
        }
        else
        {
            Debug.Log("Time has run out!");
            timeRemaining = 0;
            indicatorSource.PlayOneShot(successClip);
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        clockText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        if (seconds == 0)
        {
            if (reproduce)
            {
                indicatorSource.PlayOneShot(resumeClip);
                reproduce = false;
            }
        }
        else if (seconds == 15)
        {
            if (reproduce)
            {
                indicatorSource.PlayOneShot(pauseClip);
                reproduce = false;
            }
        }
        else
        {
            reproduce= true;
        }
    }
}
