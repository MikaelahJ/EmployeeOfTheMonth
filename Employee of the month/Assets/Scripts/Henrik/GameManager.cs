using System;
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

    public int roundsPlayed;
    public int roundsInMatch = 3;
    public Dictionary<string, int> playerPoints = new Dictionary<string, int>();


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
        Debug.Log("playername " + playerName);

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

    public int GetWinner()
    {
        int winner = CountPoints();
        return (winner);
    }
    public int GetWinnerSprite(int winner)
    {
        int winnerSprite = players["P" + winner.ToString()];
        return winnerSprite;
    }
    public int CountPoints()
    {
        int result = 0;
        int winner = 0;
        foreach (KeyValuePair<string, int> kvp in playerPoints)
        {
            Debug.LogFormat("playerPoints: {0} - {1}", kvp.Key, kvp.Value);
        }
        for (int i = 0; i < playerPoints.Count; i++)
        {
            if (playerPoints["P" + i.ToString()] > result)
            {
                result = playerPoints["P" + i];
                winner = i;
            }
        }
        return winner;
    }
}
