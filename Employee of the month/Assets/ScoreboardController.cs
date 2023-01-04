using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

public class ScoreboardController : MonoBehaviour
{
    public static ScoreboardController scoreboardController;

    [SerializeField] GameObject teamScoreHolder;
    [SerializeField] TextMeshPro winScoreText;

    Dictionary<Teams, GameObject> teams = new Dictionary<Teams, GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        scoreboardController = this;
        int winPoints = GameModeManager.Instance.GetWinPoints();
        winScoreText.text = "Get " + winPoints + " Kills to Win!";
    }

    public void AddTeam(Team team)
    {
        GameObject teamText = Instantiate(teamScoreHolder, transform);
        teams.Add(team.GetTeamName(),teamText);
        string teamName = team.GetTeamName().ToString();
        teamName = string.Join(" ", Regex.Split(teamName, @"(?<!^)(?=[0-9])"));

        teamText.transform.GetChild(0).GetComponent<TextMeshPro>().text = teamName;
        teamText.transform.GetChild(0).GetComponent<TextMeshPro>().color = team.GetColor();
    }

    public void SetScore(Team team)
    {
        int score = (int)team.GetPoints();
        teams[team.GetTeamName()].transform.GetChild(1).GetComponent<TextMeshPro>().text = score.ToString() + " Kills";
    }
}
