using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    public Dictionary<string, int> players = new Dictionary<string, int>();
    public int playersCount;
    public int playersChosen;

    public int roundsPlayed;
    public int roundsInMatch = 5;
    public Dictionary<string, int> playerPoints = new Dictionary<string, int>();

    public List<GameObject> playerControllers;

    public bool isPaused = false;
    public GameObject pauseMenu;
    private GameObject tempPauseMenu;

    public bool showedControlls = false;
    private int countdown = 3;



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
        Instance.isPaused = false;
        isPaused = false;
        Time.timeScale = 1;
        SceneManager.LoadScene(scene);

        if (scene == "TestScene")
        {
            StartCoroutine(RoundStartPause());
        }
    }

    IEnumerator RoundStartPause()
    {
        Time.timeScale = 0;
        int delay = 3;
        isPaused = true;


        while (countdown >= 0)
        {
            yield return new WaitForSecondsRealtime(1);

            SpawnManager.instance.gameOverText.text = countdown.ToString();
            countdown--;
        }
        SpawnManager.instance.gameOverText.text = "GO!";

        foreach(GameObject arrow in GameObject.FindGameObjectsWithTag("PlayerArrow"))
        {
            Destroy(arrow);
        }

        Time.timeScale = 1;
        yield return new WaitForSecondsRealtime(1);
        SpawnManager.instance.gameOverText.text = "";



    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ConnectCharacterToPlayer(string playerName, string SelectedCharacter)
    {
        int selectedCharacter = Convert.ToInt32(SelectedCharacter);
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

    public void AddPointsToPlayer(string playerName, int points)
    {
        if (playerPoints.ContainsKey(playerName))
        {
            playerPoints[playerName] += points;
        }
        else
        {
            playerPoints.Add(playerName, points);
        }
        //foreach(KeyValuePair<string,int> kvp in playerPoints)
        //{
        //    Debug.LogFormat("playerPoints: {0} - {1}", kvp.Key, kvp.Value);
        //}
    }

    public List<int> GetWinner()
    {
        List<int> winner = CountPoints();
        return (winner);
    }

    public int GetWinnerSprite(int winner)
    {
        int winnerSprite = players["P" + winner.ToString()];
        return winnerSprite;
    }

    public List<int> CountPoints()
    {
        int result = 0;
        List<int> winner = new List<int>(playersCount);

        for (int i = 0; i < playerPoints.Count; i++)
        {
            if (playerPoints["P" + i.ToString()] > result)
            {
                result = playerPoints["P" + i];
                winner.Insert(0, i);
            }
            else if (playerPoints["P" + i.ToString()] == result && playerPoints["P" + i.ToString()] > 0)
            {
                winner.Add(i);
            }
        }
        return winner;
    }

    public void PauseGame()
    {
        if (!InGameScene()) { return; }

        Debug.Log("Pausing Game!");
        Instance.isPaused = true;
        isPaused = true;
        Time.timeScale = 0;

        foreach (GameObject playerController in Instance.playerControllers)
        {
            Debug.Log("Found input " + playerController.name);
            GameObject player = playerController.GetComponent<ControllerInput>().GetPlayerSprite();
            player.GetComponent<AudioSource>().Pause();
            playerController.GetComponent<ControllerInput>().EnableAim(false);
            playerController.GetComponent<ControllerInput>().LoadCursors();
        }
        Instance.LoadPauseMenu();
    }

    public void UnpauseGame()
    {
        if (!InGameScene()) { return; }

        Debug.Log("Unpausing Game!");
        Instance.isPaused = false;
        isPaused = false;

        foreach (GameObject playerController in Instance.playerControllers)
        {
            Debug.Log("Found input " + playerController.name);
            GameObject player = playerController.GetComponent<ControllerInput>().GetPlayerSprite();
            player.GetComponent<AudioSource>().Play();
            playerController.GetComponent<ControllerInput>().EnableAim(true);
            playerController.GetComponent<ControllerInput>().DestroyCursor();
        }

        Destroy(Instance.tempPauseMenu);
        Time.timeScale = 1;
    }

    private void LoadPauseMenu()
    {
        tempPauseMenu = Instantiate(pauseMenu);
        tempPauseMenu.GetComponent<PauseMenuController>().OpenMainPauseMenu();
    }

    private bool InGameScene()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
            return false;
        if (SceneManager.GetActiveScene().name == "CharacterSelect")
            return false;
        if (SceneManager.GetActiveScene().name == "EndGame")
            return false;

        return true;
    }

}
