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
            Instantiate(bulletPrefab, firePoint.position, transform.rotation);
            timer = 0;
        }
    }
}
