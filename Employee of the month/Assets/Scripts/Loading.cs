using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Loading : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private TextMeshProUGUI tip;
    [SerializeField] private List<string> tips = new List<string>();

    private float tipTimer;
    private float tipTimeRate = 2.5f;

    private void Start()
    {
        tip.text = tips[Random.Range(0, tips.Count)];
    }

    private void Update()
    {
        tipTimer += Time.deltaTime;

        if(tipTimer>= tipTimeRate)
        {
            tip.text = tips[Random.Range(0, tips.Count)];
            tipTimer = 0;
        }

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Loading"))
        {
            Debug.Log("hello");
            GameManager.Instance.LoadScene("RandomiseMap");
        }
    }


    public void NextScene()
    {
        GameManager.Instance.LoadScene("RandomiseMap");

    }
}
