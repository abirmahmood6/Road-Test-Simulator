/*

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class Car : MonoBehaviour
{
    [SerializeField] float accelerationSpeed = 5f;
    [SerializeField] float maxSpeed = 10f;
    [SerializeField] float reverseSpeed = 3f;
    [SerializeField] float brakeSpeed = 5f;
    [SerializeField] float turnSpeed = 100f;
    [SerializeField] Slider parkingSlider;
    [SerializeField] Toggle[] carParkingToogle;
    [SerializeField] Transform parkingLot;

    private int score = 30;
    private float currentSpeed = 0f;
    private float currentTurn = 0f;
    private bool isParkingCorrect = false;
    private bool isInsideParkingLot = false;
    private float parkingSpeed = 0.5f;
    private int stopCount = 0, StopCountOnce = 0; 
    private bool isNearStopSign = false, isNearStopSignOnce = false; 

    public CollisionHandler collisionHandler;

    void Update()
    {
        CheckGameConditions();

        HandleMovement();
        HandleStopSigns();
    }

    void HandleMovement()
    {
        // Acceleration
        if (Input.GetKey(KeyCode.Space))
        {
            if (currentSpeed < maxSpeed)
                currentSpeed += accelerationSpeed * Time.deltaTime;
        }
        // Reverse
        else if (Input.GetKey(KeyCode.R))
        {
            if (currentSpeed > -maxSpeed / 2)
                currentSpeed -= reverseSpeed * Time.deltaTime;
        }
        // Brake
        else if (Input.GetKey(KeyCode.B))
        {
            if (currentSpeed > 0)
                currentSpeed -= brakeSpeed * Time.deltaTime;
            else if (currentSpeed < 0)
                currentSpeed += brakeSpeed * Time.deltaTime;
            else
                currentSpeed = 0;
        }
        else
        {
            if (currentSpeed > 0)
                currentSpeed -= brakeSpeed * Time.deltaTime;
            else if (currentSpeed < 0)
                currentSpeed += brakeSpeed * Time.deltaTime;
            else
                currentSpeed = 0;
        }

        // Apply speed and rotation
        transform.Translate(Vector3.up * currentSpeed * Time.deltaTime);
        float horizontalInput = Input.GetAxis("Horizontal");
        currentTurn = horizontalInput * turnSpeed * Time.deltaTime;
        transform.Rotate(Vector3.forward * -currentTurn);
    }

    void HandleStopSigns()
    {
        if ((isNearStopSign || isNearStopSignOnce) && Input.GetKeyDown(KeyCode.B))
        {
            stopCount++;
            StopCountOnce++;
        }
    }

    void CheckGameConditions()
    {
        if (AllTogglesOn())
            StartCoroutine(Passed(4));
        else if (score <= 0)
            StartCoroutine(Failed(4));
    }

    IEnumerator Failed(float delay)
    {
        yield return new WaitForSeconds(3);
        collisionHandler.displayMessageEvent.Invoke("Unfortunately, You failed the test!");
        yield return new WaitForSeconds(delay);
        UnityEditor.EditorApplication.isPlaying = false;
    }

    IEnumerator Passed(float delay)
    {
        yield return new WaitForSeconds(3);
        collisionHandler.displayMessageEvent.Invoke("Congratulations! You passed the test!");
        yield return new WaitForSeconds(delay);
        UnityEditor.EditorApplication.isPlaying = false;
    }

    bool AllTogglesOn()
    {
        foreach (Toggle toggle in carParkingToogle)
        {
            if (!toggle.isOn) return false;
        }
        return true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("StopSignTrigger"))
            isNearStopSign = true;
        else if (other.CompareTag("StopSignTriggerOnce"))
            isNearStopSignOnce = true;
        else if (other.CompareTag("ParkingSpace"))
            isInsideParkingLot = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("StopSignTrigger"))
        {
            isNearStopSign = false;
            if (stopCount != 2)
            {
                stopCount = 0;
                collisionHandler.UpdateScore(5);
                collisionHandler.displayMessageEvent.Invoke("You violated the Stop Sign. You should have stopped Twice (2x)");
            }
            else if (stopCount == 2)
                carParkingToogle[2].isOn = true;
        }
        else if (other.CompareTag("StopSignTriggerOnce"))
        {
            isNearStopSign = false;
            if (StopCountOnce != 1)
            {
                StopCountOnce = 0;
                collisionHandler.UpdateScore(5);
                collisionHandler.displayMessageEvent.Invoke("You violated the Stop Sign. You should have stopped Once");
            }
            else if (StopCountOnce == 1)
                carParkingToogle[4].isOn = true;
        }
        else if (other.CompareTag("ParkingSpace"))
        {
            if (parkingSlider != null)
                parkingSlider.value = 0f;
            isParkingCorrect = false;
        }
    }
}
*/
