using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private ControllerInput controllerInput;
    [SerializeField] private GameObject ammoCounter;

    private float timer;
    private float fireRate = 0.5f;
    private bool hasFired;

    private float accuracyPercentage = 1f;
    private float bulletSpreadPercentage = 0f;

    private bool isShotgun = false;
    private int shotgunAmmount = 3;
    private float shotgunSpreadBetween = 5;


    void Update()
    {
        timer += Time.deltaTime;

        if (!hasFired) { return; }

        if (timer < fireRate) { return; }

        if (ammoCounter != null)
            if (ammoCounter.GetComponent<UIAmmoCounter>().currentAmmo == 0)
            {
                //Insert Out Of ammo sound
                return;
            }

        FireBullet();
        timer = 0;
    }

    void FireBullet()
    {
        if (isShotgun)
        {
            Shotgun();
        }
        else
        {
            GameObject newBullet = Instantiate(bulletPrefab, firePoint.position, transform.rotation);
            newBullet.GetComponent<Bullet>().UpdateBulletModifyers(GetComponent<WeaponController>().weapon);

            //Bullet Spread
            float spread = bulletSpreadPercentage * (1 - accuracyPercentage);
            newBullet.transform.Rotate(new Vector3(0, 0, Random.Range(-spread, spread)));
        }

        //Ammo counter
        if (ammoCounter != null)
        {
            ammoCounter.GetComponent<UIAmmoCounter>().LoseAmmo();
        }
    }
     
    private void Shotgun()
    {
        //3,5,9 skott
        for (int i = 0; i < shotgunAmmount; i++)
        {
            GameObject newBullet = Instantiate(bulletPrefab, firePoint.position, transform.rotation);
            newBullet.GetComponent<Bullet>().UpdateBulletModifyers(GetComponent<WeaponController>().weapon);
            if(i == 0)
            {
                newBullet.transform.Rotate(new Vector3(0, 0,-shotgunSpreadBetween));
            }
            if (i == shotgunAmmount-1)
            {
                newBullet.transform.Rotate(new Vector3(0, 0, shotgunSpreadBetween));
            }
        }
    }

    public void UpdateFireModifiers()
    {
        NewItemScriptableObject weapon = GetComponent<WeaponController>().weapon;
        fireRate = weapon.fireRate;
        accuracyPercentage = weapon.accuracyPercentage;
        bulletSpreadPercentage = weapon.bulletSpreadPercentage;
        isShotgun = weapon.isShotgun;
        shotgunAmmount = weapon.shotgunAmmount;
    }

    public void GetFireButtonInput(bool input)
    {
        hasFired = input;
    }
}
