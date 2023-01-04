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
    [SerializeField] private List<Sprite> sliderCubes = new List<Sprite>();

    private Animator animator;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        StartCoroutine(CountPoints());
    }

    IEnumerator CountPoints()
    {
        yield return new WaitForSecondsRealtime(2.1f);

        int countdown = 3;

        for (int i = 0; i < GameManager.Instance.playersCount; i++)
        {
            Debug.Log("countpoints " + i);
            var pointSlider = Instantiate(pointSliderPrefab, sliderHolder);
            pointSlider.value = 0;
            pointSlider.handleRect.GetComponentInChildren<Image>().sprite = heads[GameManager.Instance.players["P" + i] - 1];
            pointSlider.fillRect.GetComponentInChildren<Image>().sprite = sliderCubes[i];

            int points = GameManager.Instance.playerPoints["P" + i];

            for (int k = 1; k <= points; k++)
            {
                yield return new WaitForSecondsRealtime(0.3f);

                pointSlider.value++;
            }

            yield return new WaitForSecondsRealtime(0.5f);
        }

        while (countdown >= 0)
        {
            yield return new WaitForSecondsRealtime(1);

            NextRoundText.alignment = TextAlignmentOptions.TopLeft;
            NextRoundText.text = "Next round in " + countdown.ToString();

            countdown--;
        }
        animator.SetTrigger("EndIntermission");
        yield return new WaitForSecondsRealtime(1.2f);
        GameManager.Instance.HideIntermission(GetComponent<Canvas>());
    }
}
