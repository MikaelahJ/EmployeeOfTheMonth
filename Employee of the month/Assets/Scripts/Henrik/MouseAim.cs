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
    private ControllerInput input;



    private void Start()
    {
        input = GetComponent<ControllerInput>();
    }

    private void Update()
    {
        MouseAimDirection();
    }

    public void MouseAimDirection()
    {
        mousePosition = input.MousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        mousePosition.z = 0;
        mousePosition = mousePosition - transform.position;
        transform.up = Vector3.Lerp(previousMousePosition, mousePosition, rotationSpeed);

        previousMousePosition = mousePosition;
    }
}
