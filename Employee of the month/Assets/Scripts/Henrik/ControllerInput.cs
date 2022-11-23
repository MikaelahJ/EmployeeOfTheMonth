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

    private Movement playerMovement;
    private MouseAim mouseAim;
    private StickAim stickAim;
    private Fire fire;



    private void Start()
    {
        playerMovement = GetComponent<Movement>();
        mouseAim = GetComponent<MouseAim>();
        stickAim = GetComponent<StickAim>();
        fire = GetComponentInChildren<Fire>();
    }


    public void GetLeftStick(InputAction.CallbackContext input)
    {
        playerMovement.GetRightStickInput(input.ReadValue<Vector2>());
    }

    public void GetRightStick(InputAction.CallbackContext input)
    {
        stickAim.GetRightStick(input.ReadValue<Vector2>());
    }

    public void GetMousePosition(InputAction.CallbackContext input)
    {
        mouseAim.GetMouseInput(input.ReadValue<Vector2>());
    }

    public void OnRun(InputAction.CallbackContext input)
    {
        if (input.started)
        {
            playerMovement.GetRunButtonInput(true);
        }

        if (input.canceled)
        {
            playerMovement.GetRunButtonInput(false);
        }
    }

    public void OnShoot(InputAction.CallbackContext input)
    {
        if (input.started)
        {
           fire.GetFireButtonInput(true);
        }
        if(input.canceled)
        {
            fire.GetFireButtonInput(false);
        }
    }
}
