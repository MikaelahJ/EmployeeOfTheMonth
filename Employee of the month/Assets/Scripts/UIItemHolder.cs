using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemHolder : MonoBehaviour
{
    GameObject[] items;

    [SerializeField] private GameObject item1;
    [SerializeField] private GameObject item2;
    [SerializeField] private GameObject item3;


    void Start()
    {
        items = new GameObject[3];

        items[0] = item1;
        items[1] = item2;
        items[2] = item3;
    }

    public void AddItem(Sprite item, int index)
    {
        SpriteRenderer itemImage = items[index].GetComponent<SpriteRenderer>();
        var tempColor = itemImage.color;
        tempColor.a = 1f;
        itemImage.color = tempColor;
        items[index].GetComponent<SpriteRenderer>().sprite = item;
    }

    public void RemoveItem(int index)
    {
        SpriteRenderer itemImage = items[index].GetComponent<SpriteRenderer>();
        var tempColor = itemImage.color;
        tempColor.a = 0f;
        itemImage.color = tempColor;
    }
}
