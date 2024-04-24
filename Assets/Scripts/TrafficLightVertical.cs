using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OppositeTrafficLight : MonoBehaviour
{
        public enum LightState { Red, Orange, Green }

        public SpriteRenderer redLight;
        public SpriteRenderer orangeLight;
        public SpriteRenderer greenLight;

        // Colors for each traffic light state
        public Color redColor = new Color(1f, 0f, 0f, 1f); // Red with full alpha
        public Color orangeColor = new Color(1f, 0.5f, 0f, 1f); // Orange with full alpha
        public Color greenColor = new Color(0f, 1f, 0f, 1f); // Green with full alpha

        // Alpha values for reduced visibility
        public float lowAlpha = 0.2f;

        // Current color of the traffic light
        public LightState currentColor { get; private set; }
        public int indexOfCurrColor { get; private set; }


        void Start()
        {
            // Set initial state of the traffic lights
            SetTrafficLight(LightState.Red);
            // Start automatic light changing
            StartCoroutine(ChangeLightAutomatically());
        }

        IEnumerator ChangeLightAutomatically()
        {
            while (true)
            {
            // Red light for 10 seconds (opposite of Green on the first light)
            yield return new WaitForSeconds(13f);
            SetTrafficLight(LightState.Green); // Green light for 7 seconds (opposite of Red on the first light)
            yield return new WaitForSeconds(10f);
            SetTrafficLight(LightState.Orange); // Orange light for 3 seconds (transition phase)
            yield return new WaitForSeconds(3f);
            SetTrafficLight(LightState.Red);
        }
        }

        void SetTrafficLight(LightState color)
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
