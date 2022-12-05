using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    public Dictionary<string, int> players = new Dictionary<string, int>();
    public int playersCount;
    public int playersChosen;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ConnectCharacterToPlayer(string playerName, string SelectedCharacter)
    {
        int selectedCharacter = System.Convert.ToInt32(SelectedCharacter);

        if (players.ContainsKey(playerName))
        {
            players[playerName] = selectedCharacter;
        }
        else
        {
            players.Add(playerName, selectedCharacter);
        }
        playersChosen = players.Count;
    }

}
