using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ControllerInput : MonoBehaviour
{
    [SerializeField] private List<GameObject> characters = new List<GameObject>();
    public GameObject playerPrefab;
    public GameObject playerHUD;
    public GameObject cursorPrefab;

    private GameObject player;
    private Movement playerMovement;
    private Aim aim;
    private Fire fire;
    private PlayerInput playerInput;
    public Sprite sprite;
    public int spriteIndex;

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
            cursorObject.name = "P" + playerInput.playerIndex.ToString();
            cursor = cursorObject.GetComponent<Cursor>();

            GameManager.Instance.playersCount += 1;
        }
        else
        {
            playerInput.SwitchCurrentActionMap("Player");
            player = Instantiate(playerPrefab, SpawnManager.instance.GetRandomSpawnPoint(), transform.rotation);

            if (GameManager.Instance.playersCount != 0)
            {
                spriteIndex = GameManager.Instance.players["P" + (playerInput.playerIndex).ToString()];
                SetCharacter();
            }
            else
            {
                SetCharacterTestScenes();
            }
            player.GetComponent<HasHealth>().playerIndex = playerInput.playerIndex;
            playerMovement = player.GetComponent<Movement>();
            aim = player.GetComponent<Aim>();
            fire = player.GetComponentInChildren<Fire>();

            SpawnPlayerHUD(player);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void SetCharacter()
    {
        Debug.Log(spriteIndex);
        Debug.Log("hejsan");
        switch (spriteIndex)
        {
            case 1:
                Instantiate(characters[0], player.transform);
                break;
            case 2:
                Instantiate(characters[1], player.transform);
                break;
            case 3:
                Instantiate(characters[2], player.transform);
                break;
            case 4:

                break;
        }
    }

    private void SetCharacterTestScenes()
    {
        Instantiate(characters[0], player.transform);
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
        player.GetComponentInChildren<Fire>().ammoCounter = hud.GetComponentInChildren<UIAmmoCounter>();
        player.GetComponent<HasHealth>().healthbar = hud.GetComponentInChildren<UIHealthbar>();
    }
    public void OnClick()
    {
        cursor.Pressed();
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
