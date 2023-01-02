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
        int winnerSprite = GameManager.Instance.GetWinnerSprite(GameManager.Instance.actualWinner);
        SetWinnerUI(GameManager.Instance.actualWinner, winnerSprite);

        //for testing:
        //int winnerSprite = 4;
        //SetWinnerUI(1, winnerSprite);
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

    private void SetTieBreaker(List<int> winners)
    {
        GameManager.Instance.tiebreaker = true;
        foreach(int winner in winners)
        {
            GameManager.Instance.tiebreakers.Add(winner);
        }

        GameManager.Instance.LoadScene(GameManager.Instance.sceneThisMatch);

        
        //winnerText.text = "TIE";

        //foreach (int winner in winners)
        //{
        //    if (GameManager.Instance.players.ContainsKey("P" + winner))
        //    {
        //        var name = Instantiate(TieNameText, TieNames);
        //        name.text += (winner + 1);

        //        int playerSprite = GameManager.Instance.players["P" + winner];
        //        SetSpriteImages(playerSprite);
        //    }
        //}
    }


    //was for ties but we have no ties winners anymore
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
