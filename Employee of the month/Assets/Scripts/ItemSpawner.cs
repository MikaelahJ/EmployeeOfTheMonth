using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemSpawner : MonoBehaviour
{
    private float spawnInterval = 5;
    private float timer;

    private List<NewItemScriptableObject> allItems;
    private List<Transform> allSpawnPoints = new List<Transform>();

    [SerializeField] private GameObject weaponModifyer;

    private void Start()
    {
        GetAllItems();
        GetSpawnPoints();
        SpawnItem();
    }
    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            //SpawnItem();
        }
    }
    private void GetAllItems()
    {
        allItems = new List<NewItemScriptableObject>(Resources.LoadAll<NewItemScriptableObject>("ItemData/Items/"));
        Debug.Log("allitems: " + allItems.Count);
        Debug.Log("item: " + allItems[2].name);
    }

    private void GetSpawnPoints()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            allSpawnPoints.Add(gameObject.transform.GetChild(i));
        }
        Debug.Log("allSpawns: " + allSpawnPoints.Count);
    }

    private void SpawnItem()
    {
        for (int i = 0; i < allSpawnPoints.Count; i++)
        {
            var randomItem = allItems[UnityEngine.Random.Range(0, allItems.Count)];

            var modifyer = Instantiate(weaponModifyer, allSpawnPoints[i]);
            modifyer.GetComponent<WeaponModifyerItem>().itemType = randomItem;
        }
    }
}
