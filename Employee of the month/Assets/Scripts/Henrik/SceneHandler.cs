using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public static SceneHandler instance;
    public string menuName;
    public string mainSceneName;


    public string currentScene;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;

        switch(sceneIndex)
        {
            case 1:
                   //current scene är character select 
                break;
            case 2:
                //current scene testScene
                break;
        }

        Debug.Log(SceneManager.GetActiveScene().name);
    }

    public void LoadMainMenu()
    {
        if (currentScene != menuName)
        {
            SceneManager.LoadScene(menuName);
            currentScene = menuName;
        }
    }

    public void LoadMainScene()
    {
        if(currentScene != mainSceneName)
        {
            SceneManager.LoadScene(mainSceneName);
            currentScene = mainSceneName;
        }
    }
}
