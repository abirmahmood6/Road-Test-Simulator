using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageDisplay : MonoBehaviour
{
    public Text messageText;
    public Text scoreText;
    public float displayDuration = 3f; // Duration to display the message

    private float displayTimer = 0f;

    void Start()
    {
        // Clear initial message
        messageText.text = "";
    }

    void Update()
    {
        // Update the timer
        if (displayTimer > 0)
        {
            displayTimer -= Time.deltaTime;
            if (displayTimer <= 0)
            {
                // Clear the message when the timer reaches 0
                messageText.text = "";
            }
        }
    }

    // Method to display a message on the screen for a certain duration
    public void DisplayMessage(string message)
    {
        // Set the message
        messageText.text = message;
        // Start the timer
        displayTimer = displayDuration;
    }

    // Method to display the score on the screen
    public void DisplayScore(int score)
    {
        scoreText.text = score.ToString();
    }


}


