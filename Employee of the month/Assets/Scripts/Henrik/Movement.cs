using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField]
    [Range(5, 50)]
    int acceleration;
    [SerializeField]
    [Range(0, 10)]
    int decelaration;
    [SerializeField]
    [Range(0, 50)]
    int walkSpeed;
    [SerializeField]
    [Range(0, 50)]
    int runSpeed;

    //Todo get Input script
    private GetControllerInput controllerInput;
    private Rigidbody2D rb;
    private Vector2 movementVector;
    private Vector2 inputVector;
    private int maxSpeed;
    private Vector2 previousDerection;
    private bool isRunning;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        controllerInput = GetComponent<GetControllerInput>();
        previousDerection = Vector2.zero;
        maxSpeed = walkSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSpeed();
        UpdateMovement();
    }

    private void UpdateSpeed()
    {
        //TODO works but is instant
        //Check if walking backwards
        //Vector2 deltaMovement = (Vector2)transform.position - previousDerection;
        //if(Vector2.Dot(transform.up, deltaMovement) < 0)
        //{
        //    maxSpeed = Mathf.Lerp(maxSpeed, walkSpeed / 2, decelaration);
        //}
        //previousDerection = transform.position;

        if (controllerInput.IsRunning && maxSpeed != runSpeed)
        {
            maxSpeed = runSpeed;
        }

        if(!controllerInput.IsRunning && maxSpeed != walkSpeed)
        {
            maxSpeed = walkSpeed;
        }
    }

    private void UpdateMovement()
    {
        inputVector = controllerInput.Leftstick.normalized;

        movementVector.x += inputVector.x * acceleration * Time.deltaTime;
        movementVector.y += inputVector.y * acceleration * Time.deltaTime;

        if (inputVector.x == 0)
        {
            movementVector.x -= movementVector.x * decelaration * Time.deltaTime;
        }

        if(inputVector.y == 0)
        {
            movementVector.y -= movementVector.y * decelaration * Time.deltaTime;
        }

        movementVector = Vector2.ClampMagnitude(movementVector, maxSpeed);
        rb.velocity = movementVector;
        Debug.Log(transform.up.magnitude);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        movementVector = Vector2.zero;
    }
}
