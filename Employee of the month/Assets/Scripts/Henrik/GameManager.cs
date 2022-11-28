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

    private List<KeyValuePair<int,string>> players;

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
    }
    
    public void ConnectCharacterToPlayer(int selectedCharacter, GameObject player)
    {
        players.Add(new KeyValuePair<int, string>(selectedCharacter, nameof(player)));
    }

}
