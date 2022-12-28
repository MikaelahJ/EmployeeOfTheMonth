using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gamepads : MonoBehaviour
{
    public int gamepadIndex;
    public InputDevice device;
    public bool hasGamepad;
    [Range(0.0f, 1.0f)]
    public float lowFrequency = 0.5f;
    [Range(0.0f, 1.0f)]
    public float highFrequency = 1.0f;
    [Range(0.0f, 10.0f)]
    public float rumbleTime = 0.1f;

    private Gamepad playerGameController;

    // Start is called before the first frame update
    void Start()
    {
        playerGameController = (Gamepad)device;
    }

    public IEnumerator Rumble()
    {        
        playerGameController.SetMotorSpeeds(lowFrequency, highFrequency);
        yield return new WaitForSeconds(rumbleTime);
        playerGameController.SetMotorSpeeds(0.0f, 0.0f);
    }
}
