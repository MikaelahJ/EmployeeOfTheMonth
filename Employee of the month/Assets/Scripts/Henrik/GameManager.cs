using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
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

    // Start is called before the first frame update
    void Start()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;

        switch(sceneIndex)
        {
            case 0:
                LoadMainMenu();
                break;
            case 1:
                LoadMainScene();
                break;
        }

        Debug.Log(SceneManager.GetActiveScene().name);
    }

    // Update is called once per frame
    void Update()
    {
        
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
