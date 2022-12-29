using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private Vector2 spawnPosition;
    public bool isTriggered;
    private int stocks = 9999;

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


    public bool Respawn()
    {
        if(stocks <= 0)
        {
            return false;
        }
        GetComponent<HasHealth>().OnRespawn();
        transform.position = SpawnManager.instance.GetRandomSpawnPoint();

        LoseStock();
        return true;
    }

    public void SetStocks(int lives)
    {
        this.stocks = lives;
    }

    public void LoseStock()
    {
        stocks--;
    }
}
