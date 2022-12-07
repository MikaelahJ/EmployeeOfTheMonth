using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GameOver : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI winnerText;
    [SerializeField] private Image winnerImage;

    public List<Sprite> winnerSprites = new List<Sprite>();
    private void Start()
    {
        CheckWinner();
    }

    public void CheckWinner()
    {
        int winner = GameManager.Instance.GetWinner();
        int winnerSprite = GameManager.Instance.GetWinnerSprite(winner);

        SetWinnerUI(winner, winnerSprite);
    }

    public void SetWinnerUI(int playerIndex, int playerSprite)
    {
        playerIndex += 1;
        winnerText.text = "Player " + playerIndex;
        Debug.Log("playersprite" + playerSprite);
        switch (playerSprite)
        {
            case 1:
                winnerImage.sprite = winnerSprites[0];
                break;
            case 2:
                winnerImage.sprite = winnerSprites[1];
                break;
            case 3:
                winnerImage.sprite = winnerSprites[2];
                break;
            case 4:
                winnerImage.sprite = winnerSprites[3];
                break;
        }
    }
}
