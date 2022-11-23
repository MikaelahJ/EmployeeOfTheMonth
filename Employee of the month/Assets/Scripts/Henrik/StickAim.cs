using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickAim : MonoBehaviour
{
    [Range(0, 20)]
    public int rotationSpeed;

    private Vector2 rightStickVector;
    private Vector2 previousRotation;


    void Start()
    {
        previousRotation = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        AimDirection();
    }

    public void AimDirection()
    {
        transform.up = Vector2.Lerp(previousRotation, rightStickVector, rotationSpeed * Time.deltaTime);
        previousRotation = transform.up;
    }

    //Used in controllerinput script
    public void GetRightStick(Vector2 input)
    {
        rightStickVector = input;
    }
}
