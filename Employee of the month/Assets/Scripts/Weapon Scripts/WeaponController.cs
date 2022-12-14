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

    public bool isDead = false;

    [SerializeField] private Laser laserScript;
    [SerializeField] private Fire fireScript;

    void Start()
    {
        items = new NewItemScriptableObject[itemSlots];
        UpdateWeaponStats();

        sound = GetComponent<AudioSource>();
        sound.volume = AudioManager.instance.audioClips.sfxVolume;
    }

    public void AddItem(NewItemScriptableObject item)
    {
        if (isDead)
        {
            Debug.Log("Can't add item: Player is dead!");
            return;
        }

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
                sound.volume = AudioManager.instance.audioClips.sfxVolume;
                sound.PlayOneShot(item.onPickup);

                UpdateWeaponStats();

                return;
            }
        }
        Debug.Log("Inventory full, can't add item: " + item.name);
    }

    public (bool, int) CanAddItem()
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                return (true, i);
            }
        }

        return (false, -1);
    }


    public void RemoveAllItems()
    {
        laserScript.DiscardSuperSprite();

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
        {
            sound.volume = AudioManager.instance.audioClips.sfxVolume;
            sound.PlayOneShot(items[index].onDestroy);
        }

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
            newWeapon.ammoWeight *= item.ammoWeight;
            newWeapon.weaponDamage += item.weaponDamage;
            newWeapon.baseFireRate *= (1 + (item.fireRatePercentage / 100f));
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
            newWeapon.scanBounds += item.scanBounds;
            newWeapon.isStapler = newWeapon.isStapler || item.isStapler;
            newWeapon.stunTime += item.stunTime;
            newWeapon.speedSlowdown += item.speedSlowdown;
        }

        //Add ultimate effects
        if (checkIfUltimate)
        {
            Debug.Log("Equipped ultimate: itemName" + items[0].name);
            if (items[0].name == "Microwave(Clone)")
            {
                newWeapon.isSuperMicro = true;
            }

            if (items[0].name == "Pencil Sharpener(Clone)")
            {
                fireScript.shakeDuration = 0.4f;
                fireScript.shakeMagnitude = 0.5f;
                GetComponent<Fire>().bulletSizeMultiplier = 2f;
            }

            if (items[0].name == "Rubber(Clone)")
            {
                GetComponent<Fire>().isUltimateRubber = true;
                newWeapon.numOfBounces = 50;
            }

            if (items[0].name == "Shredder(Clone)")
            {
                fireScript.shakeDuration = 0.4f;
                fireScript.shakeMagnitude = 0.2f;
                newWeapon.shotgunAmount = 30;
                newWeapon.isSuperShredder = true;
            }

            if (items[0].name == "Stapler(Clone)")
            {
                newWeapon.stunTime = 3;
                newWeapon.isSuperStapler = true;
            }

            if (items[0].ultimateFire != null)
            {
                newWeapon.fire = items[0].ultimateFire;
                newWeapon.ultimateFire = items[0].ultimateFire;
            }
        }
        else
        {
            fireScript.shakeDuration = 0f;
            fireScript.shakeMagnitude = 0f;
            GetComponent<Fire>().bulletSizeMultiplier = 1f;
            GetComponent<Fire>().isUltimateRubber = false;
        }
        weapon = newWeapon;
        UpdateFireStats();
        UpdateAimLine();
    }

    public void LoseItemAmmo(float shots)
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

    void UpdateAimLine()
    {
        if (GetComponentInChildren<AimLine>() != null)
        {
            AimLine aimLine = GetComponentInChildren<AimLine>();
            if(CanAddItem().Item2 == 0)
            {
                aimLine.laserMaxLength = 0;
            }
            else
            {
                aimLine.laserMaxLength = 3 * (weapon.bulletVelocity / baseWeapon.bulletVelocity);
            }
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
