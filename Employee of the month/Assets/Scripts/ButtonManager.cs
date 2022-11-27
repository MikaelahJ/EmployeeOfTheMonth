using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ButtonManager : MonoBehaviour
{
    public void OnButtonPress(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void OnHost()
    {

    }

    public void OnJoin()
    {

    }

    public void OnCredits()
    {
        //TODO show credits
    }

    public void OnExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
}
