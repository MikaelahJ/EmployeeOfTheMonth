using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioScriptableObject audioClips;

    public AudioClip mainMenu;
    public AudioClip characterSelect;
    public AudioClip inGame;

    public AudioClip fadeSound;
    public float playFadeSoundDelay = 1;
    public float fadeTime = 2;
    private bool isFade = false;

    [SerializeField]private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isFade)
        {
            fadeVolume();
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            audioSource.volume = audioClips.musicVolume;
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        audioSource.Stop();
        if (scene.name == "MainMenu")
        {
            audioSource.clip = mainMenu;
        }

        else if (scene.name == "CharacterSelect")
        {
            audioSource.clip = characterSelect;
        }
        else if (scene.name == "EndGame")
        {
            audioSource.clip = characterSelect;
        }
        else
        {
            audioSource.clip = inGame;
            
        }
        Debug.Log("AudioManager: Playing song \"" + audioSource.clip.name + "\"");
        isFade = false;
        audioSource.volume = audioClips.musicVolume;
        audioSource.Play();
    }

    public void setMusicVolume(float volume)
    {
        audioSource.volume = volume;
        //Save current volume through sessions
        audioClips.musicVolume = volume;
    }
    public void setsfxVolume(float volume)
    {
        //Save current volume through sessions
        audioClips.sfxVolume = volume;
    }
    public void setCharacterVolume(float volume)
    {
        //Save current volume through sessions
        audioClips.characterVolume = volume;
    }


    public void activateFadeVolume()
    {
        isFade = true;
        //audioSource.PlayOneShot(fadeSound);
        Invoke(nameof(playFade), playFadeSoundDelay);
    }

    private void playFade()
    {
        AudioSource.PlayClipAtPoint(fadeSound, Camera.main.transform.position, audioClips.musicVolume);
    }

    private void fadeVolume()
    {
        if(fadeTime == 0)
        {
            audioSource.volume = 0;
            return;
        }
        float fadeAmount = audioClips.musicVolume * (1 / fadeTime) * Time.deltaTime;

        if(audioSource.volume - fadeAmount <= 0)
        {
            audioSource.volume = 0;
            isFade = false;
            return;
        }

        audioSource.volume -= fadeAmount;
    }
}
