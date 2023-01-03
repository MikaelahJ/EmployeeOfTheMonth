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

    // Start is called before the first frame update
    void Start()
    {
        videoPlayer = GetComponentInChildren<VideoPlayer>();
    }

    private void Update()
    {
        if(Keyboard.current.anyKey.isPressed || Gamepad.current.buttonEast.isPressed || Gamepad.current.buttonWest.isPressed ||
            Gamepad.current.buttonSouth.isPressed || Gamepad.current.buttonNorth.isPressed || Mouse.current.leftButton.isPressed || 
        Mouse.current.rightButton.isPressed)
        {
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
