using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenuController : MonoBehaviour
{

    [SerializeField] private GameObject middleButton;
    private Image middleSprite;
    [SerializeField] private List<Sprite> CarouselSprites = new List<Sprite>();
    private int spriteIndex = 0;
    [SerializeField] private GameObject buttonCarousel;
    [SerializeField] private GameObject controlsPanel;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject creditsPanel;

    private GameObject backButton;

    private void Start()
    {
        SetFirstSelectedButton(middleButton);
        middleSprite = middleButton.GetComponent<Image>();
    }

    private void SetFirstSelectedButton(GameObject button)
    {
        var eventSystem = EventSystem.current;
        eventSystem.SetSelectedGameObject(button, new BaseEventData(eventSystem));
    }
    private void Update()
    {
        if (buttonCarousel.activeInHierarchy)
        {
            if (EventSystem.current.currentSelectedGameObject.name != "MiddleButton")
            {
                Spin();
            }
        }

    }

    private void Spin()
    {
        string direction = EventSystem.current.currentSelectedGameObject.name;

        spriteIndex += direction == "RightButton" ? 1 : -1;

        if (spriteIndex >= CarouselSprites.Count) spriteIndex = 0;
        if (spriteIndex < 0) spriteIndex = CarouselSprites.Count - 1;

        middleSprite.sprite = CarouselSprites[spriteIndex];

        EventSystem.current.SetSelectedGameObject(middleButton);
    }

    public void OnButtonPress()
    {
        string buttonName = middleSprite.sprite.name;

        switch (buttonName)
        {
            case "PLAY":
                SceneManager.LoadScene("CharacterSelect");
                break;

            case "CONTROLS":
                OnControls();
                break;

            case "OPTIONS":
                OnOptions();
                break;

            case "CREDITS":
                OnCredits();
                break;

            case "EXIT":
                OnExit();
                break;
        }
    }

    public void OnOptions()
    {
        if (optionsPanel.activeInHierarchy)
        {
            optionsPanel.SetActive(false);
            buttonCarousel.SetActive(true);
            SetFirstSelectedButton(middleButton);
        }
        else
        {
            optionsPanel.SetActive(true);
            buttonCarousel.SetActive(false);
            backButton = optionsPanel.GetComponentInChildren<Button>().gameObject;
            SetFirstSelectedButton(backButton);
        }
    }

    public void OnControls()
    {
        if (controlsPanel.activeInHierarchy)
        {
            controlsPanel.SetActive(false);
            buttonCarousel.SetActive(true);
            SetFirstSelectedButton(middleButton);
        }
        else
        {
            controlsPanel.SetActive(true);
            buttonCarousel.SetActive(false);
            backButton = controlsPanel.GetComponentInChildren<Button>().gameObject;
            SetFirstSelectedButton(backButton);
        }
    }

    public void OnCredits()
    {
        if (creditsPanel.activeInHierarchy)
        {
            creditsPanel.SetActive(false);
            buttonCarousel.SetActive(true);
            SetFirstSelectedButton(middleButton);
        }
        else
        {
            creditsPanel.SetActive(true);
            buttonCarousel.SetActive(false);
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
