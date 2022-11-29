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
    private List<NewItemScriptableObject> itemsOLD;
    public NewItemScriptableObject[] items;

    [SerializeField] private GameObject itemHolder;

    private AudioSource sound;
    void Start()
    {
        items = new NewItemScriptableObject[itemSlots];
        UpdateWeaponStats();

        //
        sound = GetComponent<AudioSource>();
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
        for (int i = 0; i < items.Length; i++)
        {
            if(items[i] == null)
            {
                items[i] = item;
                if (itemHolder != null)
                {
                    itemHolder.GetComponent<UIItemHolder>().AddItem(item.itemIcon, i);
                }
                Debug.Log("Added item: " + item.name);
                //Play pickup sound
                sound.PlayOneShot(AudioManager.instance.audioClips.itemPickup);

                UpdateWeaponStats();

                break;
            }

            if(i == items.Length - 1)
            {
                Debug.Log("Inventory full, can't add item: " + item.name);
            }
        }
    }

    //OLD
    public void AddItem2(NewItemScriptableObject item)
    {
        if (itemsOLD.Count < itemSlots)
        {
            itemsOLD.Add(item);

            if(itemHolder != null)
            {
                int index = itemsOLD.IndexOf(item);
                itemHolder.GetComponent<UIItemHolder>().AddItem(item.itemIcon, index);
            }

            Debug.Log("Added item: " + item.name);
        }
        else
        {
            Debug.Log("Inventory full, can't add item: " + item.name);
        }
        UpdateWeaponStats();
    }

    void RemoveItem(int index)
    {
        Debug.Log("Removed item: " + items[index].name);
        items[index] = null;
        if (itemHolder != null)
        {
            itemHolder.GetComponent<UIItemHolder>().RemoveItem(index);
            //Play item removed sound
            sound.PlayOneShot(AudioManager.instance.audioClips.itemDestroyed);
        }
        UpdateWeaponStats();
    }
    //OLD
    void RemoveItem2(NewItemScriptableObject item)
    {
        int index = itemsOLD.IndexOf(item);
        if (itemsOLD.Remove(item))
        {
            if (itemHolder != null)
            {
                itemHolder.GetComponent<UIItemHolder>().RemoveItem(index);
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

        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null) { continue; }
            NewItemScriptableObject item = items[i];
            //Weapon Modifiers
            newWeapon.fireRate += item.fireRate;
            newWeapon.recoilModifier += item.recoilModifier;
            newWeapon.accuracyPercentage += weapon.accuracyPercentage;
            newWeapon.bulletSpreadPercentage += weapon.bulletSpreadPercentage;
            newWeapon.isShotgun = newWeapon.isShotgun || item.isShotgun;
            newWeapon.shotgunAmmount += item.shotgunAmmount;

            //Bullet Modifiers
            newWeapon.isBouncy = newWeapon.isBouncy || item.isBouncy;
            newWeapon.numOfBounces += item.numOfBounces;
            newWeapon.numOfPenetrations += item.numOfPenetrations;
            newWeapon.isPenetrate = newWeapon.isPenetrate || item.isPenetrate;
            newWeapon.isExplosive = newWeapon.isExplosive || item.isExplosive;
            newWeapon.isKnockback = newWeapon.isKnockback || item.isKnockback;
            newWeapon.isHoming = newWeapon.isHoming || item.isHoming;

        }
        weapon = newWeapon;
        UpdateFireStats();
    }


    void UpdateFireStats()
    {
        if (GetComponent<Fire>() != null)
        {
            GetComponent<Fire>().UpdateFireModifiers();
        }
    }

}
