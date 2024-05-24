/* This Car class represent the player car which takes input from the keyboard
   and move the car accordingly.
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Car : Vehicle
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

    private int stopCount = 0, StopCountOnce = 0; // Count of stop sign encounters
    private bool isNearStopSign = false, isNearStopSignOnce = false; // Flag to indicate if the player car is near a stop sign


    public Collider2D parkingSpaceCollider;

    protected override void Start()
    {
        base.Start();
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

        CheckGameConditions();


        HandleMovement();
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

            IsInsideParkingLot = true;

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

            else if (StopCountOnce == 1)
            {
                carParkingToogle[4].isOn = true;
            }
        }

        else if (other.CompareTag("ParkingSpace"))
        {
            IsInsideParkingLot = false;
            FindObjectOfType<ParkingManager>().ExitParkingLot(); // Reset the parking logic
        }

    }

    void UpdateScore(int points)
    {
        score -= points;
        if (score <= 0) { score = 0; }
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

    bool AllTogglesOn()
    {
        foreach (Toggle toggle in carParkingToogle)
        {
            if (!toggle.isOn) return false;
        }
        return true;
    }

    void CheckGameConditions()
    {
        if (AllTogglesOn())
        {
            StartCoroutine(Passed(4));
        }
        else if (score <= 0)
        {
            StartCoroutine(Failed(4));
        }
    }

    IEnumerator Failed(float delay)
    {
        //StartCoroutine(delay(3));
        yield return new WaitForSeconds(3);
        displayMessageEvent.Invoke("Unfortunately, You failed the test!");
        yield return new WaitForSeconds(delay);
        UnityEditor.EditorApplication.isPlaying = false;
    }

    IEnumerator Passed(float delay)
    {
        yield return new WaitForSeconds(3);
        displayMessageEvent.Invoke("Congratulations! You passed the test!");
        yield return new WaitForSeconds(delay);
        UnityEditor.EditorApplication.isPlaying = false;
    }

    protected override void HandleMovement()
    {
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
    }

    public void DisplayMessage(string message)
    {
        displayMessageEvent.Invoke(message);
    }

    public bool IsInsideParkingLot { get; set; }
}




