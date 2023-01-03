using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    [SerializeField] public List<string> playScenes = new List<string>();
    public string sceneThisMatch;
    public string lastSceneThisMatch;

    [SerializeField] private List<Sprite> roundTextSprites = new List<Sprite>();
    [SerializeField] private List<Sprite> tiebreakerTextSprites = new List<Sprite>();
    public RectTransform startRoundCountdownText;
    public RectTransform finalRoundPrefab;
    public RectTransform tiebreakerPrefab;

    public Canvas playSceneCanvas;
    public Image playSceneCanvasTextImage;


    public Dictionary<string, int> players = new Dictionary<string, int>();
    public int playersCount;
    public int playersChosen;

    public int roundsPlayed;
    public int roundsInMatch = 6;
    public Dictionary<string, int> playerPoints = new Dictionary<string, int>();
    public int actualWinner;

    public List<GameObject> playerControllers;

    public bool isPaused = false;
    public GameObject pauseMenu;
    private GameObject tempPauseMenu;

    public bool showedControlls = false;
    private int countdown = 3;

    public bool tiebreaker = false;
    public List<int> tiebreakers = new List<int>();

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

        Debug.Log(sceneThisMatch);
        if (scene == "RandomiseMap")
        {
            sceneThisMatch = playScenes[UnityEngine.Random.Range(0, playScenes.Count)];

            while (sceneThisMatch == lastSceneThisMatch)
            {
                sceneThisMatch = playScenes[UnityEngine.Random.Range(0, playScenes.Count)];
            }

            lastSceneThisMatch = sceneThisMatch;
            SceneManager.LoadScene(sceneThisMatch);
        }
        else
        {
            SceneManager.LoadScene(scene);
        }
    }

    public void StartRoundPause()
    {
        if (GameObject.FindGameObjectWithTag("GameOverCanvas") != null)
        {
            playSceneCanvas = GameObject.FindGameObjectWithTag("GameOverCanvas").GetComponent<Canvas>();
            playSceneCanvasTextImage = playSceneCanvas.GetComponentInChildren<Image>();
        }

        StartCoroutine(RoundStartPause());
    }

    IEnumerator RoundStartPause()
    {
        Time.timeScale = 0;
        isPaused = true;
        playSceneCanvasTextImage.enabled = false;

        yield return new WaitForSecondsRealtime(1f);
        PlayClip();

        if (!tiebreaker)
        {
            int currentRound = roundsPlayed + 1;
            if (currentRound == roundsInMatch)
            {
                var finalRoundText = Instantiate(finalRoundPrefab, playSceneCanvas.transform);

                Camera.main.GetComponent<ScreenShakeBehavior>().TriggerShake(0.1f, 0.03f);
                yield return new WaitForSecondsRealtime(0.02f);
                Camera.main.GetComponent<ScreenShakeBehavior>().TriggerShake(0.1f, 0.03f);

                yield return new WaitForSecondsRealtime(2);
                finalRoundText.gameObject.SetActive(false);
            }
            else
            {
                playSceneCanvasTextImage.enabled = true;
                playSceneCanvasTextImage.sprite = roundTextSprites[roundsPlayed];

                yield return new WaitForSecondsRealtime(1);

                playSceneCanvasTextImage.enabled = false;
                var countdown = Instantiate(startRoundCountdownText, playSceneCanvas.transform);

                for (int i = 0; i < 3; i++)
                {
                    Camera.main.GetComponent<ScreenShakeBehavior>().TriggerShake(0.15f, 0.01f);
                    yield return new WaitForSecondsRealtime(1);
                }

                Camera.main.GetComponent<ScreenShakeBehavior>().TriggerShake(0.25f, 0.02f);

                yield return new WaitForSecondsRealtime(1);
            }
        }

        playSceneCanvasTextImage.enabled = false;
        Time.timeScale = 1;
        isPaused = false;
    }

    private void PlayClip()
    {
        Camera.main.GetComponent<AudioSource>().Play();
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
        Debug.Log("Adding " + points + " points to" + playerName);
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

    public void CheckWinner()
    {
        //foreach (KeyValuePair<string, int> kvp in playerPoints)
        //{
        //    Debug.LogFormat("playerPoints: {0} - {1}", kvp.Key, kvp.Value);
        //}

        tiebreaker = false;
        List<int> winner = new List<int>();
        winner = GetWinner();
        if (winner.Count == 1)
        {
            actualWinner = winner[0];
            LoadScene("EndGame");
        }
        else
        {
            SetTiebreaker(winner);
        }
    }

    public List<int> GetWinner()
    {
        List<int> winner = CountPoints();
        return winner;
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

    public void SetTiebreaker(List<int> winners)
    {
        tiebreaker = true;
        GameModeManager.Instance.currentMode = Gamemodes.FreeForAll;
        foreach (int winner in winners)
        {
            tiebreakers.Add(winner);
        }
        SpawnManager spawnManager = GameObject.Find("SpawnPoints").gameObject.GetComponent<SpawnManager>();
        spawnManager.RestartMatch();
    }

    public void StartTiebreaker()
    {
        if (GameObject.FindGameObjectWithTag("GameOverCanvas") != null)
        {
            playSceneCanvas = GameObject.FindGameObjectWithTag("GameOverCanvas").GetComponent<Canvas>();
            playSceneCanvasTextImage = playSceneCanvas.GetComponentInChildren<Image>();
        }
        StartCoroutine(TiebreakerText());
    }

    IEnumerator TiebreakerText()
    {
        Time.timeScale = 0;
        isPaused = true;
        playSceneCanvasTextImage.enabled = false;
        yield return new WaitForSecondsRealtime(1f);

        var tiebreakerImage = Instantiate(tiebreakerPrefab, playSceneCanvas.transform);


        yield return new WaitForSecondsRealtime(3f);

        //SpawnManager.instance.gameOverText.text = "PLAYER " + (tiebreakers[0] + 1);

        for (int i = 1; i < tiebreakers.Count; i++)
        {
            //SpawnManager.instance.gameOverText.text += " VS PLAYER " + (tiebreakers[i] + 1);
        }

        //yield return new WaitForSecondsRealtime(3f);
        foreach (RectTransform child in tiebreakerImage.transform)
        {
            child.gameObject.SetActive(false);
        }
        StartRoundPause();
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

            if (!player.GetComponentInParent<Aim>().hasGamePad)
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
        if (SceneManager.GetActiveScene().name == "LoadingScene")
            return false;

        return true;
    }

    public void ResetValues()
    {
        playerPoints.Clear();
        roundsPlayed = 0;
        tiebreaker = false;
        tiebreakers.Clear();
    }

}
