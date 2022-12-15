using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenuController : MonoBehaviour
{
    [SerializeField] GameObject optionsPanel;
    [SerializeField] GameObject HTP;//how to play
    [SerializeField] GameObject creditsPanel;

    public void OnButtonPress(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void OnOptions()
    {
        if (optionsPanel.activeInHierarchy)
        {
            optionsPanel.SetActive(false);

        }
        else
            optionsPanel.SetActive(true);
    }

    public void OnHowtoplay()
    {
        if (HTP.activeInHierarchy)
        {
            HTP.SetActive(false);

        }
        else
            HTP.SetActive(true);
    }

    public void OnCredits()
    {
        if (creditsPanel.activeInHierarchy)
        {
            creditsPanel.SetActive(false);

        }
        else
            creditsPanel.SetActive(true);
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
