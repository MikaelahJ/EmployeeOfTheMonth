using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemHolder : MonoBehaviour
{
    [SerializeField] private GameObject itemRemovedFromGun;

    [SerializeField] private GameObject[] items;

    [SerializeField] private GameObject itemHolder;

    Animator[] animators;

    void Start()
    {
        items = new GameObject[3];

        items[0] = Instantiate(itemHolder, transform);
        items[1] = Instantiate(itemHolder, transform);
        items[2] = Instantiate(itemHolder, transform);

        items[0].transform.localPosition += new Vector3(0, 0.214f, 0);

        items[2].transform.localPosition += new Vector3(0, -0.214f, 0);

        animators = GetComponentsInChildren<Animator>();
    }

    public void AddItem(NewItemScriptableObject item, int index)
    {
        if (item.hasAnimations)
        {
            animators[index].enabled = true;

            animators[index].SetBool(item.name, true);
            Debug.Log("Tried to apply item animation bool: " + item.name);
        }


        items[index].GetComponent<SpriteRenderer>().sprite = item.itemIcon;
    }

    public void RemoveItem(int index, NewItemScriptableObject item)
    {
        if (item == null)
        {
            Debug.Log("UIItemHolder: can't remove item at index " + index + ", item not found");
            return;
        }
        GameObject newItem = Instantiate(itemRemovedFromGun, items[index].transform.position, transform.rotation);

        if (item.hasAnimations)
        {
            animators[index].SetBool(item.name, false);
            animators[index].enabled = false;
        }


        newItem.GetComponent<ItemRemovedFromGun>().moveDirection = transform.right.normalized;
        newItem.GetComponent<ItemRemovedFromGun>().sprite = item.itemIcon;
        newItem.GetComponent<ItemRemovedFromGun>().brokenSprite = item.itemBrokenOnGround;
        newItem.GetComponent<ItemRemovedFromGun>().sticksToWalls = item.brokenItemSticksToWall;
        items[index].GetComponent<SpriteRenderer>().sprite = null;
        

        //var tempColor = itemImage.color;
        //tempColor.a = 0f;
        //itemImage.color = tempColor;
    }
}
