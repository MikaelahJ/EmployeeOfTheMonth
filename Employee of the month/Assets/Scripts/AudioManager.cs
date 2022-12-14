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

    [SerializeField]private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            audioSource.volume = audioClips.volume;
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
        audioSource.Play();
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
        //Save current volume through sessions
        audioClips.volume = volume;
    }
}
