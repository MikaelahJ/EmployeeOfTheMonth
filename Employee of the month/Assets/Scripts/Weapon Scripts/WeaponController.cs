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
                    itemHolder.AddItem(item, i);
                }
                Debug.Log("Added item: " + item.name);
                //Play pickup sound
                sound.PlayOneShot(item.onPickup);

                UpdateWeaponStats();

                return true;
            }
        }
        Debug.Log("Inventory full, can't add item: " + item.name);
        return false;
    }

    public void RemoveAllItems()
    {
        for (int i = 0; i < items.Length; i++)
        {
            RemoveItem(i, false);
        }
        Debug.Log("Removed all items from weapon!");
    }

    public void RemoveItem(int index, bool playSound)
    {
        if (items[index] == null)
        {
            Debug.Log("Can't remove item at position " + index + ", item not found");
            return;
        }

        //Play item removed sound
        if (playSound)
            sound.PlayOneShot(items[index].onDestroy);

        //Remove item sprite from weapon
        if (itemHolder != null)
        {
            itemHolder.RemoveItem(index, items[index]);
        }
        //Remove item from weapon
        Debug.Log("Removed item: " + items[index].name);
        items[index] = null;
        UpdateWeaponStats();
    }

    void UpdateWeaponStats()
    {
        NewItemScriptableObject newWeapon = Instantiate(baseWeapon);
        newWeapon.name = "Weapon";
        bool checkIfUltimate = true;
        for (int i = 0; i < items.Length; i++)
        {
            //Check if we have item to add to gun
            if (items[i] == null)
            {
                checkIfUltimate = false;
                continue;
            }

            NewItemScriptableObject item = items[i];

            if (i != 0)
            {
                //Check if previous item we added is the same
                checkIfUltimate = checkIfUltimate && item.name == items[i - 1].name;
            }

            //Weapon Modifiers
            if (newWeapon.fireSoundPriority < item.fireSoundPriority)
            {
                newWeapon.fireSoundPriority = item.fireSoundPriority;
                newWeapon.fire = item.fire;
            }

            if (newWeapon.bulletImpactPriority < item.bulletImpactPriority)
            {
                newWeapon.bulletImpactPriority = item.bulletImpactPriority;
                newWeapon.bulletImpactSound = item.bulletImpactSound;
            }

            newWeapon.ammo += item.ammo;
            newWeapon.weaponDamage += item.weaponDamage;
            newWeapon.fireRate += item.fireRate;
            newWeapon.recoilModifier += item.recoilModifier;
            newWeapon.accuracy *= (item.accuracy / 100f);
            newWeapon.maxMissDegAngle += item.maxMissDegAngle;
            newWeapon.isShotgun = newWeapon.isShotgun || item.isShotgun;
            newWeapon.shotgunAmount += item.shotgunAmount;

            //Bullet Modifiers
            if (newWeapon.bulletSpritePriority < item.bulletSpritePriority)
            {
                newWeapon.bulletSpritePriority = item.bulletSpritePriority;
                newWeapon.bulletSprite = item.bulletSprite;
            }

            newWeapon.bulletVelocity += item.bulletVelocity;
            newWeapon.isBouncy = newWeapon.isBouncy || item.isBouncy;
            newWeapon.numOfBounces += item.numOfBounces;
            newWeapon.numOfPenetrations += item.numOfPenetrations;
            newWeapon.isPenetrate = newWeapon.isPenetrate || item.isPenetrate;
            newWeapon.isExplosive = newWeapon.isExplosive || item.isExplosive;
            newWeapon.explosionRadius += item.explosionRadius;
            newWeapon.explosionDamage += item.explosionDamage;
            newWeapon.isKnockback = newWeapon.isKnockback || item.isKnockback;
            newWeapon.knockbackModifier += item.knockbackModifier;
            newWeapon.isHoming = newWeapon.isHoming || item.isHoming;
            newWeapon.turnSpeed += item.turnSpeed;

        }

        //Add ultimate effects
        if (checkIfUltimate)
        {
            newWeapon.isSuperMicro = true;
            if (items[0].ultimateFire != null)
            {
                newWeapon.fire = items[0].ultimateFire;
            }
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
                    RemoveItem(i, true);
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

    public int NumOfItems()
    {
        int count = 0;
        foreach (var item in items)
        {
            if (item != null) { count++; }
        }
        return count;
    }

}
