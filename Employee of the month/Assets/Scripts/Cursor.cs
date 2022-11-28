using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Cursor : MonoBehaviour
{
    public float cursorSpeed;

    private Vector2 mouseInput;
    private Vector3 mousePosition;
    private Vector3 stickInput;
    private bool hasGamepad;
    private bool pressed;

    private void Start()
    {

    }
    private void Update()
    {
        if(hasGamepad)
        {
            StickPosition();
        }
        else
        {
            MousePosition();
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log("hej" + collision.gameObject.name);

    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
            Debug.Log("hej" + collision.gameObject.name);

        if (pressed)
        {
            GameManager.instance.ConnectCharacterToPlayer(collision.gameObject.name, this.gameObject);
            Debug.Log("hej");

        }
    }
    public void Pressed()
    {
        pressed = true;
        Invoke(nameof(PressedOff), 2f);
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
