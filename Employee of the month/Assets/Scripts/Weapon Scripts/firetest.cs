using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class firetest : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;

    private float timer;
    private float fireRate = 0.2f;

    void Update()
    {
        timer += Time.deltaTime;

        if (Input.GetMouseButton(0) && timer >= fireRate)
        {
            GameObject newBullet = Instantiate(bulletPrefab, firePoint.position, transform.rotation);
            //TODO
            newBullet.GetComponent<Bullet>().UpdateBulletModifyers(GetComponent<WeaponController>().weapon);

            timer = 0;
        }
    }

    void UpdateFireModifiers()
    {
        NewItemScriptableObject weapon = GetComponent<WeaponController>().weapon;
        fireRate = weapon.fireRate;
    }
}
