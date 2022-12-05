using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;

    public GameObject gameOverText;
    public List<GameObject> spawnPositions;
    private List<int> assigned;
    public int alivePlayers = 0;
    private int roundsPlayed = 0;
    private int roundsInMatch = 3;

    private void Awake()
    {
        if (instance == null)
        {
            assigned = new List<int>();
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        gameOverText.SetActive(false);
    }

    public void PlayerDied()
    {
        alivePlayers -= 1;
        if(alivePlayers <= 1)
        {
            gameOverText.SetActive(true);
            Debug.Log(roundsPlayed);
            if(roundsPlayed < roundsInMatch)
            {
                Invoke("RestartMatch", 5);
            }
            else
            {
                EndMatch();
            }
        }
    }

    public void RestartMatch()
    {
        roundsPlayed++;
        GameManager.Instance.ReloadScene();
    }

    private void EndMatch()
    {
        GameManager.Instance.LoadScene("EndGame");
    }

    public Vector3 GetRandomSpawnPoint()
    {
        int spawnPosition = Random.Range(0, 4);

        while(assigned.Contains(spawnPosition))
        {
            spawnPosition = Random.Range(0, 4);
        }

        assigned.Add(spawnPosition);

        return spawnPositions[spawnPosition].transform.position;
    }
}
