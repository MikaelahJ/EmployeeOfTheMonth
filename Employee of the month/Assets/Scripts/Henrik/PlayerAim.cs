using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    [Range(0, 20)]
    public int rotationSpeed;

    private ControllerInput input;
    private Vector2 inputVector;
    private Vector2 previousRotation;


    void Start()
    {
        input = GetComponent<ControllerInput>();
        previousRotation = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        AimDirection();
    }

    public void AimDirection()
    {
        inputVector = input.RighStick;

        transform.up = Vector2.Lerp(previousRotation, inputVector, rotationSpeed * Time.deltaTime);
        previousRotation = transform.up;
    }
}
