using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherCar : MonoBehaviour
{
    // Reference to the line renderer
    public LineRenderer lineRenderer;

    // Reference to the traffic light
    public OppositeTrafficLight[] trafficLights;

    // Speed of the car
    public float moveSpeed = 5f;

    // Current index on the line
    private int currentIndex = 0;

    //
    public float stopDistance = 1f;

    // Update is called once per frame
    void Update()
    {
        // Check if there's a line renderer assigned and points on the line
        if (lineRenderer != null && lineRenderer.positionCount > 0)
        {
            // Get the next point on the line
            Vector3 targetPosition = lineRenderer.GetPosition(currentIndex);

            // Move towards the next point
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Check if the car has reached the current point
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                // Move to the next point on the line
                currentIndex = (currentIndex + 1) % lineRenderer.positionCount;
            }
        }

        //foreach (OppositeTrafficLight trafficLight in trafficLights)
        foreach (OppositeTrafficLight trafficLight in trafficLights)
        {
            // Calculate the distance between the car and the traffic light
            float distanceToTrafficLight = Vector3.Distance(transform.position, trafficLight.transform.position);

            // Check if the traffic light is red and the car is close to it
            if (trafficLight.indexOfCurrColor == 1 && distanceToTrafficLight < stopDistance)
            {
                // Stop the car
                moveSpeed = 0f;
                return; // Exit the loop since the car has stopped
            }
        }

        moveSpeed = 2f;
    }
}
