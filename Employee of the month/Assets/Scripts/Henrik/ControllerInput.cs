using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ControllerInput : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject cursorPrefab;

    private GameObject player;
    private Movement playerMovement;
    private Aim aim;
    private Fire fire;
    private PlayerInput playerInput;

    private GameObject cursorObject;
    private Cursor cursor;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        if (SceneManager.GetActiveScene().name == "CharacterSelect")
        {
            playerInput.SwitchCurrentActionMap("UI");
            cursorObject = Instantiate(cursorPrefab, Vector2.zero, cursorPrefab.transform.rotation);
            cursor = cursorObject.GetComponent<Cursor>();
        }
        else
        {
            playerInput.SwitchCurrentActionMap("Player");
            player = Instantiate(playerPrefab, SpawnManager.instance.GetRandomSpawnPoint(), transform.rotation);
            playerMovement = player.GetComponent<Movement>();
            aim = player.GetComponent<Aim>();
            fire = player.GetComponentInChildren<Fire>();  
        }
    }

    public void GetLeftStick(InputAction.CallbackContext input)
    {
        playerMovement.GetLeftStickInput(input.ReadValue<Vector2>());
    }

    public void GetRightStick(InputAction.CallbackContext input)
    {

        if (playerInput.currentControlScheme == "Gamepad")
        {
            aim.SetAimStickInput(input.ReadValue<Vector2>());
        }
        else
        {
            aim.SetAimMouse(input.ReadValue<Vector2>());
        }
    }

    public void GetMousePosition(InputAction.CallbackContext input)
    {
        aim.GetMouseInput(input.ReadValue<Vector2>());
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
        if (input.canceled)
        {
            fire.GetFireButtonInput(false);
        }
    }

    public void MoveCursor(InputAction.CallbackContext input)
    {
        if (playerInput.currentControlScheme == "Gamepad")
        {
            cursor.SetAimStickInput(input.ReadValue<Vector2>());
        }
        else
        {
            cursor.SetMouseAim(input.ReadValue<Vector2>());
        }
    }
}
