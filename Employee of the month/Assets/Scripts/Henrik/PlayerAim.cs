using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    [Range(0, 20)]
    public int rotationSpeed;

    private GetControllerInput input;
    private Vector2 inputVector;
    private Vector2 previousRotation;
    private Vector2 mouseInputVector;
    private Vector2 previousMouseRotation;


    void Start()
    {
        input = GetComponent<GetControllerInput>();
        previousRotation = Vector2.zero;
        previousMouseRotation = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        AimDirection();
        //MouseAimDirection();
    }

    public void AimDirection()
    {
        inputVector = input.RighStick;

        transform.up = Vector2.Lerp(previousRotation, inputVector, rotationSpeed * Time.deltaTime);
        previousRotation = transform.up;
    }

    //public void MouseAimDirection()
    //{
    //    //mouseInputVector = input.MousePosition;

    //    if(input.MousePosition.x != 0)
    //    {
    //        mouseInputVector.x = input.MousePosition.x;
    //    }

    //    if(input.MousePosition.y != 0)
    //    {
    //        mouseInputVector.y = input.MousePosition.y;
    //    }
    //    transform.up = Vector2.Lerp(previousMouseRotation, mouseInputVector * 10, rotationSpeed * Time.deltaTime);
    //    previousMouseRotation = transform.up;
    //    Debug.Log(input.MousePosition);
    //}
        
}
