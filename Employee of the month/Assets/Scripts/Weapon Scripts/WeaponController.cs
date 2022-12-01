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
    public NewItemScriptableObject[] items;

    public UIItemHolder itemHolder;

    private AudioSource sound;
    void Start()
    {
        items = new NewItemScriptableObject[itemSlots];
        UpdateWeaponStats();

        sound = GetComponent<AudioSource>();
    }

    public bool AddItem(NewItemScriptableObject item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                items[i] = Instantiate(item);
                if (itemHolder != null)
                {
                    itemHolder.AddItem(item.itemIcon, i);
                }
                Debug.Log("Added item: " + item.name);
                //Play pickup sound
                // sound.PlayOneShot(AudioManager.instance.audioClips.itemPickup);
                // AudioSource.PlayClipAtPoint(AudioManager.instance.audioClips.itemPickup, transform.position);

                UpdateWeaponStats();

                return true;
            }
        }
        Debug.Log("Inventory full, can't add item: " + item.name);
        return false;
    }


    void RemoveItem(int index)
    {
        Debug.Log("Removed item: " + items[index].name);
        items[index] = null;
        if (itemHolder != null)
        {
            itemHolder.RemoveItem(index);
            //Play item removed sound
            sound.PlayOneShot(AudioManager.instance.audioClips.itemDestroyed);
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
            newWeapon.ammo += item.ammo;
            newWeapon.weaponDamage += item.weaponDamage;
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
            newWeapon.isMicrowave = newWeapon.isMicrowave || item.isMicrowave;
            newWeapon.explosionRadius += item.explosionRadius;
            newWeapon.explosionDamage += item.explosionDamage;
            newWeapon.isKnockback = newWeapon.isKnockback || item.isKnockback;
            newWeapon.isHoming = newWeapon.isHoming || item.isHoming;

        }
        weapon = newWeapon;
        UpdateFireStats();
    }

    public void LoseItemAmmo(int shots)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null)
            {
                items[i].ammo -= shots;
                if (items[i].ammo <= 0)
                {
                    RemoveItem(i);
                }
            }
        }
    }

    void UpdateFireStats()
    {
        if (GetComponent<Fire>() != null)
        {
            GetComponent<Fire>().UpdateFireModifiers();
        }
    }

}
