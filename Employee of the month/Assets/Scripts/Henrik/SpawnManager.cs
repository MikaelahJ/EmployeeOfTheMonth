using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;

    [SerializeField] private CameraController camController;

    public TextMeshProUGUI gameOverText;
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

        if (Camera.main.TryGetComponent(out camController))
        {
            Invoke(nameof(AddToDictionary), 0.5f);
        }
    }

    private void AddToDictionary()
    {
        for (int i = 0; i < camController.players.Length; i++)
        {
            if (camController.players[i] != null)
            {
                int player = camController.players[i].gameObject.GetComponent<HasHealth>().playerIndex;
                GameManager.Instance.AddPointsToPlayer("P" + player.ToString(), 0);
            }
        }
    }

    public void PlayerDied()
    {
        alivePlayers -= 1;
        if (alivePlayers == 1)
        {
            Invoke(nameof(CheckRoundWinner), 2);
            AudioManager.instance.activateFadeVolume();
        }
    }

    public void CheckRoundWinner()
    {
        GameManager.Instance.roundsPlayed++;

        if (alivePlayers == 0)
        {
            gameOverText.text = "DRAW";
        }
        else
        {
            int player = 0;
            for (int i = 0; i < camController.players.Length; i++)
            {
                if (camController.players[i] != null)
                {
                    player = camController.players[i].gameObject.GetComponent<HasHealth>().playerIndex;
                }
            }

            //Debug.Log("P" + player.ToString());
            GameManager.Instance.AddPointsToPlayer("P" + player.ToString(), 1);

            gameOverText.text = "PLAYER " + (player += 1) + " WON";
        }
        if(GameManager.Instance.roundsPlayed == 5)
        {
            GameManager.Instance.LoadScene("Intermission");
            return;
        }

        if (GameManager.Instance.roundsPlayed < GameManager.Instance.roundsInMatch)
        {
            Invoke(nameof(RestartMatch), 3);
        }
        else
        {
            Invoke(nameof(EndMatch), 3);
        }
    }

    public void RestartMatch()
    {
        GameManager.Instance.LoadScene(GameManager.Instance.sceneThisMatch);
    }

    private void EndMatch()
    {
        GameManager.Instance.LoadScene("EndGame");
    }

    public Vector3 GetRandomSpawnPoint()
    {
        int spawnPosition = Random.Range(0, 4);

        while (assigned.Contains(spawnPosition))
        {
            spawnPosition = Random.Range(0, 4);
        }

        assigned.Add(spawnPosition);

        return spawnPositions[spawnPosition].transform.position;
    }
}
