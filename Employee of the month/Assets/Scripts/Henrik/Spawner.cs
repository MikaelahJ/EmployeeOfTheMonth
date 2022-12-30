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
            StartCoroutine(DelayedRespawn(delay));
            isTriggered = false;
        }
    }

    IEnumerator DelayedRespawn(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (!Respawn())
        {
            Debug.Log("player ran out of stocks");
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
