using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemHolder : MonoBehaviour
{
    [SerializeField] private GameObject itemRemovedFromGun;

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

    public void RemoveItem(int index, NewItemScriptableObject item)
    {
        if (item == null)
        {
            Debug.Log("UIItemHolder: can't remove item at index " + index + ", item not found");
            return;
        }
        GameObject newItem = Instantiate(itemRemovedFromGun, items[index].transform.position, transform.rotation);

        newItem.GetComponent<ItemRemovedFromGun>().moveDirection = transform.right.normalized;
        newItem.GetComponent<ItemRemovedFromGun>().sprite = item.itemIcon;
        newItem.GetComponent<ItemRemovedFromGun>().brokenSprite = item.itemBrokenOnGround;
        items[index].GetComponent<SpriteRenderer>().sprite = null;

        //var tempColor = itemImage.color;
        //tempColor.a = 0f;
        //itemImage.color = tempColor;
    }
}
