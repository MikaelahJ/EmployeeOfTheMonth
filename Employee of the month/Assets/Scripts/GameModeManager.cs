using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.UI;

public enum Gamemodes
{
    FreeForAll,
    Teams,
    KingOfTheHill,
    Deathmatch,
    Stocks,
    Random,
    LoopBackToTop
}

public class GameModeManager : MonoBehaviour
{
    public static GameModeManager Instance;
    public Gamemodes currentMode = Gamemodes.FreeForAll;

    public List<Team> teams = new List<Team>();

    [Header("Character Select")]
    [SerializeField] private List<GameObject> teamSelectButtons;
    [SerializeField] private TextMeshPro gamemodeText;
    [SerializeField] private TextMeshPro gamemodeOptionsText;
    [SerializeField] private TextMeshPro gamemodeDescriptionText;
    [SerializeField] private GameObject optionsButtons;
    [SerializeField] private Image modeSelect;
    [SerializeField] private Sprite optionsDisabled;
    [SerializeField] private Sprite optionsEnabled;
    [SerializeField] private TrailRenderer trail;
    [SerializeField] private GameObject weaponModifyer;

    private GameObject scoreboard;
    private GameObject kingOfTheHill;

    [Header("Gamemode prefabs")]
    [SerializeField] private GameObject kingOfTheHillArea;
    [SerializeField] private GameObject deathmatchScoreboard;
    [Header("Gamemode settings")]
    [SerializeField] private int chosenNumber = 30; // used for Win Points or Stocks
    [SerializeField] private int[] chooseNumberOptions;
    private int chosenNumberIndex;

    private Teams[] characterSelectedTeams;

    public bool hasEnabledTeamsButton = false;
    private bool hasLoadedCharacterSelect = false;
    private bool teamWon = false;
    bool gamemodeIsRandom = false;

    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            if (SceneManager.GetActiveScene().name == "CharacterSelect")
            {
                if (Instance.gamemodeIsRandom)
                {
                    Instance.currentMode = Gamemodes.Random;
                }
                Instance.teamSelectButtons = teamSelectButtons;
                Instance.gamemodeText = gamemodeText;
                Instance.gamemodeOptionsText = gamemodeOptionsText;
                Instance.gamemodeDescriptionText = gamemodeDescriptionText;
                Instance.optionsButtons = optionsButtons;
                Instance.modeSelect = modeSelect;
            }
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
            hasEnabledTeamsButton = false;
            characterSelectedTeams = new Teams[4];
            //EnableTeamSelectButtons();
        }
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "CharacterSelect")
        {
            EnableTeamSelectButtons();
            EnableGamemodeOptions();
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        teamWon = false;
        ResetPoints();
        if (scene.name == "CharacterSelect")
        {
            teams = new List<Team>();
            hasLoadedCharacterSelect = true;
            EnableTeamSelectButtons();
            EnableGamemodeOptions();

            string AddSpaceBeforeCapitalLetter = string.Join(" ", Regex.Split(currentMode.ToString(), @"(?<!^)(?=[A-Z])"));
            gamemodeText.text = AddSpaceBeforeCapitalLetter;
            return;
        }

        if (!hasLoadedCharacterSelect)
        {
            return;
        }

        bool isInGame = (scene.name != "LoadingScene" && scene.name != "Intermission" && scene.name != "EndGame" && scene.name != "MainMenu");

        if (isInGame)
        {
            if (Instance.gamemodeIsRandom)
            {
                Gamemodes randomMode = (Gamemodes)Random.Range((int)Gamemodes.FreeForAll, (int)Gamemodes.Random);
                currentMode = randomMode;
            }

            scoreboard = GameObject.Find(deathmatchScoreboard.name);
            kingOfTheHill = GameObject.Find(kingOfTheHillArea.name);
            Invoke(nameof(LoadGamemode), 0.001f);
        }
    }

    public void CreateTeams()
    {
        if (currentMode == Gamemodes.FreeForAll)
        {
            return;
        }

        foreach (var playerController in GameManager.Instance.playerControllers)
        {
            ControllerInput controller = playerController.GetComponent<ControllerInput>();
            int playerIndex = controller.playerInput.playerIndex;
            Debug.Log("playerIndex: " + playerIndex);
            int selectedCharacter = GameManager.Instance.players["P" + playerIndex.ToString()];
            Debug.Log("Selected Character: " + selectedCharacter);
            Teams selectedTeam = teamSelectButtons[selectedCharacter - 1].GetComponent<TeamSelectButton>().selectedTeam;
            Debug.Log("Selected team: " + selectedTeam);

            Team team = teams.Find(t => t.GetTeamName() == selectedTeam);

            if (team != null)
            {
                int index = teams.IndexOf(team);
                Debug.Log("Index: " + index);
                teams[index].AddPlayer(playerController);

            }
            else
            {
                Debug.Log("Created new team");
                team = new Team(selectedTeam);
                team.AddPlayer(playerController);
                AddTeam(team);
            }
            controller.playerTeam = team;
        }
    }

    void AddTeam(Team team)
    {
        if (teams.Contains(team))
        {
            Debug.Log("Can't add team: " + team.ToString() + " Team already exists");
            return;
        }
        teams.Add(team);
    }

    void RemoveTeam(Team team)
    {
        teams.Remove(team);
    }

    public void CheckIfTeamWon(Team team)
    {
        if (teamWon) { return; }

        if (team.GetPoints() >= chosenNumber)
        {
            teamWon = true;
            SpawnManager.instance.TeamWon(team);
        }
    }

    public void ResetPoints()
    {
        foreach (var team in teams)
        {
            team.ResetPoints();
        }
    }

    public void NextGamemode()
    {
        currentMode++;
        if (currentMode == Gamemodes.LoopBackToTop)
        {
            currentMode = 0;
        }
        OnChangeGamemode();
    }

    public void PreviousGamemode()
    {
        int prevMode = (int)currentMode - 1;
        if (prevMode < 0)
        {
            currentMode = Gamemodes.LoopBackToTop - 1;
        }
        else
        {
            currentMode--;
        }
        OnChangeGamemode();
    }

    private void OnChangeGamemode()
    {
        EnableTeamSelectButtons();
        EnableGamemodeOptions();

        if (gamemodeText != null)
        {
            string AddSpaceBeforeCapitalLetter = string.Join(" ", Regex.Split(currentMode.ToString(), @"(?<!^)(?=[A-Z])"));
            gamemodeText.text = AddSpaceBeforeCapitalLetter;
        }
    }

    public void EnableTeamSelectButtons()
    {
        bool enabled = true;
        Debug.Log("Current mode: " + currentMode);

        if (currentMode.Equals(Gamemodes.FreeForAll))
        {
            enabled = false;
            hasEnabledTeamsButton = false;
        }
        else
        {
            hasEnabledTeamsButton = true;
        }

        int index = 0;

        foreach (var teamSelectButton in teamSelectButtons)
        {
            teamSelectButton.GetComponent<TeamSelectButton>().SetActive(enabled);

            if (!teamSelectButton.GetComponent<TeamSelectButton>().isActive)
            {
                teamSelectButton.GetComponent<TeamSelectButton>().SetActive(false);
                //Debug.Log("Is not active: " + teamSelectButton.name);
                continue;
            }

            if (enabled && !hasEnabledTeamsButton)
            {
                teamSelectButton.GetComponent<TeamSelectButton>().SetTeam(index);
                index++;
            }
        }

        if (enabled)
            hasEnabledTeamsButton = true;
    }

    public void ActivateTeamSelectButton(int selectIndex, int playerIndex, bool enabled, string name, GameObject controllerInput)
    {
        if (SceneManager.GetActiveScene().name != "CharacterSelect") { return; }
        if (selectIndex == -1) { return; }

        if (enabled)
        {
            teamSelectButtons[selectIndex].GetComponent<TeamSelectButton>().Activate(playerIndex, true, name);
            Debug.Log(teamSelectButtons[selectIndex].GetComponent<TeamSelectButton>().selectedTeam);
            Debug.Log(controllerInput.name);
            //controllerInput.GetComponent<ControllerInput>().playerTeam = teamSelectButtons[index].GetComponent<TeamSelectButton>().selectedTeam;
        }
        else
        {
            teamSelectButtons[selectIndex].GetComponent<TeamSelectButton>().Activate(playerIndex, false, name);
        }
    }

    public void EnableGamemodeOptions()
    {
        gamemodeIsRandom = false;
        switch (currentMode)
        {
            case Gamemodes.FreeForAll:
                {
                    GameManager.Instance.roundsInMatch = 6;
                    gamemodeDescriptionText.text = "";
                    ActivateOptions(false);
                    break;
                }
            case Gamemodes.Teams:
                {
                    GameManager.Instance.roundsInMatch = 6;
                    gamemodeDescriptionText.text = "";
                    ActivateOptions(false);
                    break;
                }
            case Gamemodes.KingOfTheHill:
                {
                    GameManager.Instance.roundsInMatch = 4;
                    gamemodeDescriptionText.text = "Seconds";
                    chooseNumberOptions = new int[] { 5, 10, 15, 20, 25, 30, 40, 50, 60, 99 };
                    chosenNumberIndex = 5;
                    SetChosenNumber();
                    ActivateOptions(true);
                    break;
                }
            case Gamemodes.Deathmatch:
                {
                    GameManager.Instance.roundsInMatch = 4;
                    gamemodeDescriptionText.text = "Kills";
                    chooseNumberOptions = new int[] { 1, 2, 3, 4, 5, 10, 15, 20, 25, 30 };
                    chosenNumberIndex = 5;
                    SetChosenNumber();
                    ActivateOptions(true);
                    break;
                }
            case Gamemodes.Stocks:
                {
                    GameManager.Instance.roundsInMatch = 4;
                    gamemodeDescriptionText.text = "Lives";
                    chooseNumberOptions = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
                    chosenNumberIndex = 2;
                    SetChosenNumber();
                    ActivateOptions(true);
                    break;
                }
            case Gamemodes.Random:
                {
                    gamemodeIsRandom = true;
                    GameManager.Instance.roundsInMatch = 6;
                    gamemodeDescriptionText.text = "";
                    ActivateOptions(false);
                    break;
                }
        }
    }

    private void ActivateOptions(bool enabled)
    {
        if (enabled)
        {
            modeSelect.GetComponent<Image>().sprite = optionsEnabled;
        }
        else
        {
            modeSelect.GetComponent<Image>().sprite = optionsDisabled;
        }

        optionsButtons.SetActive(enabled);
        gamemodeOptionsText.enabled = enabled;
    }

    public void NextOption()
    {
        chosenNumberIndex++;
        if (chosenNumberIndex >= chooseNumberOptions.Length)
            chosenNumberIndex = 0;
        SetChosenNumber();
    }

    public void PreviousOption()
    {
        chosenNumberIndex--;
        if (chosenNumberIndex < 0)
            chosenNumberIndex = chooseNumberOptions.Length - 1;
        SetChosenNumber();
    }

    private void SetChosenNumber()
    {
        chosenNumber = chooseNumberOptions[chosenNumberIndex];
        gamemodeOptionsText.text = chosenNumber.ToString();
    }

    public void LoadGamemode()
    {
        LoadGamemode(currentMode);
    }

    public void LoadGamemode(Gamemodes gamemode, bool isRandom = false)
    {
        Debug.Log("Setting gamemode to " + gamemode.ToString());
        switch (gamemode)
        {
            case Gamemodes.FreeForAll:
                {
                    FreeForAll();
                    break;
                }
            case Gamemodes.Teams:
                {
                    Teams();
                    break;
                }
            case Gamemodes.KingOfTheHill:
                {
                    if (gamemodeIsRandom)
                        chosenNumber = 30;

                    KingOfTheHill();
                    break;
                }
            case Gamemodes.Deathmatch:
                {
                    if (gamemodeIsRandom)
                        chosenNumber = 10;

                    DeathMatch();
                    break;
                }
            case Gamemodes.Stocks:
                {
                    if (gamemodeIsRandom)
                        chosenNumber = 3;

                    Stocks();
                    break;
                }
            case Gamemodes.Random:
                {
                    Gamemodes randomMode = (Gamemodes)Random.Range((int)Gamemodes.FreeForAll, (int)Gamemodes.Random);
                    LoadGamemode(randomMode, true);
                    break;
                }

            case Gamemodes.LoopBackToTop:
                currentMode = 0;
                LoadGamemode(currentMode);
                break;
        }
    }
    private static void SetItemRespawnRate(float respawnRate)
    {
        foreach (WeaponModifyerItem modifyer in FindObjectsOfType<WeaponModifyerItem>())
        {
            modifyer.respawnTime = respawnRate;
        }
    }
    public void FreeForAll()
    {
        SetItemRespawnRate(15f);
    }

    public void Teams()
    {
        SetItemRespawnRate(15f);
    }
    public void KingOfTheHill()
    {
        SetItemRespawnRate(7.5f);
        AddRespawn();

        GameObject kingOfTheHill = GameObject.Find(nameof(kingOfTheHillArea));
        if (kingOfTheHill == null)
        {
            Instantiate(kingOfTheHillArea);
        }
        else
        {
            kingOfTheHill.SetActive(true);
        }
    }



    public void DeathMatch()
    {
        SetItemRespawnRate(7.5f);
        AddRespawn();

        GameObject scoreboard = GameObject.Find(nameof(deathmatchScoreboard));
        if (scoreboard == null)
        {
            scoreboard = Instantiate(deathmatchScoreboard);
        }
        else
        {
            scoreboard.transform.GetChild(0).gameObject.SetActive(true);
        }

        foreach (var team in teams)
        {
            scoreboard.GetComponent<ScoreboardController>().AddTeam(team);
        }
    }
    public void Stocks()
    {
        SetItemRespawnRate(7.5f);
        AddRespawn(chosenNumber);
    }

    public void AddRespawn(int stocks = -1)
    {
        foreach (var team in teams)
        {
            foreach (var player in team.GetPlayers())
            {
                Debug.Log("Added spawner to: " + player.name + ", stocks set to " + stocks);
                player.AddComponent<Spawner>();
                var respawnTrail = Instantiate(trail, player.transform.position, Quaternion.identity, player.transform);
                respawnTrail.gameObject.transform.position = player.transform.position;
                respawnTrail.time = 1.5f;
                respawnTrail.startWidth = 0.3f;
                respawnTrail.endWidth = 0.08f;
                foreach (Transform child in player.transform)//get circleHighlight
                {
                    if (child.name == "PlayerCircleHighlight(Clone)")
                    {
                        respawnTrail.startColor = child.GetComponent<SpriteRenderer>().color;//set trail to player colour
                        respawnTrail.endColor = child.GetComponent<SpriteRenderer>().color;//set trail to player colour
                    }
                }

                if (stocks > 0)
                {
                    player.GetComponent<Spawner>().SetStocks(stocks);
                }
            }
        }
    }


    public int GetWinPoints()
    {
        return chosenNumber;
    }
}

public enum Teams
{
    NoTeam,
    Team1,
    Team2,
    Team3,
    Team4
}

public class Team
{
    private Teams team;
    private List<GameObject> members;
    private float points;
    int alivePlayers;

    public Team(Teams team)
    {
        this.team = team;
        members = new List<GameObject>();
        points = 0;
        alivePlayers = 0;
    }

    public void AddPlayer(GameObject player)
    {
        Debug.Log("Added " + player.name + " to team: " + team);
        members.Add(player);

        foreach (var member in members)
        {
            Debug.Log(member.name);
        }
        alivePlayers++;
    }

    public void RemovePlayer(GameObject player)
    {
        if (members.Remove(player))
            alivePlayers--;
    }

    public void AddPoints(float points)
    {
        Debug.Log("Added points to " + team);
        this.points += points;
        if (this.points > GameModeManager.Instance.GetWinPoints())
            this.points = GameModeManager.Instance.GetWinPoints();

        GameModeManager.Instance.CheckIfTeamWon(this);
    }

    public float GetPoints()
    {
        return points;
    }

    public void ResetPoints()
    {
        points = 0;
    }

    public List<GameObject> GetPlayers()
    {
        foreach (var member in members)
        {
            Debug.Log(member.name);
        }
        List<GameObject> players = new List<GameObject>(members);

        for (int i = 0; i < players.Count; i++)
        {
            players[i] = players[i].GetComponent<ControllerInput>().GetPlayer();
        }

        return players;
    }

    public bool Equals(Team other)
    {
        return team == other.team;
    }

    public Teams GetTeamName()
    {
        return team;
    }

    public int GetAlivePlayers()
    {
        return alivePlayers;
    }

    public Color32 GetColor()
    {
        switch (team)
        {
            case Teams.Team1:
                {
                    return new Color32(255, 146, 0, 255); // P1 Orange
                }
            case Teams.Team2:
                {
                    return new Color32(169, 0, 255, 255); // P2 Purple
                }
            case Teams.Team3:
                {
                    return new Color32(0, 255, 109, 255); // P3 Green
                }
            case Teams.Team4:
                {
                    return new Color32(0, 192, 255, 255); // P4 Blue
                }
        }
        return new Color32(255, 255, 255, 255); // No Team
    }
}

//public Team AddPlayerToTeam(ControllerInput controllerInput)
//{
//    foreach (var team in teams)
//    {
//        Debug.Log("Compare team: " + team.GetTeamName() + " to team: " + controllerInput.playerTeam);
//        if (team.GetTeamName() == controllerInput.playerTeam)
//        {
//            int index = teams.IndexOf(team);
//            teams[index].AddPlayer(controllerInput.GetPlayer());
//            return team;
//        }
//    }
//    return null;
//}

//public void AddPlayersToTeams()
//{
//    foreach (var playerController in GameManager.Instance.playerControllers)
//    {
//        ControllerInput controller = playerController.GetComponent<ControllerInput>();
//        GameObject player = controller.GetPlayer();

//        foreach(var team in teams)
//        {
//            int playerIndex = controller.playerInput.playerIndex;
//            Debug.Log("playerIndex: " + playerIndex);
//            int selectedCharacter = GameManager.Instance.players["P" + playerIndex.ToString()];
//            Debug.Log("Selected Character: " + selectedCharacter);

//            Debug.Log("Compare team: " + team.GetTeamName() + " to team: " + characterSelectedTeams[selectedCharacter - 1]);
//            if (team.GetTeamName() == characterSelectedTeams[selectedCharacter - 1])
//            {
//                int index = teams.IndexOf(team);
//                teams[index].AddPlayer(player);
//            }
//        }
//    }

//    LoadGamemode(currentMode);
//}