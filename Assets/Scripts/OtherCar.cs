using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherCar : MonoBehaviour
{
    // Reference to the line renderer
    public LineRenderer lineRenderer;

    // Speed of the car
    public float moveSpeed = 5f;

    // Current index on the line
    private int currentIndex = 0;

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
    }
}
