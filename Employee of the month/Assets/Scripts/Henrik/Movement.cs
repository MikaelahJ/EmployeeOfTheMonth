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
    public float walkSpeed;
    [SerializeField]
    [Range(0, 50)]
    int runSpeed;

    //Todo get Input script
    private Rigidbody2D rb;
    private Vector2 movementVector;
    public Vector2 leftstickInput;
    private float maxSpeed;
    private bool isRunning;
    public bool justTeleported;

    public AudioSource walksound;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        maxSpeed = walkSpeed;

        animator = GetComponentInChildren<Animator>();

        //Get Audio source in PlayerSprite
        AudioSource[] audiosources = GetComponentsInChildren<AudioSource>();
        foreach (AudioSource source in audiosources)
        {
            if (source.clip != null)
            {
                walksound = source;
            }
        }

        //walking Audio
        walksound.Play();
    }

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

        if (leftstickInput.magnitude < 0.2f)  // slow down rapidly if we dont give a movement input 0.2 is the deadzone
        {
            movementVector = Vector3.Lerp(movementVector, Vector3.zero, Time.fixedDeltaTime * decelaration);
            animator.SetBool("isWalking", false);
        }
        else
        {
            animator.SetBool("isWalking", true);
        }

        if (movementVector.magnitude > maxSpeed) // limit movement speed to the maximum speed set from UpdateSpeed();
        {
            movementVector = movementVector.normalized * maxSpeed;
        }

        rb.AddForce(movementVector, ForceMode2D.Impulse);

        //Set audio sound volume
        walksound.volume = Mathf.Abs(leftstickInput.magnitude * 0.10f * AudioManager.instance.audioClips.sfxVolume);
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
