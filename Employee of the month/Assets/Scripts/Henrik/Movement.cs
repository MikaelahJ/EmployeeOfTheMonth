using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField]
    [Range(5, 2000)]
    int acceleration;
    [SerializeField]
    [Range(0, 1000)]
    int decelaration;
    [SerializeField]
    [Range(0, 50)]
    int walkSpeed;
    [SerializeField]
    [Range(0, 50)]
    int runSpeed;

    //Todo get Input script
    private Rigidbody2D rb;
    private Vector2 movementVector;
    public Vector2 leftstickInput;
    private int maxSpeed;
    private bool isRunning;

    public AudioSource walksound;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        maxSpeed = walkSpeed;

        //Get Audio source in PlayerSprite
        AudioSource[] audiosources = GetComponentsInChildren<AudioSource>();
        foreach (AudioSource source in audiosources)
        {
            if(source.clip != null)
            {
                walksound = source;
            }
        }

        //walking Audio
        walksound.Play();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateSpeed();
        UpdateMovement();
    }

    private void UpdateSpeed()
    {
        if (isRunning)
        {
            maxSpeed = runSpeed;
        }

        if (!isRunning)
        {
            maxSpeed = walkSpeed;
        }
    }

    private void UpdateMovement()
    {
        movementVector += leftstickInput * acceleration * Time.fixedDeltaTime;

        if (leftstickInput.magnitude < 0.2f)  // slow down rapidly if we dont give a movement input (12 is an arbitrary large number) 0.2 is the deadzone
        {
            movementVector = Vector3.Lerp(movementVector, Vector3.zero, Time.fixedDeltaTime * decelaration);
        }

        if (movementVector.magnitude > maxSpeed) // limit movement speed to the maximum speed set from UpdateSpeed();
        {
            movementVector = movementVector.normalized * maxSpeed;
        }

        rb.AddForce(movementVector, ForceMode2D.Impulse);

        //Set audio sound volume
        walksound.volume = Mathf.Abs(leftstickInput.magnitude/2f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        movementVector = Vector2.zero;
    }


    //Used in the controllerinput script
    public void GetLeftStickInput(Vector2 input)
    {
        leftstickInput = input;
    }

    //Used in the controllerinput script
    public void GetRunButtonInput(bool input)
    {
        isRunning = input;
    }


}
