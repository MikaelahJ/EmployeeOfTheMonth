using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;



public class Cursor : MonoBehaviour
{
    [SerializeField] private GameObject selectedCharacterBall;
    public float cursorSpeed;

    private Vector2 mouseInput;
    private Vector3 mousePosition;
    private Vector3 stickInput;
    private bool hasGamepad;
    private bool pressed;
    private GameObject selected;


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
        if (pressed && collision.gameObject.CompareTag("StartButton"))
        {
            if(GameManager.Instance.playersCount != GameManager.Instance.playersChosen)
            {
                Debug.Log("Everyone must select a character");
            }
            else
            {
                SceneManager.LoadScene("TestScene");
            }

        }
        if (pressed && !collision.gameObject.CompareTag("StartButton"))
        {
            SetSelectedBall(collision);
            GameManager.Instance.ConnectCharacterToPlayer(this.name, collision.gameObject.name);
            //PressedOff();
        }
        pressed = false;
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
