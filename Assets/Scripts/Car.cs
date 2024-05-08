/* /* This Car class represent the player car which takes input from the keyboard
 * and move the car accordingly.*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class Car : MonoBehaviour
{
    // Variables to control car movement
    [SerializeField] float accelerationSpeed = 5f;
    [SerializeField] float maxSpeed = 10f;
    [SerializeField] float reverseSpeed = 3f; // Speed when reversing
    [SerializeField] float brakeSpeed = 5f;
    [SerializeField] float turnSpeed = 100f;

    // UnityEvent for displaying messages
    public UnityEvent<string> displayMessageEvent;

    // Reference to the parking progress slider
    public UnityEngine.UI.Slider parkingProgressSlider;

    private int score = 30;
    private float currentSpeed = 0f;
    private float currentTurn = 0f;

    private bool hasParked = false;

    public Collider2D parkingSpaceCollider;

    void Start()
    {
        // Initialize the display message event
        if (displayMessageEvent == null)
            displayMessageEvent = new UnityEvent<string>();

        // Find MessageDisplay object in the scene
        MessageDisplay messageDisplay = FindObjectOfType<MessageDisplay>();
        if (messageDisplay != null)
        {
            // Connect the display message event to the message display method
            displayMessageEvent.AddListener(messageDisplay.DisplayMessage);
        }
        else
        {
            Debug.LogError("MessageDisplay object not found in the scene!");
        }
    }



    // Update is called once per frame
    void Update()
    {
        if (hasParked)
            return;

        // Brake takes priority
        if (Input.GetKey(KeyCode.B))
        {
            if (currentSpeed > 0)
            {
                currentSpeed -= brakeSpeed * Time.deltaTime;
            }
            else if (currentSpeed < 0) // Braking while reversing
            {
                currentSpeed += brakeSpeed * Time.deltaTime;
            }
            else
            {
                currentSpeed = 0;
            }
        }
        // Acceleration
        else if (Input.GetKey(KeyCode.Space))
        {
            if (currentSpeed < maxSpeed)
            {
                currentSpeed += accelerationSpeed * Time.deltaTime;
            }
        }
        // Reverse
        else if (Input.GetKey(KeyCode.R))
        {
            if (currentSpeed > -maxSpeed / 2) // Limit the reverse speed
            {
                currentSpeed -= reverseSpeed * Time.deltaTime;
            }
        }
        // Natural deceleration or 'idle' braking when no keys pressed
        else
        {
            if (currentSpeed > 0)
            {
                currentSpeed -= brakeSpeed * Time.deltaTime; // Decelerate
            }
            else if (currentSpeed < 0)
            {
                currentSpeed += brakeSpeed * Time.deltaTime; // Decelerate when reversing
            }
            else
            {
                currentSpeed = 0;
            }
        }


        // Apply current speed to move the car
        transform.Translate(Vector3.up * currentSpeed * Time.deltaTime);

        // Turning left and right
        float horizontalInput = Input.GetAxis("Horizontal");
        currentTurn = horizontalInput * turnSpeed * Time.deltaTime;

        transform.Rotate(Vector3.forward * -currentTurn);

        // Check if the car is inside the parking lot
        if (parkingSpaceCollider != null && parkingSpaceCollider.bounds.Contains(transform.position))
        {
            float distanceToEnter = Vector2.Distance(transform.position, parkingSpaceCollider.bounds.min);
            float totalDistance = Vector2.Distance(parkingSpaceCollider.bounds.min, parkingSpaceCollider.bounds.max);
            float parkingProgress = (distanceToEnter / totalDistance) * 100f;

            // Update the parking progress slider value
            parkingProgressSlider.value = parkingProgress;

            // Check if parking is complete
            if (parkingProgress >= 100f)
            {
                hasParked = true;
                Debug.Log("Parking complete!");
            }
        }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("RoadBoundry"))
        {
            AddScore(5);
            displayMessageEvent.Invoke("Collided with road!");
        }
        else if (collision.gameObject.CompareTag("Pedestrian"))
        {
            AddScore(5);
            displayMessageEvent.Invoke("Collided with pedestrian!");
        }
        else if (collision.gameObject.CompareTag("AI"))
        {
            AddScore(5);
            displayMessageEvent.Invoke("Collided with AI car!");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Pedestrian"))
        {
            AddScore(30);
            displayMessageEvent.Invoke("Collided with pedestrian!");
        }
        else if (other.CompareTag("TrafficLight"))
        {
            AddScore(5);
            displayMessageEvent.Invoke("Collided with traffic light!");
        }
    }

    void AddScore(int points)
    {
        if (score > 30)
        score -= points;
        // Update the score display or send it wherever you need
        Debug.Log("Current score: " + score);
    }


}






