/* /* This Car class represent the player car which takes input from the keyboard
 * and move the car accordingly.*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    [SerializeField] TrafficLight trafficLight_Ver;
    [SerializeField] OppositeTrafficLight trafficLight_Hor;
    [SerializeField] CountdownTimer countdownTimer;

    // UnityEvent for displaying messages
    public UnityEvent<string> displayMessageEvent;
    public UnityEvent<int> updateScoreEvent;

    // Reference to the parking progress slider
    public Slider parkingSlider;
    public Toggle[] carParkingToogle;
    public Transform parkingLot;

    private int score = 30;
    private float currentSpeed = 0f;
    private float currentTurn = 0f;

    private bool isParkingCorrect = false;
    private bool isInsideParkingLot = false;
    private float parkingSpeed = 0.5f; 

    private int stopCount = 0, StopCountOnce = 0; // Count of stop sign encounters
    private bool isNearStopSign = false, isNearStopSignOnce = false; // Flag to indicate if the player car is near a stop sign
    

    public Collider2D parkingSpaceCollider;

    void Start()
    {
        // Initialize the display message event
        if (displayMessageEvent == null)
        {
            displayMessageEvent = new UnityEvent<string>();
        }
        if (updateScoreEvent == null)
        {
            updateScoreEvent = new UnityEvent<int>();
        }
        
        // Find MessageDisplay object in the scene
        MessageDisplay messageDisplay = FindObjectOfType<MessageDisplay>();
        if (messageDisplay != null)
        {
            // Connect the display message event to the message display method
            displayMessageEvent.AddListener(messageDisplay.DisplayMessage);
            updateScoreEvent.AddListener(messageDisplay.DisplayScore);
        }
        else
        {
            Debug.LogError("MessageDisplay object not found in the scene!");
        }
    }



    // Update is called once per frame
    void Update()
    {
        // Smoothly update the parking slider value
        if (isInsideParkingLot && parkingSlider != null && !isParkingCorrect)
        {
            float targetValue = CalculateParkingPercentage();
            parkingSlider.value = Mathf.MoveTowards(parkingSlider.value, targetValue, parkingSpeed * Time.deltaTime);
            if (Mathf.Approximately(parkingSlider.value, targetValue))
            {
                isParkingCorrect = targetValue >= 0.85f;
                if (isParkingCorrect)
                {
                    carParkingToogle[0].isOn = true;
                    Debug.Log("Car is parked correctly!");
                    // You can add more actions here when the car is parked correctly
                }
            }
        }


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

        

        if ((isNearStopSign || isNearStopSignOnce) && Input.GetKeyDown(KeyCode.B)) // Assuming 'S' key is for stopping
        {
            stopCount++;
            StopCountOnce++;
           
        }

        if ( (carParkingToogle[0].isOn) && (carParkingToogle[1].isOn) &&
            ( carParkingToogle[2].isOn) && (carParkingToogle[3].isOn) &&
              (carParkingToogle[4].isOn))
        {
            displayMessageEvent.Invoke("You passed the test!");
            Debug.Log("You passed");
        }

        if (score <= 0)
        {
            displayMessageEvent.Invoke("You failed the test!");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("RoadBoundry"))
        {
            UpdateScore(5);
            updateScoreEvent.Invoke(score);
            displayMessageEvent.Invoke("You went out of bound!");
        }
        else if (collision.gameObject.CompareTag("Pedestrian"))
        {
            UpdateScore(30);
            updateScoreEvent.Invoke(score);
            displayMessageEvent.Invoke("You killed a pedestrian!");
        }
        else if (collision.gameObject.CompareTag("AI"))
        {
            UpdateScore(5);
            updateScoreEvent.Invoke(score);
            displayMessageEvent.Invoke("Collided with AI car! You should be more careful");
        }
        else if (collision.gameObject.CompareTag("Cones"))
        {
            UpdateScore(5);
            updateScoreEvent.Invoke(score);
            displayMessageEvent.Invoke("Collided with Construction cones! You should avoid the cones!");
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("TrafficLight") && trafficLight_Ver.indexOfCurrColor == 1)
        {
            UpdateScore(5);
            updateScoreEvent.Invoke(score);
            displayMessageEvent.Invoke("You violated the traffic light! You should respect the traffic rule");
        }
        else if ((other.CompareTag("TrafficLight") && trafficLight_Ver.indexOfCurrColor != 1) || (other.CompareTag("TrafficLight_Ver") && trafficLight_Hor.indexOfCurrColor != 1))
        {
            carParkingToogle[1].isOn = true;
        }
        else if (other.CompareTag("TrafficLight_Ver") && trafficLight_Hor.indexOfCurrColor == 1)
        {
            UpdateScore(5);
            updateScoreEvent.Invoke(score);
            displayMessageEvent.Invoke("You violated the traffic light! You should respect the traffic rule");
        }
        else if (other.CompareTag("StopSignTrigger"))
        {
            isNearStopSign = true;
        }
        else if (other.CompareTag("StopSignTriggerOnce"))
        {

            isNearStopSignOnce = true;
        }
        else if (other.CompareTag("ParkingSpace"))
        {
            isInsideParkingLot = true;
        }
        else if (other.CompareTag("Detour"))
        {
            carParkingToogle[3].isOn = true;
        }


    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("StopSignTrigger"))
        {
            isNearStopSign = false;

            // If the stop count is 2, reset it and allow the player car to continue
            if (stopCount != 2)
            {
                stopCount = 0;
                // Display a message that the car can continue
                UpdateScore(5);
                updateScoreEvent.Invoke(score);
                displayMessageEvent.Invoke("You violated the Stop Sign. You should have stop Twice (2x)");
            }

            else if (stopCount == 2)
            {
                carParkingToogle[2].isOn = true;
            }
        }

        else if (other.CompareTag("StopSignTriggerOnce"))
        {
            isNearStopSign = false;

            // If the stop count is 2, reset it and allow the player car to continue
            if (StopCountOnce != 1)
            {
                StopCountOnce = 0;
                // Display a message that the car can continue
                UpdateScore(5);
                updateScoreEvent.Invoke(score);
                displayMessageEvent.Invoke("You violated the Stop Sign. You should have stop Once");
            }

            else if (stopCount == 1)
            {
                carParkingToogle[4].isOn = true;
            }
        }

        else if (other.CompareTag("ParkingSpace"))
        {
            // Reset the parking slider value
            if (parkingSlider != null)
            { parkingSlider.value = 0f; }
            isParkingCorrect = false;
        }


    }

    void UpdateScore(int points)
    {
        
        score -= points;
        if (score <= 0) { score = 0; }

        /*
        // Update the score display or send it wherever you need
        Debug.Log("Current score: " + score);
        */
    }

    private bool IsCarParkedCorrectly()
    {
        // Check if the car is correctly parked in the parking lot
        if (Vector2.Distance(transform.position, parkingLot.position) < 1.5f)
        {
            return true;
        }
        return false;
    }

    private float CalculateParkingPercentage()
    {
        // Get the size of the parking space
        Vector2 parkingLotSize = parkingLot.GetComponent<Renderer>().bounds.size;

        // Calculate the car's position relative to the parking space
        Vector2 carPosition = new Vector2(transform.position.x, transform.position.y);
        // Vector2 parkingLotPosition = new Vector2(parkingLot.position.x, parkingLot.position.y);
        Vector2 localCarPosition = parkingLot.InverseTransformPoint(transform.position);

        // Calculate the percentage overlap along X and Y axes
        float percentageX = Mathf.Clamp01(1f - Mathf.Abs(localCarPosition.x) / (parkingLotSize.x / 2f));
        float percentageY = Mathf.Clamp01(1f - Mathf.Abs(localCarPosition.y) / (parkingLotSize.y / 2f));

        // Get the maximum percentage
        float percentage = Mathf.Max(percentageX, percentageY);

        if (!isInsideParkingLot)
        {
            percentage = Mathf.MoveTowards(percentage, 0f, parkingSpeed * Time.deltaTime);
        }
        

        return percentage;

    }


    

}




