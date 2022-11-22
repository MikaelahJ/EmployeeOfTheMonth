using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthbar : MonoBehaviour
{
    public AnimationCurve healthCurve;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().position;

        
    }

    public void SetHealthBar(float health)
    {
        float lostHealth = 100 - health;
        transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().position = new Vector3(startPosition.x - lostHealth, startPosition.y, startPosition.z);
    }
}
