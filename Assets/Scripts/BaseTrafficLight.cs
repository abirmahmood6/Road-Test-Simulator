using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseTrafficLight : MonoBehaviour
{
    public enum LightState { Red, Orange, Green }

    public SpriteRenderer redLight;
    public SpriteRenderer orangeLight;
    public SpriteRenderer greenLight;

    public Color redColor = new Color(1f, 0f, 0f, 1f); // Red with full alpha
    public Color orangeColor = new Color(1f, 0.5f, 0f, 1f); // Orange with full alpha
    public Color greenColor = new Color(0f, 1f, 0f, 1f); // Green with full alpha

    public float lowAlpha = 0.2f;

    public LightState currentColor { get; private set; }
    public int indexOfCurrColor { get; private set; }

    protected virtual void Start()
    {
        // Initial setup can be done here or in the derived classes
    }

    protected abstract IEnumerator ChangeLightAutomatically();

    protected virtual void SetTrafficLight(LightState color)
    {
        currentColor = color;

        switch (color)
        {
            case LightState.Red:
                redLight.color = redColor;
                orangeLight.color = new Color(orangeColor.r, orangeColor.g, orangeColor.b, lowAlpha);
                greenLight.color = new Color(greenColor.r, greenColor.g, greenColor.b, lowAlpha);
                indexOfCurrColor = 1;
                break;
            case LightState.Orange:
                redLight.color = new Color(redColor.r, redColor.g, redColor.b, lowAlpha);
                orangeLight.color = orangeColor;
                greenLight.color = new Color(greenColor.r, greenColor.g, greenColor.b, lowAlpha);
                indexOfCurrColor = 2;
                break;
            case LightState.Green:
                redLight.color = new Color(redColor.r, redColor.g, redColor.b, lowAlpha);
                orangeLight.color = new Color(orangeColor.r, orangeColor.g, orangeColor.b, lowAlpha);
                greenLight.color = greenColor;
                indexOfCurrColor = 3;
                break;
            default:
                Debug.LogError("Unknown traffic light color!");
                break;
        }
    }
}


