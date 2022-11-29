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
        if (pressed)
        {
            SetSelectedBall(collision);
            Debug.Log(this.name);
            GameManager.Instance.ConnectCharacterToPlayer(this.name, collision.gameObject.name);
            PressedOff();
        }
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
        Invoke(nameof(PressedOff), 0.02f);
    }
    private void PressedOff()
    {
        pressed = false;
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
