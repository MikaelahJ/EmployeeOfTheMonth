using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

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
   
    private void LateUpdate()
    {
        if((ulong)videoPlayer.frame == videoPlayer.frameCount -1.0f)
        {
            StartCoroutine(LoadMainMenu());
        }
        //Debug.Log(videoPlayer.frame);
        //Debug.Log(videoPlayer.frameCount);
    }

    public IEnumerator LoadMainMenu()
    {
        yield return new WaitForSeconds(timeDelay);
        SceneManager.LoadScene(SceneToLoad);
    }
}
