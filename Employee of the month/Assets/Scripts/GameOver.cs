using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GameOver : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI winnerText;
    [SerializeField] private Image winnerImage;
    [SerializeField] private RectTransform winnerSpritePosition;

    [SerializeField] private TextMeshProUGUI TieNameText;
    [SerializeField] private RectTransform TieNames;
    [SerializeField] private RectTransform TieSprites;

    public List<Sprite> winnerSprites = new List<Sprite>();

    private void Start()
    {
        CheckWinner();
    }

    public void CheckWinner()
    {
        List<int> winner = GameManager.Instance.GetWinner();
        if (winner.Count == 1)
        {
            int winnerSprite = GameManager.Instance.GetWinnerSprite(winner[0]);
            SetWinnerUI(winner[0], winnerSprite);
        }
        else
        {
            SetTieUI(winner);
        }
    }

    public void SetWinnerUI(int playerIndex, int playerSprite)
    {
        playerIndex += 1;
        winnerText.text = "Player " + playerIndex;

        switch (playerSprite)
        {
            case 1:
                var image = Instantiate(winnerImage, winnerSpritePosition);
                image.sprite = winnerSprites[0];
                break;
            case 2:
                var image2 = Instantiate(winnerImage, winnerSpritePosition);
                image2.sprite = winnerSprites[1];
                break;
            case 3:
                var image3 = Instantiate(winnerImage, winnerSpritePosition);
                image3.sprite = winnerSprites[2];
                break;
            case 4:
                var image4 = Instantiate(winnerImage, winnerSpritePosition);
                image4.sprite = winnerSprites[3];
                break;
        }
    }
    private void SetTieUI(List<int> winners)
    {
        winnerText.text = "TIE";

        foreach (int winner in winners)
        {
            if (GameManager.Instance.players.ContainsKey("P" + winner))
            {
                var name = Instantiate(TieNameText, TieNames);
                name.text += (winner + 1);

                int playerSprite = GameManager.Instance.players["P" + winner];
                SetSpriteImages(playerSprite);
            }
        }
    }

    private void SetSpriteImages(int playerSprite)
    {
        switch (playerSprite)
        {
            case 1:
                var image = Instantiate(winnerImage, TieSprites);
                image.sprite = winnerSprites[0];
                break;
            case 2:
                var image2 = Instantiate(winnerImage, TieSprites);
                image2.sprite = winnerSprites[1];
                break;
            case 3:
                var image3 = Instantiate(winnerImage, TieSprites);
                image3.sprite = winnerSprites[2];
                break;
            case 4:
                var image4 = Instantiate(winnerImage, TieSprites);
                image4.sprite = winnerSprites[3];
                break;
        }
    }
}
