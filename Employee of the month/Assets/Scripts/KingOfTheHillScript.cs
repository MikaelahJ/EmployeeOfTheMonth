using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;

public class KingOfTheHillScript : MonoBehaviour
{
    ParticleSystem sys;
    ParticleSystemRenderer myParticleSystemRenderer;
    BoxCollider2D myCollider;
    LineRenderer myLine;
    [SerializeField] private TextMeshPro scoreText;

    List<Team> teamsInZone = new List<Team>();
    int currentPoints = 0;

    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "";
        myParticleSystemRenderer = GetComponent<ParticleSystemRenderer>();
        sys = GetComponent<ParticleSystem>();
        myCollider = GetComponent<BoxCollider2D>();
        myLine = GetComponent<LineRenderer>();
        SetLine();
    }

    private void SetLine()
    {
        if (teamsInZone.Count == 0)
        {
            SetBorderColors();
        }
        else
        {
            SetBorderColors(teamsInZone[0]);
        }

        Mesh mesh = myCollider.CreateMesh(false, false);
        mesh.Optimize();

        Vector3[] positions = mesh.vertices;
        positions = positions.OrderBy(pos => Vector3.SignedAngle(pos.normalized, Vector3.up, Vector3.forward)).ToArray();

        myLine.positionCount = positions.Length;
        myLine.SetPositions(positions);
    }

    private void SetBorderColors(Team team = null)
    {
        Color32 teamColor = new Color32(255, 255, 255, 255);
        if (team != null)
            teamColor = team.GetColor();

        Material newLineMaterial = new Material(myLine.material);
        newLineMaterial.color = teamColor;
        myLine.material = newLineMaterial;



        var mainModule = sys.main;
        ParticleSystem.MinMaxGradient colGrad = new ParticleSystem.MinMaxGradient(teamColor);
        mainModule.startColor = colGrad;


        //  Material newParticleMaterial = new Material(myParticleSystemRenderer.material);
        //  newParticleMaterial.color = teamColor;
        //  myParticleSystemRenderer.material = newParticleMaterial;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            teamsInZone.Add(other.GetComponentInParent<HasHealth>().team);
            OnlyOneTeamInZone();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (OnlyOneTeamInZone())
            {
                Team team = other.GetComponentInParent<HasHealth>().team;
                team.AddPoints(Time.deltaTime);
                if (team.GetPoints() > currentPoints + 1)
                {
                    DisplayTeamText(team);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            teamsInZone.Remove(other.GetComponentInParent<HasHealth>().team);
            OnlyOneTeamInZone();
        }
    }

    private bool OnlyOneTeamInZone()
    {
        if (teamsInZone.Count == 0)
        {
            scoreText.text = "";
            SetLine();
            return false;
        }
        Team lastTeam = teamsInZone[0];
        for (int i = 1; i < teamsInZone.Count; i++)
        {
            if (lastTeam != teamsInZone[i])
            {
                return false;
            }
        }
        DisplayTeamText(lastTeam);
        return true;

    }

    private void DisplayTeamText(Team team)
    {
        string AddSpaceBeforeNumbers = string.Join(" ", Regex.Split(team.GetTeamName().ToString(), @"(?<!^)(?=[0-9])"));
        currentPoints = Mathf.FloorToInt(team.GetPoints());
        string displayText = AddSpaceBeforeNumbers + "\n" + "Score: " + currentPoints;
        scoreText.text = displayText;
        Color32 teamColor = team.GetColor();
        scoreText.color = teamColor;
        SetLine();
    }


}
