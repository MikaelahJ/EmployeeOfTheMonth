using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private ControllerInput controllerInput;

    private WeaponController weaponController;
    //public UIAmmoCounter ammoCounter;

    private float timer;

    private int ammo = 0;
    private float fireRate = 0.5f;
    private bool hasFired;
    public float recoil;

    private float accuracy = 1f;
    private float maxMissDegAngle = 0f;

    private bool isShotgun = false;
    private int shotgunAmount = 3;
    private float shotgunSpreadBetween = 5;

    //private bool isInWall = false;

    private AudioSource sound;
    private Vector3 startFirePoint;

    void Start()
    {
        weaponController = GetComponent<WeaponController>();
        sound = GetComponent<AudioSource>();
        startFirePoint = firePoint.transform.localPosition;
    }

    void Update()
    {
        timer += Time.deltaTime;

        //if (isInWall) { return; }

        if (!hasFired) { return; }

        if (timer < fireRate) { return; }

        if (ammo <= 0)
        {
            //Out Of ammo sound
            sound.PlayOneShot(AudioManager.instance.audioClips.emptyMag);
            timer = 0;
            return;
        }

        ApplyRecoil();

        if (isShotgun)
        {
            FireShotgun();
        }
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

        //Bullet Spread
        float spread = maxMissDegAngle * (1 - accuracy / 100);
        newBullet.transform.Rotate(new Vector3(0, 0, Random.Range(-spread, spread)));
    }

    private void FireShotgun()
    {
        LoseAmmo(1);
        //3,5,9 skott
        switch (shotgunAmount)
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
        for (int i = 0; i < shotgunAmount; i++)
        {
            GameObject newBullet = Instantiate(bulletPrefab, firePoint.position, transform.rotation);
            newBullet.GetComponent<Bullet>().UpdateBulletModifyers(weaponController.weapon);

            newBullet.transform.Rotate(new Vector3(0, 0, -shotgunSpreadBetween + i * 5));
        }
    }

    private void LoseAmmo(int shots)
    {
        ammo -= (shots * weaponController.NumOfItems());
        weaponController.LoseItemAmmo(shots);
        //ammoCounter.SetAmmo(ammo);
    }

    public void UpdateFireModifiers()
    {
        NewItemScriptableObject weapon = weaponController.weapon;
        ammo = weapon.ammo;
        fireRate = weapon.fireRate;
        accuracy = weapon.accuracy;
        maxMissDegAngle = weapon.maxMissDegAngle;
        isShotgun = weapon.isShotgun;
        shotgunAmount = weapon.shotgunAmount;
        //ammoCounter.SetAmmo(ammo);
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
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("HardWall") || collision.gameObject.CompareTag("SoftWall"))
        {
            firePoint.transform.localPosition = new Vector3(0, -startFirePoint.y, 0);
            //isInWall = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        firePoint.transform.localPosition = startFirePoint;
        //isInWall = false;
    }
}
