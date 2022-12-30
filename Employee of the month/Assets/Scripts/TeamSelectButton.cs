using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

public class TeamSelectButton : MonoBehaviour
{
    public Teams selectedTeam = Teams.NoTeam;
    private TextMeshProUGUI text;
    private List<Color32> pColors = new List<Color32>();

    public bool isActive = false;
    private bool hasInitiated = false;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        pColors.Add(new Color32(255, 255, 255, 255)); // No Team
        pColors.Add(new Color32(255, 146, 0, 255)); // P1 Orange
        pColors.Add(new Color32(169, 0, 255, 255)); // P2 Purple
        pColors.Add(new Color32(0, 255, 109, 255)); // P3 Green
        pColors.Add(new Color32(0, 192, 255, 255)); // P4 Blue
    }

    public void SetTeam(int playerIndex)
    {
        Debug.Log(playerIndex);
        selectedTeam = (Teams)(playerIndex);
        text.color = pColors[playerIndex];
        string AddSpaceBeforeNumbers = string.Join(" ", Regex.Split(selectedTeam.ToString(), @"(?<!^)(?=[0-9])"));
        text.text = AddSpaceBeforeNumbers;
    }

    public void ChangeTeam()
    {
        if (selectedTeam.Equals(Teams.Team4))
        {
            SetTeam((int)Teams.Team1);
        }
        else
        {
            SetTeam((int)selectedTeam + 1);
        }
    }

    public void Activate(int index, bool enabled, string name)
    {
        this.name = name;
        if (!hasInitiated)
        {
            hasInitiated = true;
            SetTeam(index + 1);
        }
        isActive = enabled;
        SetActive(enabled);
    }

    public void SetActive(bool enabled)
    {
        if (!GameModeManager.Instance.hasEnabledTeamsButton)
        {
            enabled = false;
        }
        text.enabled = enabled;
        GetComponent<BoxCollider2D>().enabled = enabled;
    }
}
