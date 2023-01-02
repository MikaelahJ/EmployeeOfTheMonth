using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;

    [SerializeField] private CameraController camController;
    [SerializeField] private List<Sprite> playerTextSprites = new List<Sprite>();
    [SerializeField] private List<Sprite> roundTextSprites = new List<Sprite>();
    [SerializeField] private List<Sprite> otherTextSprites = new List<Sprite>();

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
        if (alivePlayers <= 1)
        {
            if (GameManager.Instance.tiebreaker)
            {
                Invoke(nameof(CheckRoundWinner), 0.1f);
            }
            else
            {
                Invoke(nameof(CheckRoundWinner), 2);
            }

            AudioManager.instance.activateFadeVolume();
        }
    }

    public void CheckRoundWinner()
    {
        GameManager.Instance.roundsPlayed++;

        if (alivePlayers == 0)
        {
            GameManager.Instance.gameoverCanvas.GetComponentInChildren<Image>().sprite = otherTextSprites[0];
            //gameOverText.text = "DRAW";
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
            GameManager.Instance.AddPointsToPlayer("P" + player.ToString(), 1);

            int playerNumber = player + 1;
            gameOverText.text = "PLAYER " + playerNumber + " WON";

            if (GameManager.Instance.tiebreaker)
            {
                GameManager.Instance.actualWinner = player;
                Invoke(nameof(EndMatch), 3);
                return;
            }
        }

        if (GameManager.Instance.roundsPlayed == 3)
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
            Invoke(nameof(CheckWinner), 3);
        }
    }

    public void TeamWon(Team team)
    {
        GameManager.Instance.roundsPlayed++;
        AudioManager.instance.activateFadeVolume();
        foreach (var player in team.GetPlayers())
        {
            int playerIndex = player.gameObject.GetComponent<HasHealth>().playerIndex;
            GameManager.Instance.AddPointsToPlayer("P" + playerIndex.ToString(), 1);
        }

        string AddSpaceBeforeNumbers = string.Join(" ", Regex.Split(team.GetTeamName().ToString(), @"(?<!^)(?=[0-9])"));
        gameOverText.text = AddSpaceBeforeNumbers + " WON";

        if (GameManager.Instance.roundsPlayed == 3)
        {
            Invoke(nameof(LoadIntermission), 3);
            return;
        }

        if (GameManager.Instance.roundsPlayed < GameManager.Instance.roundsInMatch)
        {
            Invoke(nameof(RestartMatch), 3);
        }
        else
        {
            Invoke(nameof(CheckWinner), 3);
        }
    }

    public void LoadIntermission()
    {
        GameManager.Instance.LoadScene("Intermission");
    }

    public void RestartMatch()
    {
        GameManager.Instance.LoadScene(GameManager.Instance.sceneThisMatch);
    }

    public void CheckWinner()
    {
        GameManager.Instance.CheckWinner();
    }

    private void EndMatch()
    {
        GameManager.Instance.tiebreaker = false;
        GameManager.Instance.LoadScene("EndGame");
    }

    public Vector3 GetRandomSpawnPoint()
    {
        int spawnPosition = Random.Range(0, 4);

        while (assigned.Contains(spawnPosition) && assigned.Count < 4)
        {
            spawnPosition = Random.Range(0, 4);
        }

        assigned.Add(spawnPosition);
        StartCoroutine(ClearAssignedSpawn(spawnPosition));

        return spawnPositions[spawnPosition].transform.position;
    }

    private IEnumerator ClearAssignedSpawn(int spawnPosition)
    {
        yield return new WaitForSeconds(5f);
        assigned.Remove(spawnPosition);
    }
}
