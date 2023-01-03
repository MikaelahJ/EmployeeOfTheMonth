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
    private GameObject closest;
    private List<GameObject> targetsInRange;


    private void Start()
    {
        range = 0;
        targetsInRange = new List<GameObject>();
    }

    private void Update()
    {
        //Set controls for aim
        if (hasGamePad)
        {
            AimAssist();
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
        transform.up = Vector2.Lerp(previousMousePosition, (Vector2)mousePosition, rotationSpeed);

        transform.up = mousePosition;

        //Fix, preventing character from flipping on x and z axis at z 180 or -180
        if (transform.eulerAngles.x != 0 || transform.eulerAngles.y != 0)
        {
            Vector3 flip = transform.eulerAngles;
            flip.x = 0;
            flip.y = 0;
            transform.eulerAngles = flip;
        }

        previousMousePosition = mousePosition;
    }

    public void StickAim()
    {
        //if (aimDirection.magnitude > 0.5f) //0,5 is deadzone
        //{
        //    transform.up = aimDirection;
        //}


        Vector3 rotate = Vector2.Lerp(previousDirection, aimDirection + aimAssistVector, rotationSpeed * Time.deltaTime);
        rotate.z = 0;
        transform.up = rotate;

        //Fix, preventing character from flipping on x and z axis at z 180 or -180
        if (transform.eulerAngles.x != 0 || transform.eulerAngles.y != 0)
        {
            Vector3 flip = transform.eulerAngles;
            flip.x = 0;
            flip.y = 0;
            transform.eulerAngles = flip;
        }

        previousDirection = transform.up;
    }

    public void AimAssist()
    {
        //Runs if player inside collider on weaponpoint
        if (targetsInRange.Count > 0)
        {
            FindClosest();
            Vector2 direction = closest.transform.position - transform.position;
            RaycastHit2D enemy = Physics2D.Raycast(transform.position, direction, direction.magnitude, aimAssistLayer);

            if (enemy.collider != null && enemy.collider.CompareTag("Player") && aimDirection != Vector2.zero)
            {
                aimAssistVector = direction.normalized;
            }
        }
        else
        {
            aimAssistVector = Vector2.zero;
            range = 0;
            closest = null;
        }
    }

    //finds the player closest to the aim
    private void FindClosest()
    {
        foreach (GameObject enemy in targetsInRange)
        {
            float objectRange = (enemy.transform.position - transform.position).magnitude;

            if (range == 0)
            {
                //Debug.Log("0-range" + range);
                range = objectRange;
                closest = enemy;
            }
            else if (objectRange < range)
            {
                //Debug.Log("Update-range" + range);
                range = objectRange;
                closest = enemy;
                //Debug.Log(closest.name);
            }
            else
            {
                range = (closest.transform.position - transform.position).magnitude;
            }
        }
    }

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    float angle;
    //    float aimAngle;
    //    FindClosest();
    //    Debug.Log("Finding Closest");
    //    RaycastHit2D forward = Physics2D.Raycast(transform.position, transform.up, Mathf.Infinity);
    //    Vector2 direction = closest - transform.position;
    //    RaycastHit2D enemy = Physics2D.Raycast(transform.position, direction, direction.magnitude, aimAssistLayer);

    //    if (enemy.collider != null && enemy.collider.CompareTag("Player"))
    //    {
    //        Debug.Log("Aiming At " + enemy.collider.gameObject.name);
    //        aimAssistVector = direction.normalized;
    //        angle = Vector3.Angle(transform.up, direction);
    //        aimAngle = Vector3.Angle(previousDirection, aimDirection);

    //        Debug.Log("enemy angle" + angle);
    //        Debug.Log("aim angle" + aimAngle);

    //    }
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Add players that are in range of the aim
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!targetsInRange.Contains(collision.gameObject))
            {
                //Debug.Log("added");
                targetsInRange.Add(collision.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Remove players that are not in range of the aim
        if (collision.CompareTag("Player"))
        {
            if (targetsInRange.Contains(collision.gameObject))
            {
                //Debug.Log("removed");
                targetsInRange.Remove(collision.gameObject);
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
