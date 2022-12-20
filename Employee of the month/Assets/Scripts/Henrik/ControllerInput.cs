using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;

public class ControllerInput : MonoBehaviour
{
    [SerializeField] private List<Sprite> cursorSprites = new List<Sprite>();
    [SerializeField] private List<GameObject> characters = new List<GameObject>();
    [SerializeField] private List<GameObject> healthbars = new List<GameObject>();
    [SerializeField] private List<GameObject> playerHUDs = new List<GameObject>();
    public GameObject playerPrefab;
    public GameObject cursorPrefab;
    public GameObject healthbarPrefab;
    public GameObject roomSpriteMaskHolder;
    public GameObject playerHighlightCircle;
    public GameObject whichPlayerArrow;

    private GameObject player;
    private Movement playerMovement;
    private Aim aim;
    private Fire fire;
    private GameObject healthbar;

    public PlayerInput playerInput;
    public Sprite sprite;
    public int spriteIndex;

    private GameObject playerSprite;
    private GameObject cursorObject;
    private WeaponController weaponController;
    private Cursor cursor;
    private List<Color32> pColors = new List<Color32>();

    public List<string> players;

    [SerializeField] private GameObject vent;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        pColors.Add(new Color32(255, 146, 0, 255)); // P1 Orange
        pColors.Add(new Color32(169, 0, 255, 255)); // P2 Purple
        pColors.Add(new Color32(0, 255, 109, 255)); // P3 Green
        pColors.Add(new Color32(0, 192, 255, 255)); // P4 Blue

        playerInput = GetComponent<PlayerInput>();
        GameManager.Instance.playerControllers.Add(gameObject);

        GameManager.Instance.playersCount += 1;

        if (SceneManager.GetActiveScene().name == "CharacterSelect")
        {
            LoadCharacterSelect();
        }
        else
        {
            LoadGame();
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
        {

        }
        else if (scene.name == "CharacterSelect")
        {
            LoadCharacterSelect();
        }
        else if (scene.name == "EndGame")
        {
            LoadCursors();
        }
        else if (scene.name != "LoadingScene")
        {
            LoadGame();
        }
    }

    private void LoadCharacterSelect()
    {
        playerInput.SwitchCurrentActionMap("UI");
        GameManager.Instance.playersChosen = 0;
        cursorObject = Instantiate(cursorPrefab, Vector3.zero, cursorPrefab.transform.rotation);
        cursorObject.name = "P" + playerInput.playerIndex.ToString();
        cursor = cursorObject.GetComponent<Cursor>();


        //Set Cursor color
        //cursor.col = pColors[playerInput.playerIndex];
        cursor.sprite = cursorSprites[playerInput.playerIndex];
        //Sets the index of the player
        cursor.playerIndex = playerInput.playerIndex;
    }

    public void LoadCursors()
    {
        playerInput.SwitchCurrentActionMap("UI");

        if (GameManager.Instance.playersCount != 0)
        {
            cursorObject = Instantiate(cursorPrefab, Vector3.zero, cursorPrefab.transform.rotation);
            cursorObject.name = "P" + playerInput.playerIndex.ToString();
            cursor = cursorObject.GetComponent<Cursor>();

            cursor.sprite = cursorSprites[playerInput.playerIndex];
            cursor.playerIndex = playerInput.playerIndex;
        }
        else
        {
            SetCursorTestScenes();
        }
    }

    public void DestroyCursor()
    {
        Destroy(cursorObject);
        playerInput.SwitchCurrentActionMap("Player");
    }

    private void LoadGame()
    {
        playerInput = GetComponent<PlayerInput>();
        playerInput.SwitchCurrentActionMap("Player");
        SpawnPlayer();
        LoadHealthBar();
        

        if (GameManager.Instance.playersChosen != 0)
        {
            spriteIndex = GameManager.Instance.players["P" + (playerInput.playerIndex).ToString()];
            SetCharacter();
        }
        else
        {
            SetCharacterTestScenes();
        }

        SpawnSpriteMask();
        Camera.main.GetComponent<CameraController>().AddCameraTracking(player);
    }

    private void SetCharacter()
    {
        Debug.Log("hejsan");
        switch (spriteIndex)
        {
            case 1:
                playerSprite = Instantiate(characters[0], player.transform);
                playerSprite.name = "Character 1";
                break;
            case 2:
                playerSprite = Instantiate(characters[1], player.transform);
                playerSprite.name = "Character 2";
                break;
            case 3:
                playerSprite = Instantiate(characters[2], player.transform);
                playerSprite.name = "Character 3";
                break;
            case 4:
                playerSprite = Instantiate(characters[3], player.transform);
                playerSprite.name = "Character 4";
                break;
        }
        LoadPlayerChildScripts();
    }

    private void SetCharacterTestScenes()
    {
        spriteIndex = 1;
        playerSprite = Instantiate(characters[0], player.transform);
        playerSprite.name = "Character 1";
        LoadPlayerChildScripts();
    }
    private void SetCursorTestScenes()
    {
        Instantiate(cursorPrefab, Vector3.zero, cursorPrefab.transform.rotation);
    }

    private void SpawnPlayer()
    {
        player = Instantiate(playerPrefab, SpawnManager.instance.GetRandomSpawnPoint(), transform.rotation);
        player.name = "P" + (playerInput.playerIndex + 1).ToString() + " Player";
        playerMovement = player.GetComponent<Movement>();
        aim = player.GetComponent<Aim>();

        player.GetComponent<Movement>().animator = player.GetComponent<Animator>();

        player.GetComponent<HasHealth>().playerIndex = playerInput.playerIndex;
        player.GetComponent<HasHealth>().animator = player.GetComponent<Animator>();

        GameObject circle = Instantiate(playerHighlightCircle, player.transform);
        circle.GetComponent<SpriteRenderer>().color = pColors[playerInput.playerIndex];

        //GameObject whichPlayer = Instantiate(whichPlayerArrow, player.transform.position, Quaternion.identity);
        //foreach (var sprite in whichPlayer.GetComponentsInChildren<SpriteRenderer>())
        //{
        //    sprite.sprite = cursorSprites[playerInput.playerIndex];
        //}
    }

    private void LoadHealthBar()
    {
        healthbar = Instantiate(healthbars[playerInput.playerIndex], player.transform);
        healthbar.transform.SetParent(player.transform);
        healthbar.transform.position = player.transform.position;

        player.GetComponent<HasHealth>().healthbarAnimator = healthbar.GetComponent<Animator>();
    }

    private void LoadPlayerChildScripts()
    {
        player.GetComponentInChildren<WeaponController>().itemHolder = player.GetComponentInChildren<UIItemHolder>();
        fire = playerSprite.GetComponentInChildren<Fire>();
        weaponController = player.GetComponentInChildren<WeaponController>();
        player.GetComponent<HasHealth>().animator = playerSprite.GetComponent<Animator>();
        player.GetComponent<Movement>().animator = playerSprite.GetComponent<Animator>();
    }

    private void SpawnPlayerHUD(GameObject player)
    {
        GameObject canvas = GameObject.Find("Canvas");
        GameObject hud = Instantiate(playerHUDs[playerInput.playerIndex], canvas.transform);

        RectTransform rectT = hud.GetComponent<RectTransform>();
        float spacing = 1f; // 1 puts them next to each other
        int offset = playerInput.playerIndex - 2; // offset from bottom middle
        rectT.localPosition = new Vector3(rectT.rect.width * rectT.localScale.x * spacing * offset, rectT.localPosition.y, rectT.localPosition.z);

        player.GetComponentInChildren<WeaponController>().itemHolder = player.GetComponentInChildren<UIItemHolder>();
        //player.GetComponentInChildren<Fire>().ammoCounter = hud.GetComponentInChildren<UIAmmoCounter>();
        //player.GetComponent<HasHealth>().healthbar = hud.GetComponentInChildren<UIHealthbar>();
    }

    public void SpawnSpriteMask()
    {
        //playerSprite.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        //player.GetComponent<SortingGroup>().sortingLayerID = SortingLayer.NameToID("Player " + (spriteIndex));
        GameObject roomMask = Instantiate(roomSpriteMaskHolder);
        roomMask.GetComponent<RoomMaskManager>().layerName = "Character " + (spriteIndex);
    }

    public void OnClick()
    {
        if (cursor == null) { return; }
        cursor.Pressed();
    }
    public void GetLeftStick(InputAction.CallbackContext input)
    {
        if (playerMovement == null) { return; }
        playerMovement.GetLeftStickInput(input.ReadValue<Vector2>());
    }

    public void GetRightStick(InputAction.CallbackContext input)
    {
        if (aim == null) { return; }
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
        if (aim == null) { return; }
        aim.GetMouseInput(input.ReadValue<Vector2>());
    }

    //public void OnRun(InputAction.CallbackContext input)
    //{
    //    if (playerMovement == null) { return; }
    //    if (input.started)
    //    {
    //        playerMovement.GetRunButtonInput(true);
    //    }

    //    if (input.canceled)
    //    {
    //        playerMovement.GetRunButtonInput(false);
    //    }
    //}

    public void OnShoot(InputAction.CallbackContext input)
    {
        if (fire == null) { return; }
        if (input.started)
        {
            fire.GetFireButtonInput(true);
        }
        if (input.canceled)
        {
            fire.GetFireButtonInput(false);
        }
    }

    public void OnDiscard(InputAction.CallbackContext input)
    {
        if (input.performed)
        {
            weaponController.RemoveAllItems();
        }
    }

    public void MoveCursor(InputAction.CallbackContext input)
    {
        if (cursor == null) { return; }
        if (playerInput.currentControlScheme == "Gamepad")
        {
            cursor.SetAimStickInput(input.ReadValue<Vector2>());
        }
        else
        {
            cursor.SetMouseAim(input.ReadValue<Vector2>());
        }
    }

    public void Vent()
    {
        //   vent.GetComponent<Vent>().WhereToGo(player);
    }


    //for testing
    public void KillSelf()
    {
        player.GetComponent<HasHealth>().LoseHealth(100);
    }

    public void Pause(InputAction.CallbackContext input)
    {
        if (!input.performed) { return; }

        if (GameManager.Instance.isPaused)
        {
            GameManager.Instance.UnpauseGame();
        }
        else
        {
            GameManager.Instance.PauseGame();
        }
    }

    public void EnableAim(bool enable)
    {
        if (aim != null)
        {
            aim.enabled = enable;
        }
    }

    public GameObject GetPlayerSprite()
    {
        return playerSprite;
    }
}
