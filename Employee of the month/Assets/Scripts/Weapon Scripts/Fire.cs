using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GetControllerInput controllerInput;

    private float timer;
    private float fireRate = 0.2f;

    void Update()
    {
        timer += Time.deltaTime;

        if (controllerInput.HasFired && timer >= fireRate)
        {
            GameObject newBullet = Instantiate(bulletPrefab, firePoint.position, transform.rotation);
            //TODO
            //newBullet.GetComponent<Bullet>().UpdateBulletModifiers(GetComponent<WeaponController>().weapon);

            timer = 0;
        }
    }

    void UpdateFireModifiers()
    {
        NewItemScriptableObject weapon = GetComponent<WeaponController>().weapon;
        fireRate = weapon.fireRate;
    }
}
