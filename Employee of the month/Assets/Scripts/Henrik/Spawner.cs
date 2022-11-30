using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private Vector2 spawnPosition;
    public bool isTriggered;

    // Start is called before the first frame update
    void Start()
    {
        spawnPosition = transform.position;
    }

    public void TriggerRespawn(float delay)
    {
        isTriggered = true;
        if (!isTriggered)
        {
            GetComponent<Rigidbody2D>().drag = 10;
            GetComponent<Rigidbody2D>().freezeRotation = true;
            Invoke("Respawn", delay);
            isTriggered = false;
        }
    }


    public void Respawn()
    {
        GetComponent<HasHealth>().OnRespawn();
        GetComponent<Rigidbody2D>().drag = 5;
        GetComponent<Rigidbody2D>().freezeRotation = false;
        transform.position = spawnPosition;
    }
}
