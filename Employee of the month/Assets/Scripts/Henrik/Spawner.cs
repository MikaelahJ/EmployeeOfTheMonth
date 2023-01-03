using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private Vector2 spawnPosition;
    public bool isRespawning = false;
    [SerializeField] private int stocks = 9999;

    // Start is called before the first frame update
    void Start()
    {
        spawnPosition = transform.position;

        stockScript stockScript = GetComponentInChildren<stockScript>();
        if (stockScript != null)
        {
            stockScript.AddStocks(stocks);
        }
        else
        {
            Debug.Log("Can't find StockHolder");
        }
    }

    public void TriggerRespawn(float delay)
    {
        Debug.Log("Trigger respawn");
        if (!isRespawning)
        {
            isRespawning = true;
            StartCoroutine(DelayedRespawn(delay));
        }
    }

    IEnumerator DelayedRespawn(float delay)
    {
        Debug.Log("Init delayed respawn");
        yield return new WaitForSeconds(delay);
        Debug.Log("Init Respawn");
        Respawn();
    }

    public void Respawn()
    {
        Debug.Log("Spawner: Respawning");
        GetComponent<HasHealth>().OnRespawn();
        transform.position = SpawnManager.instance.GetRandomSpawnPoint();
        isRespawning = false;

        LoseStock();
        if(stocks <= 1)
        {
            Debug.Log("Removing spawner");
            Destroy(this);
        }
    }

    public void SetStocks(int lives)
    {
        this.stocks = lives;
    }

    public void LoseStock()
    {
        stocks--;
    }

    public int GetStocks()
    {
        return stocks;
    }
}
