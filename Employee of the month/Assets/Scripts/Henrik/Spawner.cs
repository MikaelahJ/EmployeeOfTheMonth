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

    public void TriggerRespawn()
    {
        isTriggered = true;
        if (!isTriggered)
        {
            GetComponent<Rigidbody2D>().drag = 10;
            GetComponent<Rigidbody2D>().freezeRotation = true;
            Invoke("Respawn", 5);
            isTriggered = false;
        }
    }

    public void Respawn()
    {
        int health = GetComponent<HasHealth>().maxHealth;
        GetComponent<HasHealth>().GainHealth(health);
        GetComponent<Rigidbody2D>().drag = 5;
        GetComponent<Rigidbody2D>().freezeRotation = false;
        transform.position = spawnPosition;
    }
}
