using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OppositeTrafficLight : BaseTrafficLight
{
    protected override void Start()
    {
        base.Start();
        SetTrafficLight(LightState.Red);
        StartCoroutine(ChangeLightAutomatically());
    }

    protected override IEnumerator ChangeLightAutomatically()
    {
        while (true)
        {
            yield return new WaitForSeconds(13f);
            SetTrafficLight(LightState.Green);
            yield return new WaitForSeconds(10f);
            SetTrafficLight(LightState.Orange);
            yield return new WaitForSeconds(3f);
            SetTrafficLight(LightState.Red);
        }
    }

}

