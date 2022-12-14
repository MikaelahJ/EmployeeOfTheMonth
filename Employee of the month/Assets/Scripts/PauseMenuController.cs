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

    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider characterSlider;

    private bool hasStarted = false;
    // Start is called before the first frame update
    private void Awake()
    {
        musicSlider.value = AudioManager.instance.audioClips.musicVolume;
        sfxSlider.value = AudioManager.instance.audioClips.sfxVolume;
        characterSlider.value = AudioManager.instance.audioClips.characterVolume;
    }

    void Start()
    {
        hasStarted = true;
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

    public void SetMusicVolume()
    {
        if (!hasStarted)
            return;

        float volume = musicSlider.value;
        AudioManager.instance.setMusicVolume(volume);
    }

    public void SetsfxVolume()
    {
        if (!hasStarted)
            return;

        float volume = sfxSlider.value;
        AudioManager.instance.setsfxVolume(volume);
    }
    public void SetCharacterVolume()
    {
        if (!hasStarted)
            return;

        float volume = characterSlider.value;
        AudioManager.instance.setCharacterVolume(volume);
    }

}
