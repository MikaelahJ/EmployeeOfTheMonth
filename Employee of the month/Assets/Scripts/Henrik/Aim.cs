using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Aim : MonoBehaviour
{
    [Range(5, 20)]
    public int rotationSpeed;

    private Vector3 mousePosition;
    private Vector3 previousMousePosition;
    private Vector2 mouseInput;
    private Vector2 previousDirection;
    private Vector2 aimDirection;
    private bool hasGamePad;


    private void Update()
    {
        if (hasGamePad)
        {
            StickAim();
        }
        else
        {
            MouseAim();
        }
    }


    public void MouseAim()
    {
        mousePosition = mouseInput;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        mousePosition.z = 0;
        mousePosition = mousePosition - transform.position;
        transform.up = Vector3.Lerp(previousMousePosition, mousePosition, rotationSpeed);

        transform.up = mousePosition;
        previousMousePosition = mousePosition;
    }

    public void StickAim()
    {
        transform.up = Vector2.Lerp(previousDirection, aimDirection, rotationSpeed * Time.deltaTime);
        previousDirection = transform.up;
    }

    //Used in the controllerinput script
    public void GetMouseInput(Vector3 input)
    {
        mouseInput = input;
    }
    public void SetAimStickInput(Vector2 controllerInput)
    {
        aimDirection = controllerInput;
        hasGamePad = true;
    }

    public void SetAimMouse(Vector3 mouseInput)
    {
        this.mouseInput = mouseInput;
        hasGamePad = false;
    }
}