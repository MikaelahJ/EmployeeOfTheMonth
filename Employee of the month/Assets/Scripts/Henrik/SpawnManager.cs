using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;

    public List<GameObject> spawnPositions;
    private List<int> assigned;

    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        assigned = new List<int>();
    }

    public Vector3 GetRandomSpawnPoint()
    {
        int spawnPosition = Random.Range(0, 4);

        while(assigned.Contains(spawnPosition))
        {
            spawnPosition = Random.Range(0, 4);
        }

        assigned.Add(spawnPosition);

        return spawnPositions[spawnPosition].transform.position;
    }
}
