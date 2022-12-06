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
            if(GameManager.Instance.roundsPlayed < GameManager.Instance.roundsInMatch)
            {
                Invoke(nameof(RestartMatch), 5);
            }
            else
            {
                Invoke(nameof(EndMatch), 5);
            }
        }
    }

    public void RestartMatch()
    {
        GameManager.Instance.roundsPlayed++;
        Debug.Log(GameManager.Instance.roundsPlayed);

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
