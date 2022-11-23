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

        items[1].GetComponent<Image>().sprite = items[0].GetComponent<Image>().sprite;
        RemoveItem(0);
    }

    public void AddItem(Sprite item, int index)
    {
        Image itemImage = items[index].GetComponent<Image>();
        var tempColor = itemImage.color;
        tempColor.a = 1f;
        itemImage.color = tempColor;  
        items[index].GetComponent<Image>().sprite = item;
    }

    public void RemoveItem(int index)
    {
        Image itemImage = items[index].GetComponent<Image>();
        var tempColor = itemImage.color;
        tempColor.a = 0f;
        itemImage.color = tempColor;
    }
}
