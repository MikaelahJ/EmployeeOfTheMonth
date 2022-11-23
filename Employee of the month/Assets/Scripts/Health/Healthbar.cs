using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public AnimationCurve healthCurve;

    private Vector3 startPosition;
    private GameObject mask;
    GameObject UIhealth;

    void Start()
    {
        mask = transform.GetChild(0).GetComponent<RectTransform>().gameObject;
        UIhealth = mask.transform.GetChild(0).GetComponent<RectTransform>().gameObject;
        startPosition = mask.transform.position;

        
    }

    public void SetHealthBar(float health)
    {
        float lostHealthPercentage = (100 - health)/100f;
        float UIsize = UIhealth.GetComponent<RectTransform>().rect.width;
        float UIoffset = UIsize * lostHealthPercentage;

        UIhealth.GetComponent<RectTransform>().localPosition = new Vector3(-UIoffset, 0, 0);
    }
}
