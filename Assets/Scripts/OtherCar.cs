/* This OtherCar same as Ai Car is an auto piloted car that moves
 * respecting the Pedestrian and the traffic light*/

using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.RuleTile.TilingRuleOutput;

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

    //
    public float minDistance = 2f;

    public Car carInFront;

    //safeDistance = 2f;

    private void Start()
    {
        Destroy(gameObject, 25f);
    }

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
            if (carInFront != null)
            {
                float distanceToCarInfront = Vector3.Distance(transform.position, carInFront.transform.position);

                // Calculate the distance between the car and the traffic light
                float distanceToTrafficLight = Vector3.Distance(transform.position, trafficLight.transform.position);

                // Check if the traffic light is red and the car is close to it
                if (distanceToCarInfront < minDistance || (trafficLight.indexOfCurrColor == 1 && distanceToTrafficLight < stopDistance))
                {
                    // Stop the car
                    moveSpeed = 0f;
                    return; // Exit the loop since the car has stopped
                }
            }


            /*if (carInFront != null)
            {
                float distanceToCarInfront = Vector3.Distance(transform.position, carInFront.transform.position);

                if (distanceToCarInfront < minDistance)
                {
                    moveSpeed = 0f;
                    return;
                }
            }*/



            // Continue moving at default speed
            moveSpeed = 2f;


            // Move the car
            //transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
        }
    }

    
}

    /*
    // Reference to the line renderer
    public LineRenderer lineRenderer;

    // Reference to the traffic lights at each intersection
    public OppositeTrafficLight trafficLight;

    // Speed of the car
    public float moveSpeed = 5f;

    // Distance at which the car stops near each traffic light
    public float stopDistance = 1f;

    // Update is called once per frame
    void Update()
    {
        // Check if there's a line renderer assigned and points on the line
        if (lineRenderer != null && lineRenderer.positionCount > 0)
        {
            // Loop through each point on the line renderer
            for (int i = 1; i < lineRenderer.positionCount - 1; i++)
            {
                // Get the current point on the line
                Vector3 point = lineRenderer.GetPosition(i);

                // Calculate the distance between the car and the current point
                //float distanceToPoint = Vector3.Distance(transform.position, point);

                //
                //float pointY = lineRenderer.GetPosition(i).y;

                // Check if the distance is less than the stop distance
                /*if (distanceToPoint < stopDistance)
                {
                    // Check if the car is close to any traffic light
                    foreach (OppositeTrafficLight trafficLight in trafficLights)
                    {
                        // Get the position of the traffic light
                        Vector3 trafficLightPosition = lineRenderer.GetPosition(trafficLight.pointIndex);

                        // Calculate the distance between the car and the traffic light
                        float distanceToTrafficLight = Vector3.Distance(transform.position, trafficLightPosition);*/

                // Check if the traffic light is red and the car is close to it
                /*
                if (trafficLight.indexOfCurrColor == 1 && Mathf.Abs(transform.position.y - point.y) < 1f)
                {
                    // Stop the car
                    moveSpeed = 0f;
                    return; // Exit the loop since the car has stopped
                }

            }
        }
        // Continue moving
        moveSpeed = 5f;
    }

}*/