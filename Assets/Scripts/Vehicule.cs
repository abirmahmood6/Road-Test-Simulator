using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Vehicle : MonoBehaviour
{
    protected virtual void Start() { }

    protected virtual void Update()
    {
        HandleMovement();
    }

    protected abstract void HandleMovement();
}
