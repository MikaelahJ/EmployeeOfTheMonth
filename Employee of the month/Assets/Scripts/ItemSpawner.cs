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

    private void Start()
    {
        GetAllItems();

    }

    private void GetAllItems()
    {
        allItems = new List<NewItemScriptableObject>(Resources.LoadAll<NewItemScriptableObject>("ItemData/Items/"));
        Debug.Log("allitems: " + allItems.Count);
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnItem();
        }

    }

    private void SpawnItem()
    {
        throw new NotImplementedException();
    }
}
