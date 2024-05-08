using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedestrian : MonoBehaviour
{
    public enum State { Crossing, Waiting }

    // Reference to the traffic light
    public OppositeTrafficLight trafficLight;

    // Speed of the people crossing
    public float moveSpeed = 2f;

    // Movement bounds
    public float leftBound = -5f;
    public float rightBound = 5f;

    // Current state of the people
    private State currentState = State.Waiting;
    private bool movingRight = true;

    //Timer for destroying the object
    private float destroyTimer = 0f;
    private bool shouldDestroy = false;

    // Update is called once per frame
    void Update()
    {
        // Check the state of the traffic light
        if (trafficLight != null)
        {
            if (trafficLight.indexOfCurrColor == 3 && currentState == State.Waiting)
            {
                // If the light is green and people are waiting, start crossing
                currentState = State.Crossing;
            }
            else if (trafficLight.indexOfCurrColor == 1 && currentState == State.Crossing)
            {
                // If the light is not green and people are crossing, stop them
                currentState = State.Waiting;

                //Start the timer for destruction
                shouldDestroy = true;
                destroyTimer = 0f;
            }
        }

        // Move the people
        if (currentState == State.Crossing)
        {
            if (movingRight)
            {
                transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
            }

            // Check if the people reached the left or right bounds
            if (transform.position.x >= rightBound)
            {
                movingRight = false; // Change direction to move left
            }
            else if (transform.position.x <= leftBound)
            {
                movingRight = true; // Change direction to move right
            }
        }

        /*if (shouldDestroy)
        {
            destroyTimer += Time.deltaTime;
            if (destroyTimer >= 0f)
            {
                Destroy(gameObject);
            }
        }*/
    }
}
