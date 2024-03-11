using System;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public TextMesh timerText;
    private float startTime;

    void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
        //float elapsedTime = Time.time - startTime;
        DateTime currentTime = DateTime.Now;

        string timerString = currentTime.ToString("HH:mm:ss.ff");

        //int minutes = (int)(elapsedTime / 60);
        //int seconds = (int)(elapsedTime % 60);
        //float milliseconds = (elapsedTime * 1000) % 1000;

        //string timerString = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
        timerText.text = timerString;
    }
}