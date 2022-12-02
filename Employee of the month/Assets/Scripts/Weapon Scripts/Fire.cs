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
    public float recoil;

    private float accuracy = 1f;
    private float maxMissDegAngle = 0f;

    private bool isShotgun = false;
    private int shotgunAmmount = 3;
    private float shotgunSpreadBetween = 5;

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
        else
        {
            FireGun();
        }

        timer = 0;
    }

    void FireGun()
    {
        //Fire bullet
        GameObject newBullet = Instantiate(bulletPrefab, firePoint.position, transform.rotation);
        newBullet.GetComponent<Bullet>().UpdateBulletModifyers(weaponController.weapon);
        LoseAmmo(1);
        ApplyRecoil();

        //Bullet Spread
        float spread = maxMissDegAngle * (1 - accuracy/100);
        newBullet.transform.Rotate(new Vector3(0, 0, Random.Range(-spread, spread)));
        
        //Play Fire Sound
        sound.PlayOneShot(AudioManager.instance.audioClips.fire);

    }

    private void FireShotgun()
    {
        LoseAmmo(1);
        //3,5,9 skott
        Debug.Log(shotgunAmmount);
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
            GameObject newBullet = Instantiate(bulletPrefab, firePoint.position, transform.rotation);
            newBullet.GetComponent<Bullet>().UpdateBulletModifyers(weaponController.weapon);
            

            newBullet.transform.Rotate(new Vector3(0, 0, -shotgunSpreadBetween + i*5));

            //if (i == 0)
            //{
            //}
            //if (i == shotgunAmmount - 1)
            //{
            //    newBullet.transform.Rotate(new Vector3(0, 0, shotgunSpreadBetween));
            //}
        }

        //Play Shotgun Sound
        sound.PlayOneShot(AudioManager.instance.audioClips.shotgun);
    }

    private void LoseAmmo(int shots)
    {
        ammo -= (shots * weaponController.NumOfItems());
        weaponController.LoseItemAmmo(shots);
        ammoCounter.SetAmmo(ammo);

    }

    public void UpdateFireModifiers()
    {
        NewItemScriptableObject weapon = weaponController.weapon;
        ammo = weapon.ammo;
        fireRate = weapon.fireRate;
        accuracy = weapon.accuracy;
        maxMissDegAngle = weapon.maxMissDegAngle;
        isShotgun = weapon.isShotgun;
        shotgunAmmount = weapon.shotgunAmmount;
        ammoCounter.SetAmmo(ammo);
        recoil = weapon.recoilModifier;
    }

    public void GetFireButtonInput(bool input)
    {
        hasFired = input;
    }

    public void ApplyRecoil()
    {
        transform.GetComponentInParent<Rigidbody2D>().AddForce(-transform.up * recoil, ForceMode2D.Impulse);
    }
}
