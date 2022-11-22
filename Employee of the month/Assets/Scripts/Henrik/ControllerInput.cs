using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerInput : MonoBehaviour
{
    [SerializeField]
    private Vector2 leftStick;
    [SerializeField]
    private Vector2 rightStick;
    [SerializeField]
    private Vector2 mousePosition;

    private bool isRunning;
    private bool hasFired;

    public Vector2 Leftstick { get { return leftStick; } }
    public Vector2 RighStick { get { return rightStick; } }
    public Vector2 MousePosition { get { return mousePosition; } }
    public bool IsRunning { get { return isRunning; } }
    public bool HasFired { get { return hasFired; } set { hasFired = value; } }
  

    public void GetLeftStick(InputAction.CallbackContext input)
    {
        leftStick = input.ReadValue<Vector2>();
    }

    public void GetRightStick(InputAction.CallbackContext input)
    {
        rightStick = input.ReadValue<Vector2>();
    }

    public void GetMousePosition(InputAction.CallbackContext input)
    {
        mousePosition = input.ReadValue<Vector2>();
    }

    public void OnRun(InputAction.CallbackContext input)
    {
        if (input.started)
        {
            isRunning = true;
        }

        if(input.canceled)
        {
            isRunning = false;
        }
    }

    public void OnShoot(InputAction.CallbackContext input)
    {
        if(input.started)
        {
            hasFired = true;
        }

        if(input.canceled)
        {
            hasFired = false;
        }
    }
}
