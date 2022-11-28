using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Cursor : MonoBehaviour
{
    private Vector2 mouseInput;
    private Vector3 mousePosition;

    
    public void MouseAim(Vector3 input)
    {
        mousePosition = input;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        mousePosition.z = 0;

        transform.position = mousePosition;
    }
}
