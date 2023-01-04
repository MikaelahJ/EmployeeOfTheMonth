using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

public class Loading : MonoBehaviour
{
    public float SceneLoadDelay = 5f;

    [SerializeField] private Animator animator;
    [SerializeField] private TextMeshProUGUI tip;
    [SerializeField] private TextMeshProUGUI gameMode;
    [SerializeField] private List<string> tips = new List<string>();
    [SerializeField] private List<string> explainations = new List<string>();

    private float tipTimer;
    private float tipTimeRate = 2.5f;

    private void Start()
    {
        StartCoroutine(DelayedLoadScene());
        tip.text = tips[Random.Range(0, tips.Count)];
        string AddSpaceBeforeCapitalLetter = string.Join(" ", Regex.Split(GameModeManager.Instance.currentMode.ToString(), @"(?<!^)(?=[A-Z])"));
        gameMode.text = AddSpaceBeforeCapitalLetter;

        

    }

    private void Update()
    {
        tipTimer += Time.deltaTime;

        if (tipTimer >= tipTimeRate)
        {
            tip.text = tips[Random.Range(0, tips.Count)];
            tipTimer = 0;
        }

        //if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Loading"))
        //{
        //    Debug.Log("hello");
        //    GameManager.Instance.LoadScene("RandomiseMap");
        //}
    }

    private IEnumerator DelayedLoadScene()
    {
        yield return new WaitForSeconds(SceneLoadDelay);
        NextScene();
    }

    public void NextScene()
    {
        GameManager.Instance.LoadScene("RandomiseMap");

    }

}