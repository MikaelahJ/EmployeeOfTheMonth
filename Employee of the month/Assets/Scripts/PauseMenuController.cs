using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private GameObject mainPauseMenu;
    [SerializeField] private GameObject mainFirstSelectedButton;

    [SerializeField] private GameObject optionsPauseMenu;
    [SerializeField] private GameObject optionsFirstSelectedButton;

    [SerializeField] private GameObject volumeSlider;
    // Start is called before the first frame update
    void Start()
    {
        float currentvol = AudioManager.instance.GetComponent<AudioSource>().volume;
        volumeSlider.GetComponent<Slider>().value = currentvol;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenMainPauseMenu()
    {
        mainPauseMenu.SetActive(true);
        optionsPauseMenu.SetActive(false);

        SetFirstSelectedButton(mainFirstSelectedButton);
    }

    public void OpenOptionsPauseMenu()
    {
        mainPauseMenu.SetActive(false);
        optionsPauseMenu.SetActive(true);

        SetFirstSelectedButton(optionsFirstSelectedButton);
    }

    private void SetFirstSelectedButton(GameObject button)
    {
        var eventSystem = EventSystem.current;
        eventSystem.SetSelectedGameObject(button, new BaseEventData(eventSystem));
    }

    public void SetVolume()
    {
        float volume = volumeSlider.GetComponent<Slider>().value;
        AudioManager.instance.SetVolume(volume);
    }

}
