using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLight : MonoBehaviour
{
    public enum LightColor { Red, Orange, Green }

    public SpriteRenderer redLight;
    public SpriteRenderer orangeLight;
    public SpriteRenderer greenLight;

    // Colors for each traffic light state
    public Color redColor = new Color(1f, 0f, 0f, 1f); // Red with full alpha
    public Color orangeColor = new Color(1f, 0.5f, 0f, 1f); // Orange with full alpha
    public Color greenColor = new Color(0f, 1f, 0f, 1f); // Green with full alpha

    // Alpha values for reduced visibility
    public float lowAlpha = 0.2f;

    void Start()
    {
        // Set initial state of the traffic lights
        SetTrafficLight(LightColor.Green); // Start with Green
        // Start automatic light changing
        StartCoroutine(ChangeLightAutomatically());
    }

    IEnumerator ChangeLightAutomatically()
    {
        while (true)
        {
            // Green light for 10 seconds
            yield return new WaitForSeconds(10f);
            // Change to orange for 3 seconds
            SetTrafficLight(LightColor.Orange);
            yield return new WaitForSeconds(3f);
            // Change to red for 10 seconds
            SetTrafficLight(LightColor.Red);
            yield return new WaitForSeconds(10f);
            // Loop back to green
            SetTrafficLight(LightColor.Green);
        }
    }

    void SetTrafficLight(LightColor color)
    {
        switch (color)
        {
            case LightColor.Green:
                greenLight.color = greenColor;
                orangeLight.color = new Color(orangeColor.r, orangeColor.g, orangeColor.b, lowAlpha);
                redLight.color = new Color(redColor.r, redColor.g, redColor.b, lowAlpha);
                break;
            case LightColor.Orange:
                greenLight.color = new Color(greenColor.r, greenColor.g, greenColor.b, lowAlpha);
                orangeLight.color = orangeColor;
                redLight.color = new Color(redColor.r, redColor.g, redColor.b, lowAlpha);
                break;
            case LightColor.Red:
                greenLight.color = new Color(greenColor.r, greenColor.g, greenColor.b, lowAlpha);
                orangeLight.color = new Color(orangeColor.r, orangeColor.g, orangeColor.b, lowAlpha);
                redLight.color = redColor;
                break;
            default:
                Debug.LogError("Unknown traffic light color!");
                break;
        }
    }
}
