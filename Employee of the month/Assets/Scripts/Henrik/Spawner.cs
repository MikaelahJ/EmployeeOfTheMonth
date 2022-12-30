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
        yield return new WaitForSeconds(delay);
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
}
