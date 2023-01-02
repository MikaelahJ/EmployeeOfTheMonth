using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mug : MonoBehaviour
{
    public float offsetY;

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.parent.position + Vector3.up * offsetY; //Make sure the bar is over player
        transform.LookAt(transform.position + Camera.main.transform.forward);
        Destroy(gameObject, 3.0f);
    }
}
