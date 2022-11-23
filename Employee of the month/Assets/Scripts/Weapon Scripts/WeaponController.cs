using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public int itemSlots = 3;
    [Header("Equipped Weapon")]
    public NewItemScriptableObject weapon;

    [Header("Base Weapon")]
    public NewItemScriptableObject baseWeapon;

    [Header("Test of items")]
    public NewItemScriptableObject item1;
    public NewItemScriptableObject item2;
    public NewItemScriptableObject item3;

    [Header("Equipped Items")]
    public List<NewItemScriptableObject> items;

    [SerializeField] private GameObject itemHolder;

    void Start()
    {
        UpdateWeaponStats();
        //Test to add
        //Debug.Log(items.Count);
        //AddItem(item1);
        //AddItem(item2);
        //AddItem(item3);
        //AddItem(item1);
        //RemoveItem(item1);
    }

    public void AddItem(NewItemScriptableObject item)
    {
        if (items.Count < itemSlots)
        {
            items.Add(item);

            if(itemHolder != null)
            {
                int index = items.IndexOf(item);
                itemHolder.GetComponent<ItemHolder>().AddItem(item.itemIcon, index);
            }

            Debug.Log("Added item: " + item.name);
        }
        else
        {
            Debug.Log("Inventory full, can't add item: " + item.name);
        }
        UpdateWeaponStats();
    }

    void RemoveItem(NewItemScriptableObject item)
    {
        int index = items.IndexOf(item);
        if (items.Remove(item))
        {
            if (itemHolder != null)
            {
                itemHolder.GetComponent<ItemHolder>().RemoveItem(index);
            }

            Debug.Log("Removed item: " + item.name);
        }
        else
        {
            Debug.Log("Did not find item: " + item.name + " in inventory");
        }
        UpdateWeaponStats();
    }

    void UpdateWeaponStats()
    {
        NewItemScriptableObject newWeapon = Instantiate(baseWeapon);
        newWeapon.name = "Weapon";
        foreach (NewItemScriptableObject item in items)
        {
            //Weapon Modifiers
            newWeapon.fireRate += item.fireRate;
            newWeapon.recoilModifier += item.recoilModifier;

            //Bullet Modifiers
            newWeapon.isBouncy = newWeapon.isBouncy || item.isBouncy;
            newWeapon.numOfBounces += item.numOfBounces;
            newWeapon.numOfPenetrations += item.numOfPenetrations;
            newWeapon.isPenetrate = newWeapon.isPenetrate || item.isPenetrate;
            newWeapon.isExplosive = newWeapon.isExplosive || item.isExplosive;
            newWeapon.isKnockback = newWeapon.isKnockback || item.isKnockback;
        }
        weapon = newWeapon;
    }

}
