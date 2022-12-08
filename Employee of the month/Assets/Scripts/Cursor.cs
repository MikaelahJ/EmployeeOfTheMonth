using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;



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

    public Color32 col;
    public int playerIndex;

    private void Awake()
    {
        Invoke("Start", 0.01f);
    }

    private void Start()
    {
        transform.position = Vector3.zero;
        GetComponent<SpriteRenderer>().color = col;
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
            if (pressed && collision.gameObject.CompareTag("StartButton"))
            {
                if (GameManager.Instance.playersCount != GameManager.Instance.playersChosen)
                {
                    Debug.Log("Everyone must select a character");
                }
                else
                {
                    LoadScene("TestScene");
                }
            }

            if (pressed && collision.gameObject.CompareTag("Free"))
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

                collision.tag = "Selected";
                //SetSelectedBall(collision);
                SetFrameColor(collision);
                GameManager.Instance.ConnectCharacterToPlayer(this.name, collision.gameObject.name);
            }
        }

        else if (SceneManager.GetActiveScene().name == "EndGame")
        {
            GameManager.Instance.roundsPlayed = 0;

            if (pressed && collision.gameObject.CompareTag("ResetButton"))
            {
                LoadScene("TestScene");
            }
            else if (pressed && collision.gameObject.CompareTag("MainMenuButton"))
            {
                LoadScene("MainMenu");
            }
        }

        pressed = false;
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

    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

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

        transform.position = mousePosition;
    }

    public void StickPosition()
    {
        transform.position += stickInput * cursorSpeed * Time.deltaTime;
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
