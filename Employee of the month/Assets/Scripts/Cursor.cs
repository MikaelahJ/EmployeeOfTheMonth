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

    public Color32 col;

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
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        pressed = false;
    }
    public void OnTriggerStay2D(Collider2D collision)
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
                //�r denna spelare kopplad till en karakt�r
                if (GameManager.Instance.players.ContainsKey(this.name))
                {
                    //�r karakt�ren inte den man nyss klickade p�
                    if (GameManager.Instance.players[this.name] != Convert.ToInt32(collision.gameObject.name))
                    {
                        //hitta den man valt innan och �ndra tag p� den
                        GameObject.Find(GameManager.Instance.players[this.name].ToString()).tag = "Free";

                        Debug.Log(GameObject.Find(GameManager.Instance.players[this.name].ToString()));
                    }
                }

                collision.tag = "Selected";
                SetSelectedBall(collision);
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

    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private void SetSelectedBall(Collider2D collision)
    {
        if (selected == null)
        {
            selected = Instantiate(selectedCharacterBall, collision.gameObject.transform);
            selected.name = this.name + "Selected" + collision.gameObject.name;
        }
        else if (selected.name != this.name + "Selected" + collision.gameObject.name)
        {
            Destroy(selected.gameObject);

            selected = Instantiate(selectedCharacterBall, collision.gameObject.transform);
            selected.name = this.name + "Selected" + collision.gameObject.name;
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
