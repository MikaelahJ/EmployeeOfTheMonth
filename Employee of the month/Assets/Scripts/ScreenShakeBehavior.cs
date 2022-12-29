using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeBehavior : MonoBehaviour
{
    private Transform transform;
    private float shakeDuration = 0f;
    public float shakeMagnitude = 0.7f;
    private float dampingSpeed = 1.0f;
    Vector3 startPosition;

    private void Awake()
    {
        transform = gameObject.GetComponent<Transform>();
        startPosition = transform.localPosition;
    }

    private void Update()
    {
        if(shakeDuration > 0)
        {
            transform.localPosition = transform.localPosition + Random.insideUnitSphere * shakeMagnitude;
            shakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            shakeDuration = 0;
            //transform.localPosition = startPosition;
        }
    }

    public void TriggerShake(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
    }

}
