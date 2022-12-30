using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class Cursor : MonoBehaviour
{
    [SerializeField] public GameObject selectedCharacterBall;
    public float cursorSpeed;

    private Vector2 mouseInput;
    private Vector3 mousePosition;
    private Vector3 stickInput;
    private bool hasGamepad;
    private bool pressed;
    private GameObject selected;
    private GameObject selectedFrame;
    private Collider2D collidedObject;
    private bool canSelect;
    private int selectIndex;

    public Color32 col;
    public int playerIndex;
    public Sprite sprite;
    public GameObject controller;

    private void Awake()
    {
        //Invoke("Start", 0.01f);
    }

    private void Start()
    {
        Vector3 spawnpoint = Camera.main.transform.position;
        spawnpoint.z = 0;
        transform.position = spawnpoint;

        sprite =  GetComponent<SpriteRenderer>().sprite = sprite;
        selectedCharacterBall.GetComponent<SpriteRenderer>().color = GetComponent<SpriteRenderer>().color;
    }

    private void Update()
    {
        if (hasGamepad)
        {
            StickPosition();
        }
        else
        {
            MousePosition();
        }

        if(canSelect)
        {
            CharacterSelection(collidedObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        canSelect = true;
        pressed = false;
        collidedObject = collision;
    }

    public void CharacterSelection(Collider2D collision)
    {
        if (SceneManager.GetActiveScene().name == "CharacterSelect")
        {
            if (!pressed) { return; }

            if (collision.gameObject.CompareTag("StartButton"))
            {
                if (GameManager.Instance.playersCount != GameManager.Instance.playersChosen)
                {
                    GameObject mustChoose = GameObject.Find("StartGameText");
                    mustChoose.GetComponent<TextMeshProUGUI>().text = "Everyone must choose a character";
                    Invoke(nameof(ResetStartText), 1);
                    Debug.Log("Everyone must select a character");
                }
                else
                {
                    GameModeManager.Instance.CreateTeams();
                    GameManager.Instance.LoadScene("LoadingScene");
                }
            }

            if (collision.gameObject.CompareTag("Free"))
            {
                //är denna spelare kopplad till en karaktär
                if (GameManager.Instance.players.ContainsKey(this.name))
                {
                    //är karaktären inte den man nyss klickade på
                    if (GameManager.Instance.players[this.name] != Convert.ToInt32(collision.gameObject.name))
                    {
                        //hitta den man valt innan och ändra tag på den
                        GameObject.Find(GameManager.Instance.players[this.name].ToString()).tag = "Free";

                    }
                }
                //Deactivate previous button
                GameModeManager.Instance.ActivateTeamSelectButton(selectIndex, false, null, controller);
                selectIndex = Convert.ToInt32(collision.gameObject.name) - 1;
                //Activate new selected
                GameModeManager.Instance.ActivateTeamSelectButton(selectIndex, true, this.name, controller);
                
                
                collision.tag = "Selected";
                //SetSelectedBall(collision);
                SetFrameColor(collision);
                GameManager.Instance.ConnectCharacterToPlayer(this.name, collision.gameObject.name);
            }

            if (collision.gameObject.CompareTag("ChangeGameMode"))
            {
                GameModeManager.Instance.NextGamemode();
            }

            if(collision.gameObject.TryGetComponent(out TeamSelectButton teamSelectButton))
            {
                teamSelectButton.ChangeTeam();
            }
        }

        else if (SceneManager.GetActiveScene().name == "EndGame")
        {
            GameManager.Instance.roundsPlayed = 0;

            if (pressed && collision.gameObject.CompareTag("ResetButton"))
            {
                GameManager.Instance.ResetValues();
                GameManager.Instance.LoadScene("RandomiseMap");
            }
            else if (pressed && collision.gameObject.CompareTag("MainMenuButton"))
            {
                GameManager.Instance.ResetValues();
                GameManager.Instance.playersChosen = 0;
                GameManager.Instance.playersCount = 0;
                GameManager.Instance.players.Clear();

                GameManager.Instance.LoadScene("MainMenu");
            }
        }

        pressed = false;
    }
    private void ResetStartText()
    {
        GameObject mustChoose = GameObject.Find("StartGameText");
        mustChoose.GetComponent<TextMeshProUGUI>().text = "Start Game";
    }

    //public void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (SceneManager.GetActiveScene().name == "CharacterSelect")
    //    {
    //        if (pressed && collision.gameObject.CompareTag("StartButton"))
    //        {
    //            if (GameManager.Instance.playersCount != GameManager.Instance.playersChosen)
    //            {
    //                Debug.Log("Everyone must select a character");
    //            }
    //            else
    //            {
    //                LoadScene("TestScene");
    //            }
    //        }

    //        if (pressed && collision.gameObject.CompareTag("Free"))
    //        {
    //            är denna spelare kopplad till en karaktär
    //            if (GameManager.Instance.players.ContainsKey(this.name))
    //            {
    //                är karaktären inte den man nyss klickade på
    //                if (GameManager.Instance.players[this.name] != Convert.ToInt32(collision.gameObject.name))
    //                {
    //                    hitta den man valt innan och ändra tag på den
    //                    GameObject.Find(GameManager.Instance.players[this.name].ToString()).tag = "Free";

    //                }
    //            }

    //            collision.tag = "Selected";
    //            SetSelectedBall(collision);
    //            SetFrameColor(collision);
    //            GameManager.Instance.ConnectCharacterToPlayer(this.name, collision.gameObject.name);
    //        }
    //    }

    //    else if (SceneManager.GetActiveScene().name == "EndGame")
    //    {
    //        GameManager.Instance.roundsPlayed = 0;

    //        if (pressed && collision.gameObject.CompareTag("ResetButton"))
    //        {
    //            LoadScene("TestScene");
    //        }
    //        else if (pressed && collision.gameObject.CompareTag("MainMenuButton"))
    //        {
    //            LoadScene("MainMenu");
    //        }
    //    }

    //    pressed = false;
    //}

    private void OnTriggerExit2D(Collider2D collision)
    {
        canSelect = false;
    }

    //private void LoadScene(string sceneName)
    //{
    //    SceneManager.LoadScene(sceneName);
    //}

    //private void SetSelectedBall(Collider2D collision)
    //{
    //    if (selected == null)
    //    {
    //        selected = Instantiate(selectedCharacterBall, collision.gameObject.transform);
    //        selected.name = this.name + "Selected" + collision.gameObject.name;
    //    }
    //    else if (selected.name != this.name + "Selected" + collision.gameObject.name)
    //    {
    //        Destroy(selected.gameObject);

    //        selected = Instantiate(selectedCharacterBall, collision.gameObject.transform);
    //        selected.name = this.name + "Selected" + collision.gameObject.name;
    //    }
    //}

    private void SetFrameColor(Collider2D frame)
    {
        if (selectedFrame == null)
        {      
            GameObject emptyFrame = frame.transform.GetChild(0).gameObject; //Saving correct scale
            Destroy(frame.transform.GetChild(0).gameObject); //Remove the empty character frame
            selectedFrame = Instantiate(frame.transform.parent.GetComponent<Selector>().characterFrames[playerIndex], frame.transform.position, frame.transform.rotation, frame.transform);
            selectedFrame.transform.localScale = emptyFrame.transform.localScale;
            selectedFrame.name = name + "Selected" + frame.gameObject.name;
        }
        else if (selectedFrame.name != name + "Selected" + frame.gameObject.name)
        {
            //Sets the previous choice to correct characterframe
            Transform ParentframeTransform = selectedFrame.transform.parent.GetComponent<Transform>(); //Save the parent transform for emptyframe to set position
            Transform frameTransform = selectedFrame.transform; //Keep track of correct scale
            Destroy(selectedFrame.transform.gameObject);
            GameObject emptyFrame = Instantiate(frame.transform.parent.GetComponent<Selector>().characterFrames[4], ParentframeTransform);
            emptyFrame.transform.localScale = frameTransform.localScale; //Set correct scale

            //Sets new choice to correct characterframe
            Destroy(frame.transform.GetChild(0).gameObject);
            selectedFrame = Instantiate(frame.transform.parent.GetComponent<Selector>().characterFrames[playerIndex], frame.transform.position, frame.transform.rotation);
            selectedFrame.transform.SetParent(frame.GetComponent<Transform>());
            selectedFrame.name = name + "Selected" + frame.gameObject.name;
        }
    }

    public void Pressed()
    {
        pressed = true;
    }

    public void MousePosition()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(mouseInput);
        mousePosition.z = 0;

        transform.position = ClampCursorToCamera(mousePosition);
    }

    private Vector3 ClampCursorToCamera(Vector3 mousePosition)
    {
        Camera cam = Camera.main;
        Vector3 camPosition = cam.transform.position;
        float height = cam.orthographicSize;
        float width = height * cam.aspect;

        float camX = Mathf.Clamp(mousePosition.x, -width + camPosition.x, width + camPosition.x);
        float camY = Mathf.Clamp(mousePosition.y, -height + camPosition.y, height + camPosition.y);
        return new Vector3(camX, camY, mousePosition.z);
    }

    public void StickPosition()
    {
        transform.position += stickInput * cursorSpeed * Time.deltaTime;
        transform.position = ClampCursorToCamera(transform.position);
    }


    public void SetMouseAim(Vector3 input)
    {
        mouseInput = input;
        hasGamepad = false;
    }

    public void SetAimStickInput(Vector2 controllerInput)
    {
        stickInput = controllerInput;
        hasGamepad = true;
    }
}
