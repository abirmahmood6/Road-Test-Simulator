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

    public float DestroyTime = 25f;

    public Car carInFront;

    //safeDistance = 2f;

    private void Start()
    {
        Destroy(gameObject, DestroyTime);
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


           
            // Continue moving at default speed
            moveSpeed = 2f;

        }
    }

    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the OtherCar collided with an object tagged as "Destination"
        if (collision.gameObject.CompareTag("Destination"))
        {
            // Instantiate explosion prefab at the current position of the OtherCar
            //Instantiate(explosionPrefab, transform.position, Quaternion.identity);

            // Destroy the OtherCar
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the OtherCar collided with an object tagged as "Destination"
        if (other.CompareTag("Destination"))
        {
            // Instantiate explosion prefab at the current position of the OtherCar
            //Instantiate(explosionPrefab, transform.position, Quaternion.identity);

            // Destroy the OtherCar
            Destroy(gameObject);
        }
    }
    */

}
