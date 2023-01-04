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

    // Start is called before the first frame update
    void Start()
    {
        videoPlayer = GetComponentInChildren<VideoPlayer>();
        connectedGamepads = Gamepad.all.ToArray();
    }

    private void Update()
    {
        if(Keyboard.current.anyKey.isPressed || Mouse.current.leftButton.isPressed ||Mouse.current.rightButton.isPressed)
        {
            SceneManager.LoadScene(SceneToLoad);
        }

        foreach (Gamepad gamepad in connectedGamepads)
        {
            if (gamepad.buttonEast.isPressed || gamepad.buttonWest.isPressed || gamepad.buttonSouth.isPressed || 
                gamepad.buttonNorth.isPressed)
            {
                SceneManager.LoadScene(SceneToLoad);
            }
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
