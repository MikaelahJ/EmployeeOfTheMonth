using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;
    public int intermissionRound = 3;
    [SerializeField] private CameraController camController;
    [SerializeField] private List<Sprite> playerTextSprites = new List<Sprite>();
    [SerializeField] private List<Sprite> otherTextSprites = new List<Sprite>();
    
    public TextMeshProUGUI gameOverText;
    public RectTransform PwinsHolder;
    public Image pNumberImage;


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
        bool teamWon = false;

        if (GameModeManager.Instance.currentMode != Gamemodes.FreeForAll)
        {
            int teamsAlive = 0;
            Team lastTeam = null;
            foreach (Team team in GameModeManager.Instance.teams)
            {
                if (team.GetAlivePlayers() > 0)
                {
                    lastTeam = team;
                    teamsAlive++;
                }
            }
            if (teamsAlive == 1)
            {
                teamWon = true;
                if (GameModeManager.Instance.currentMode != Gamemodes.Teams)
                {
                    TeamWon(lastTeam);
                    return;
                }
                Debug.Log("Team Won! " + lastTeam.GetTeamName());
                //if(GameModeManager.Instance.currentMode != Gamemodes.Teams || GameModeManager.Instance.currentMode != Gamemodes.Stocks)
                //{
                //    TeamWon(lastTeam);
                //    return;
                //}
            }

        }

        if (alivePlayers <= 1 || teamWon)
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
        {//set DRAW text
            GameManager.Instance.playSceneCanvasTextImage.enabled = true;
            GameManager.Instance.playSceneCanvasTextImage.sprite = otherTextSprites[0];
        }
        else
        {
            if (GameModeManager.Instance.currentMode == Gamemodes.FreeForAll)
            {
                AddPointsToLastPlayer();
            }
            else if(GameModeManager.Instance.currentMode == Gamemodes.Teams ||
                    GameModeManager.Instance.currentMode == Gamemodes.Stocks)
            {
                for (int i = 0; i < camController.players.Length; i++)
                {
                    if (camController.players[i] != null)
                    {
                        Team winTeam = camController.players[i].gameObject.GetComponent<HasHealth>().team;
                        AddPointsToTeam(winTeam);
                    }
                }
            }

            if (GameManager.Instance.tiebreaker)
            {
                Invoke(nameof(EndMatch), 3);
                return;
            }
        }

        if (GameManager.Instance.roundsPlayed == intermissionRound)
        {
            PwinsHolder.gameObject.SetActive(false);
            
            StartCoroutine(GameManager.Instance.ShowCoffeeBreak());
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

    private void AddPointsToLastPlayer()
    {
        Debug.Log("Adding points to last player");
        int player = 0;
        for (int i = 0; i < camController.players.Length; i++)
        {
            if (camController.players[i] != null)
            {
                player = camController.players[i].gameObject.GetComponent<HasHealth>().playerIndex;
            }
        }
        GameManager.Instance.AddPointsToPlayer("P" + player.ToString(), 1);

        //Show round winner
        PwinsHolder.gameObject.SetActive(true);
        pNumberImage.sprite = playerTextSprites[player];

        if (GameManager.Instance.tiebreaker)
            GameManager.Instance.actualWinner = player;
    }

    public void TeamWon(Team team)
    {
        GameManager.Instance.roundsPlayed++;
        AudioManager.instance.activateFadeVolume();

        AddPointsToTeam(team);

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

    private void AddPointsToTeam(Team team)
    {
        Debug.Log("Adding points to team: " + team.GetTeamName().ToString());
        foreach (var player in team.GetPlayers())
        {
            int playerIndex = player.gameObject.GetComponent<HasHealth>().playerIndex;
            GameManager.Instance.AddPointsToPlayer("P" + playerIndex.ToString(), 1);
        }

        string AddSpaceBeforeNumbers = string.Join(" ", Regex.Split(team.GetTeamName().ToString(), @"(?<!^)(?=[0-9])"));
        gameOverText.text = AddSpaceBeforeNumbers + " WON";
    }


    public void LoadIntermission()
    {
        PwinsHolder.gameObject.SetActive(false);
        StartCoroutine(GameManager.Instance.ShowCoffeeBreak());
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
