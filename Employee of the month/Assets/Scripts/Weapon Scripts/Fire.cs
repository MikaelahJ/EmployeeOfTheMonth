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
    private float fireRate = 0.2f;

    void Update()
    {
        timer += Time.deltaTime;

        if (!controllerInput.HasFired) { return; }

        if(timer < fireRate) { return; }

        if(ammoCounter != null)
            if(ammoCounter.GetComponent<AmmoCounter>().currentAmmo == 0) { return; }

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

    void UpdateFireModifiers()
    {
        NewItemScriptableObject weapon = GetComponent<WeaponController>().weapon;
        fireRate = weapon.fireRate;
    }
}
