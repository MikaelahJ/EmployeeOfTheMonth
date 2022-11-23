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

    void Update()
    {
        timer += Time.deltaTime;

        if (!hasFired) { return; }

        if(timer < fireRate) { return; }

        if(ammoCounter != null)
            if(ammoCounter.GetComponent<AmmoCounter>().currentAmmo == 0)
            {
                //Insert Out Of ammo sound
                return;
            }

        FireBullet();
        timer = 0;
    }

    void FireBullet()
    {
        GameObject newBullet = Instantiate(bulletPrefab, firePoint.position, transform.rotation);
        newBullet.GetComponent<Bullet>().UpdateBulletModifyers(GetComponent<WeaponController>().weapon);

        if (ammoCounter != null)
        {
            ammoCounter.GetComponent<AmmoCounter>().LoseAmmo();
        }
    }

    public void UpdateFireModifiers()
    {
        NewItemScriptableObject weapon = GetComponent<WeaponController>().weapon;
        fireRate = weapon.fireRate;
    }

    public void GetFireButtonInput(bool input)
    {
        hasFired = input;
    }
}
