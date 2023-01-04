using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.InputSystem;

public class IntroScene : MonoBehaviour
{
    public string SceneToLoad = "DeskIntro";
    public float timeDelay = 0.0f;
    private VideoPlayer videoPlayer;
    private Gamepad[] connectedGamepads;
    private Mouse mouse;
    private Keyboard keyboard;
    private bool hasBeenPressed;

    // Start is called before the first frame update
    void Start()
    {
        videoPlayer = GetComponentInChildren<VideoPlayer>();
        connectedGamepads = Gamepad.all.ToArray();
        mouse = Mouse.current;
        keyboard = Keyboard.current;
    }

    private void Update()
    {

        foreach (Gamepad gamepad in connectedGamepads)
        {
            if (gamepad.buttonEast.isPressed || gamepad.buttonWest.isPressed || gamepad.buttonSouth.isPressed ||
                gamepad.buttonNorth.isPressed)
            {
                SceneManager.LoadScene(SceneToLoad);
            }
        }


        if (keyboard.anyKey.wasPressedThisFrame || mouse.leftButton.wasPressedThisFrame || mouse.rightButton.wasPressedThisFrame)
        {
            hasBeenPressed = true;
            SceneManager.LoadScene(SceneToLoad);
        }
    }

    private void LateUpdate()
    {
        if((ulong)videoPlayer.frame == videoPlayer.frameCount -1.0f)
        {
            StartCoroutine(LoadNextScene());
        }
        //Debug.Log(videoPlayer.frame);
        //Debug.Log(videoPlayer.frameCount);
    }

    public IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(timeDelay);
        SceneManager.LoadScene(SceneToLoad);
    }
}
