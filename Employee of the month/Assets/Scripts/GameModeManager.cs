using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;

public enum Gamemodes
{
    FreeForAll,
    Teams,
    KingOfTheHill,
    DeathMatch,
    Stocks,
    LoopBackToTop
}

public class GameModeManager : MonoBehaviour
{
    public static GameModeManager Instance;
    public Gamemodes currentMode = Gamemodes.FreeForAll;

    List<Team> teams = new List<Team>();

    [Header("Character Select")]
    [SerializeField] private List<GameObject> teamSelectButtons;
    [SerializeField] private TextMeshProUGUI gamemodeText;
    [Header("Gamemode prefabs")]
    [SerializeField] private GameObject kingOfTheHillArea;
    [SerializeField] private GameObject captureTheFlagHolder;
    [Header("Gamemode settings")]
    [SerializeField] private int winPoints = 30;
    [SerializeField] private int numOfStocks = 3;

    private Teams[] characterSelectedTeams; 

    public bool hasEnabledTeamsButton = false;
    private bool hasLoadedCharacterSelect = false;
    private bool teamWon = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            if(SceneManager.GetActiveScene().name == "CharacterSelect")
            {
                Instance.teamSelectButtons = teamSelectButtons;
                Instance.gamemodeText = gamemodeText;
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
        EnableTeamSelectButtons();
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
            //hasEnabledTeamsButton = false;
            string AddSpaceBeforeCapitalLetter = string.Join(" ", Regex.Split(currentMode.ToString(), @"(?<!^)(?=[A-Z])"));
            gamemodeText.text = AddSpaceBeforeCapitalLetter;
            return;
        }

        if (!hasLoadedCharacterSelect)
        {
            return;
        }

        bool isInGame = (scene.name != "LoadingScene" && scene.name != "Intermission" && scene.name != "EndGame" && scene.name != "MainMenu");

        if(isInGame)
        {
            Invoke(nameof(LoadGamemode), 0.001f);

        }
    }

    public void CreateTeams()
    {
        if (currentMode == Gamemodes.FreeForAll)
        {
            return;
        }

        foreach(var playerController in GameManager.Instance.playerControllers)
        {
            ControllerInput controller = playerController.GetComponent<ControllerInput>();
            int playerIndex = controller.playerInput.playerIndex;
            Debug.Log("playerIndex: " + playerIndex);
            int selectedCharacter = GameManager.Instance.players["P" + playerIndex.ToString()];
            Debug.Log("Selected Character: " + selectedCharacter);
            Teams selectedTeam = teamSelectButtons[selectedCharacter - 1].GetComponent<TeamSelectButton>().selectedTeam;
            Debug.Log("Selected team: " + selectedTeam);
            Team team = new Team(selectedTeam);

            if (!teams.Contains(team))
            {
                team.AddPlayer(playerController);
                AddTeam(team);
            }
            else
            {
                teams[teams.IndexOf(team)].AddPlayer(playerController);
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

        if(team.GetPoints() >= winPoints)
        {
            teamWon = true;
            SpawnManager.instance.TeamWon(team);
        }
    }

    public void ResetPoints()
    {
        foreach(var team in teams)
        {
            team.ResetPoints();
        }
    }

    public void NextGamemode()
    {
        currentMode++;
        if(currentMode == Gamemodes.LoopBackToTop)
        {
            currentMode = 0;
        }

        EnableTeamSelectButtons();

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

        foreach(var teamSelectButton in teamSelectButtons)
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

    public void ActivateTeamSelectButton(int index, bool enabled, string name, GameObject controllerInput)
    {
        if (SceneManager.GetActiveScene().name != "CharacterSelect"){ return; }

        if (enabled)
        {
            teamSelectButtons[index].GetComponent<TeamSelectButton>().Activate(index, true, name);
            Debug.Log(teamSelectButtons[index].GetComponent<TeamSelectButton>().selectedTeam);
            Debug.Log(controllerInput.name);
            //controllerInput.GetComponent<ControllerInput>().playerTeam = teamSelectButtons[index].GetComponent<TeamSelectButton>().selectedTeam;
        }
        else
        {
            teamSelectButtons[index].GetComponent<TeamSelectButton>().Activate(index, false, name);
        }
    }

    public void LoadGamemode()
    {
        LoadGamemode(currentMode);
    }

    public void LoadGamemode(Gamemodes gamemode)
    {
        Debug.Log("Setting gamemode to " + gamemode.ToString());
        switch(gamemode)
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
                    KingOfTheHill();
                    break;
                }
            case Gamemodes.DeathMatch:
                {
                    DeathMatch();
                    break;
                }
            case Gamemodes.Stocks:
                {
                    Stocks();
                    break;
                }
            case Gamemodes.LoopBackToTop:
                currentMode = 0;
                LoadGamemode(currentMode);
                break;
        }
    }

    public void FreeForAll()
    {

    }
    public void Teams()
    {

    }
    public void KingOfTheHill()
    {
        AddRespawn();

        GameObject kingOfTheHill = GameObject.Find(nameof(kingOfTheHillArea));
        if(kingOfTheHill == null)
        {
            Instantiate(kingOfTheHillArea);
        }
    }
    public void DeathMatch()
    {
        AddRespawn();
    }
    public void Stocks()
    {
        AddRespawn(numOfStocks);
    }

    public void AddRespawn(int stocks = -1)
    {
        foreach (var team in teams)
        {
            foreach (var player in team.GetPlayers())
            {
                Debug.Log("Added spawner to: " + player.name + ", stocks set to " + stocks);
                player.AddComponent<Spawner>();
                if(stocks > 0)
                {
                    player.GetComponent<Spawner>().SetStocks(stocks);
                }
            }
        }
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

    public Team(Teams team)
    {
        this.team = team;
        members = new List<GameObject>();
        points = 0;
    }

    public void AddPlayer(GameObject player)
    {
        Debug.Log("Added " + player.name + " to team: " + team);
        members.Add(player);
    }

    public void RemovePlayer(GameObject player)
    {
        members.Remove(player);
    }

    public void AddPoints(float points)
    {
        Debug.Log("Added points to " + team);
        this.points += points;
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
        List<GameObject> players = new List<GameObject>(members);

        for (int i = 0; i < players.Count; i++)
        {
            players[i] = players[i].GetComponent<ControllerInput>().GetPlayer();
        }

        return players;
    }

    public Teams GetTeamName()
    {
        return team;
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