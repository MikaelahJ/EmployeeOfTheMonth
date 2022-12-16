using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject startButton;
    private GameObject backButton;

    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject HTP;//how to play
    [SerializeField] private GameObject creditsPanel;
    private void Start()
    {
        SetFirstSelectedButton(startButton);
    }

    private void SetFirstSelectedButton(GameObject button)
    {
        var eventSystem = EventSystem.current;
        eventSystem.SetSelectedGameObject(button, new BaseEventData(eventSystem));
    }

    public void OnButtonPress(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void OnOptions()
    {
        if (optionsPanel.activeInHierarchy)
        {
            optionsPanel.SetActive(false);
            SetFirstSelectedButton(startButton);
        }
        else
        {
            optionsPanel.SetActive(true);
            backButton = optionsPanel.GetComponentInChildren<Button>().gameObject;
            SetFirstSelectedButton(backButton);
        }
    }

    public void OnHowtoplay()
    {
        if (HTP.activeInHierarchy)
        {
            HTP.SetActive(false);
            SetFirstSelectedButton(startButton);
        }
        else
        {
            HTP.SetActive(true);
            backButton = HTP.GetComponentInChildren<Button>().gameObject;
            SetFirstSelectedButton(backButton);
        }
    }

    public void OnCredits()
    {
        if (creditsPanel.activeInHierarchy)
        {
            creditsPanel.SetActive(false);
            SetFirstSelectedButton(startButton);
        }
        else
        {
            creditsPanel.SetActive(true);
            backButton = creditsPanel.GetComponentInChildren<Button>().gameObject;
            SetFirstSelectedButton(backButton);
        }
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
