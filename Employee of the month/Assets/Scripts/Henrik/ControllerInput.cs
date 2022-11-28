using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ControllerInput : MonoBehaviour
{
    public GameObject playerPrefab;

    private GameObject player;
    private Movement playerMovement;
    private Aim aim;
    private Fire fire;
    private PlayerInput playerInput;


    private void Awake()
    {
        player = Instantiate(playerPrefab, SpawnManager.instance.GetRandomSpawnPoint(), transform.rotation);
        playerMovement = player.GetComponent<Movement>();
        aim = player.GetComponent<Aim>();
        fire = player.GetComponentInChildren<Fire>();
        playerInput = GetComponent<PlayerInput>();
        if (SceneManager. == "CharacterSelect")
        {
            playerInput.SwitchCurrentActionMap("UI");
        }
        else
            playerInput.SwitchCurrentActionMap("Player");
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
        if(input.canceled)
        {
            fire.GetFireButtonInput(false);
        }
    }
}
