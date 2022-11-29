using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ControllerInput : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject playerHUD;
    public GameObject cursorPrefab;

    private GameObject player;
    private Movement playerMovement;
    private Aim aim;
    private Fire fire;
    private PlayerInput playerInput;

    private GameObject cursorObject;
    private Cursor cursor;

    public List<string> players;

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
            SpawnPlayerHUD(player);
        }
        Debug.Log(playerInput.currentActionMap);
    }

    private void SpawnPlayerHUD(GameObject player)
    {
        GameObject canvas = GameObject.Find("Canvas");
        GameObject hud = Instantiate(playerHUD, canvas.transform);
        RectTransform rectT = hud.GetComponent<RectTransform>();
        float spacing = 1.2f; // 1 puts them next to each other
        int offset = playerInput.playerIndex - 2; // offset from bottom middle
        rectT.localPosition = new Vector3(rectT.rect.width * rectT.localScale.x * spacing * offset, rectT.localPosition.y, rectT.localPosition.z);

        player.GetComponentInChildren<WeaponController>().itemHolder = hud.GetComponentInChildren<UIItemHolder>();
        player.GetComponentInChildren<Fire>().ammoCounter = hud.GetComponent<UIAmmoCounter>();
        player.GetComponent<HasHealth>().healthbar = hud.GetComponentInChildren<UIHealthbar>();
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
