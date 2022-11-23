using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseAim : MonoBehaviour
{
    [Range(5, 20)]
    public int rotationSpeed;

    private Vector3 mousePosition;
    private Vector3 previousMousePosition;
    private Vector2 mouseInput;


    private void Update()
    {
        MouseAimDirection();
    }


    public void MouseAimDirection()
    {
        mousePosition = mouseInput;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        mousePosition.z = 0;
        mousePosition = mousePosition - transform.position;
        transform.up = Vector3.Lerp(previousMousePosition, mousePosition, rotationSpeed);

        transform.up = mousePosition;
        previousMousePosition = mousePosition;
    }

    //Used in the controllerinput script
    public void GetMouseInput(Vector3 input)
    {
        mouseInput = input;
    }
}
