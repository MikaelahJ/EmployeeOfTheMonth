using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemHolder : MonoBehaviour
{
    GameObject[] items;

    void Start()
    {
        items = new GameObject[transform.childCount];

        for (int i = 0; i < items.Length; i++)
        {
            items[i] = transform.GetChild(i).gameObject;
        }
    }

    void AddItem(Sprite item, int index)
    {
        items[index].GetComponent<Image>().sprite = item;
    }
}
