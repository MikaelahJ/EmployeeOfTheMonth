using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private Vector2 spawnPosition;
    public bool isRespawning = false;
    [SerializeField] private int stocks = 9999;
    private GameObject trail;

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
            //Debug.Log("Can't find StockHolder");
        }

        if (stocks == 1)
        {
            Destroy(this);
        }
    }

    public void TriggerRespawn(float delay)
    {
        if (!isRespawning)
        {
            isRespawning = true;
            StartCoroutine(DelayedRespawn(delay));
        }
    }

    IEnumerator DelayedRespawn(float delay)
    {
        trail = GetComponentInChildren<TrailRenderer>().gameObject;
        trail.GetComponent<TrailRenderer>().enabled = true;

        yield return new WaitForSeconds(delay);
        Respawn();
    }

    public void Respawn()
    {
        GetComponent<HasHealth>().OnRespawn();

        Vector3 whereToSpawn = SpawnManager.instance.GetRandomSpawnPoint();
        transform.position = whereToSpawn;
        isRespawning = false;

        Invoke(nameof(HideTrail), 1);

        LoseStock();
        if (stocks <= 1)
        {
            Destroy(this);
        }
    }
    private void HideTrail()
    {
        trail.GetComponent<TrailRenderer>().enabled = false;
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
