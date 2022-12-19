using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Aim : MonoBehaviour
{
    [Range(0, 40)]
    public float rotationSpeed;
    public Vector3 mousePosition;
    public Vector2 aimDirection;
    public bool hasGamePad;
    public LayerMask aimAssistLayer;

    private Vector2 aimAssistVector;
    private Vector3 previousMousePosition;
    private Vector2 previousDirection;
    private Vector2 mouseInput;
    private float range;
    private Vector3 closest;
    private List<Collider2D> targetsInRange;


    private void Start()
    {
        range = 0;
        targetsInRange = new List<Collider2D>();
    }

    private void Update()
    {

        AimAssist();

        //Set controls for aim
        if (hasGamePad)
        {
            StickAim();
        }
        else
        {
            MouseAim();
        }
    }


    public void MouseAim()
    {
        mousePosition = mouseInput;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        mousePosition.z = 0;
        mousePosition = mousePosition - transform.position;
        transform.up = Vector2.Lerp(previousMousePosition, (Vector2)mousePosition + aimAssistVector, rotationSpeed);

        transform.up = mousePosition;
        previousMousePosition = mousePosition;
    }

    public void StickAim()
    {
        //if (aimDirection.magnitude > 0.5f) //0,5 is deadzone
        //{
        //    transform.up = aimDirection;
        //}

        //For slower turning speed if needed
        transform.up = Vector2.Lerp(previousDirection, aimDirection + aimAssistVector, rotationSpeed * Time.deltaTime);
        previousDirection = transform.up;
    }

    public void AimAssist()
    {

        if (targetsInRange.Count > 0)
        {
            float angle;
            float aimAngle;
            FindClosest();
            Debug.Log("Finding Closest");
            RaycastHit2D forward = Physics2D.Raycast(transform.position, transform.up, Mathf.Infinity);
            Vector2 direction = closest - transform.position;
            RaycastHit2D enemy = Physics2D.Raycast(transform.position, direction, direction.magnitude, aimAssistLayer);

            if (enemy.collider != null && enemy.collider.CompareTag("Player"))
            {
                Debug.Log("Aiming At " + enemy.collider.gameObject.name);
                aimAssistVector = direction.normalized;
                angle = Vector3.Angle(transform.up, direction);
                aimAngle = Vector3.Angle(previousDirection, aimDirection);

                Debug.Log("enemy angle" + angle);
                Debug.Log("aim angle" + aimAngle);

            }
        }
        else
        {
            aimAssistVector = Vector2.zero;
        }
    }

    private void FindClosest()
    {
        foreach (Collider2D col in targetsInRange)
        {
            float objectRange = (col.gameObject.transform.position - transform.position).magnitude;
            Debug.Log(col.name);

            if (range == 0)
            {
                range = objectRange;
                closest = col.gameObject.transform.position;
            }
            else if (objectRange < range)
            {
                Debug.Log("Updates");
                range = objectRange;
                closest = col.gameObject.transform.position;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Add players that are in range of the bullet
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!targetsInRange.Contains(collision))
            {
                targetsInRange.Add(collision);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Remove players that are not in range of the bullet
        if (collision.CompareTag("Player"))
        {
            if (targetsInRange.Contains(collision))
            {
                targetsInRange.Remove(collision);
            }
        }
    }

    //Used in the controllerinput script
    public void GetMouseInput(Vector3 input)
    {
        mouseInput = input;
    }
    public void SetAimStickInput(Vector2 controllerInput)
    {
        aimDirection = controllerInput;
        hasGamePad = true;
    }

    public void SetAimMouse(Vector3 mouseInput)
    {
        this.mouseInput = mouseInput;
        hasGamePad = false;
    }
}
