using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Intermission : MonoBehaviour
{
    [SerializeField] private Slider pointSliderPrefab;
    [SerializeField] private RectTransform sliderHolder;
    [SerializeField] private TextMeshProUGUI NextRoundText;

    [SerializeField] private List<Sprite> heads = new List<Sprite>();

    private void Start()
    {
        StartCoroutine(CountPoints());
    }

    IEnumerator CountPoints()
    {
        int countdown = 3;

        for (int i = 0; i < GameManager.Instance.playersCount; i++)
        {
            Debug.Log("countpoints " + i);
            var pointSlider = Instantiate(pointSliderPrefab, sliderHolder);
            pointSlider.value = 0;
            pointSlider.handleRect.GetComponentInChildren<Image>().sprite = heads[GameManager.Instance.players["P" + i] - 1];


            int points = GameManager.Instance.playerPoints["P" + i];


            for (int k = 1; k <= points; k++)
            {
                yield return new WaitForSeconds(0.3f);

                pointSlider.value++;
            }

            yield return new WaitForSeconds(0.5f);
        }

        while (countdown >= 0)
        {
            yield return new WaitForSecondsRealtime(1);

            NextRoundText.alignment = TextAlignmentOptions.TopLeft;
            NextRoundText.text = "Next round in " + countdown.ToString();

            countdown--;
        }

        GameManager.Instance.LoadScene("RandomiseMap");
    }
}
