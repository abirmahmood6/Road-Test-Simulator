using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour
{
    public Text timerText; // Reference to the UI Text element to display the timer

    private float totalTime = 240f; // Total time in seconds (4 minutes)
    protected float currentTime; // Current time left

    void Start()
    {
        // Set the initial time
        currentTime = totalTime;
    }

    void Update()
    {
        // Decrease the time
        currentTime -= Time.deltaTime;

        // Update the UI text
        UpdateTimerUI();

        // Check if time is up
        if (currentTime <= 0)
        {
            // Time's up, you can add your actions here
            Debug.Log("Time's up!");
            // For example, you can end the game or reset the level
        }
    }

    void UpdateTimerUI()
    {
        // Convert time to minutes and seconds
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);

        // Update the UI text with the remaining time
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
