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
    int winPoints;
    [Header("Character Select")]
    [SerializeField] private List<GameObject> teamSelectButtons;
    [SerializeField] private TextMeshProUGUI gamemodeText;
    [Header("Gamemode prefabs")]
    [SerializeField] private GameObject kingOfTheHillArea;
    [SerializeField] private GameObject captureTheFlagHolder;
    [Header("Gamemode settings")]
    [SerializeField] private int numOfStocks;

    private Teams[] characterSelectedTeams; 

    public bool hasEnabledTeamsButton = false;
    private bool hasLoadedCharacterSelect = false;

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
        if (scene.name == "CharacterSelect")
        {
            hasLoadedCharacterSelect = true;
            EnableTeamSelectButtons();
            hasEnabledTeamsButton = false;
        }

        if (!hasLoadedCharacterSelect)
        {
            return;
        }

        if (scene.name == "TestScene" || scene.name == "Map2")
        {
            Invoke(nameof(AddPlayersToTeams), 0.01f);
            
        }
    }

    public void CreateTeams()
    {
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
                AddTeam(team);
            }
        }
    }

    public void AddPlayersToTeams()
    {
        foreach (var playerController in GameManager.Instance.playerControllers)
        {
            ControllerInput controller = playerController.GetComponent<ControllerInput>();
            GameObject player = controller.GetPlayer();

            foreach(var team in teams)
            {
                int playerIndex = controller.playerInput.playerIndex;
                Debug.Log("playerIndex: " + playerIndex);
                int selectedCharacter = GameManager.Instance.players["P" + playerIndex.ToString()];
                Debug.Log("Selected Character: " + selectedCharacter);

                Debug.Log("Compare team: " + team.GetTeam() + " to team: " + characterSelectedTeams[selectedCharacter - 1]);
                if (team.GetTeam() == characterSelectedTeams[selectedCharacter - 1])
                {
                    int index = teams.IndexOf(team);
                    teams[index].AddPlayer(player);
                }
            }
        }

        LoadGamemode(currentMode);
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
        if(team.GetPoints() >= winPoints)
        {

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

    public void ActivateTeamSelectButton(int index, bool enabled, string name)
    {
        if (SceneManager.GetActiveScene().name != "CharacterSelect"){ return; }

        if (enabled)
        {
            teamSelectButtons[index].GetComponent<TeamSelectButton>().Activate(index, true, name);
            characterSelectedTeams[index] = teamSelectButtons[index].GetComponent<TeamSelectButton>().selectedTeam;
        }
        else
        {
            teamSelectButtons[index].GetComponent<TeamSelectButton>().Activate(index, false, name);
            //characterSelectedTeams[index] = global::Teams.NoTeam;
        }
    }

    public void LoadGamemode(Gamemodes gamemode)
    {
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
    private int points;

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

    public void AddPoints(int points)
    {
        this.points += points;
    }

    public int GetPoints()
    {
        return points;
    }

    public List<GameObject> GetPlayers()
    {
        return members;
    }

    public Teams GetTeam()
    {
        return team;
    }
}