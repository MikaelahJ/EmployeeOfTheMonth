using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private ControllerInput controllerInput;

    private WeaponController weaponController;
    public UIAmmoCounter ammoCounter;

    private float timer;

    private int ammo = 0;
    private float fireRate = 0.5f;
    private bool hasFired;

    private float accuracyPercentage = 1f;
    private float bulletSpreadPercentage = 0f;

    private bool isShotgun = false;
    private int shotgunAmmount = 3;
    private float shotgunSpreadBetween = 5;

    public bool isMicrowave = false;
    private float trailLength = 5;

    private AudioSource sound;

    void Start()
    {
        weaponController = GetComponent<WeaponController>();
        sound = GetComponent<AudioSource>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (!hasFired) { return; }

        if (timer < fireRate) { return; }

        if (ammo <= 0)
        {
            //Out Of ammo sound
            sound.PlayOneShot(AudioManager.instance.audioClips.emptyMag);
            timer = 0;
            return;
        }

        if (isShotgun)
            FireShotgun();

        if (isMicrowave)
            FireLaser();


        else
        {
            FireGun();
        }
        //Ammo counter
        if (ammoCounter != null)
        {
            ammoCounter.SetAmmo(ammo);
        }

        timer = 0;
    }

    void FireGun()
    {
        //Fire bullet
        GameObject newBullet = Instantiate(bulletPrefab, firePoint.position, transform.rotation);
        newBullet.GetComponent<Bullet>().UpdateBulletModifyers(weaponController.weapon);
        LoseAmmo(1);

        //Bullet Spread
        float spread = bulletSpreadPercentage * (1 - accuracyPercentage);
        newBullet.transform.Rotate(new Vector3(0, 0, Random.Range(-spread, spread)));

        //Play Fire Sound
        sound.PlayOneShot(AudioManager.instance.audioClips.fire);
    }

    private void FireShotgun()
    {
        LoseAmmo(1);
        //3,6,9 skott
        switch (shotgunAmmount)
        {
            case 3:
                shotgunSpreadBetween = 5;
                break;
            case 6:
                shotgunSpreadBetween = 7;
                break;
            case 9:
                shotgunSpreadBetween = 9;
                break;
        }
        for (int i = 0; i < shotgunAmmount; i++)
        {
            var newBullet = Instantiate(bulletPrefab, firePoint.position, transform.rotation);
            newBullet.GetComponent<Bullet>().UpdateBulletModifyers(weaponController.weapon);

            newBullet.transform.Rotate(new Vector3(0, 0, -shotgunSpreadBetween + i * 5));
        }

        //Play Shotgun Sound
        sound.PlayOneShot(AudioManager.instance.audioClips.shotgun);
    }
    private void FireLaser()
    {

    }
    private void FireMicrowave()
    {
        Vector3 right = firePoint.transform.right;

        for (int i = 0; i < 5; i++)
        {
            float offset = -0.2f + (0.1f * i);
            Vector3 position = firePoint.position + right * offset;

            var newBullet = Instantiate(bulletPrefab, position, transform.rotation);
            newBullet.GetComponent<Bullet>().UpdateBulletModifyers(weaponController.weapon);
        }
    }

    private void LoseAmmo(int shots)
    {
        ammo -= shots;
        weaponController.LoseItemAmmo(shots);
        ammoCounter.SetAmmo(ammo);
    }

    public void UpdateFireModifiers()
    {
        NewItemScriptableObject weapon = weaponController.weapon;
        ammo = weapon.ammo;
        fireRate = weapon.fireRate;
        accuracyPercentage = weapon.accuracyPercentage;
        bulletSpreadPercentage = weapon.bulletSpreadPercentage;
        isShotgun = weapon.isShotgun;
        shotgunAmmount = weapon.shotgunAmmount;
        isMicrowave = weapon.isMicrowave;
        ammoCounter.SetAmmo(ammo);
    }

    public void GetFireButtonInput(bool input)
    {
        hasFired = input;
    }
}
