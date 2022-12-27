using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject animationPoint;
    [SerializeField] private Transform firePoint;
    [SerializeField] private ControllerInput controllerInput;
    [SerializeField] private GameObject laser;
    [SerializeField] private Laser laserScript;
    [SerializeField] private GameObject noAmmoIcon;

    private Animator fireAnimation;
    private WeaponController weaponController;
    private Collider2D ownerCollider;

    private float timer;

    private float ammo = 0;
    private float ammoWeight = 1;
    private float fireRate = 0.5f;
    private bool hasFired;
    public float recoil;

    private float accuracy = 1f;
    private float maxMissDegAngle = 0f;

    private bool isShotgun = false;
    private int shotgunAmount = 3;
    private float shotgunSpreadBetween = 5;

    public bool isSuperMicro = false;


    //private bool isInWall = false;

    private AudioSource sound;
    private AudioSource laserSound;
    private Vector3 startFirePoint;

    void Start()
    {
        fireAnimation = animationPoint.GetComponent<Animator>();
        weaponController = GetComponent<WeaponController>();
        ownerCollider = GetComponentInParent<CircleCollider2D>();

        sound = GetComponent<AudioSource>();
        
        startFirePoint = firePoint.transform.localPosition;
        laser.SetActive(false);
    }

    void Update()
    {
        timer += Time.deltaTime;

        //if (isInWall) { return; }

        if (!hasFired) { return; }

        if (isSuperMicro)
        {
            if (!laserScript.isCharged && !laserScript.isCharging && !laserScript.isShooting)
                ChargeSuperMicro();

            else if (!laserScript.isCharging && laserScript.isCharged && !laserScript.isShooting)
                FireSuperMicro();

            return;
        }

        if (timer < 1/fireRate) { return; }

        //set volume to selected volume in options
        sound.volume = AudioManager.instance.audioClips.sfxVolume;

        if (ammo <= 0f)
        {
            //Out Of ammo sound
            sound.PlayOneShot(AudioManager.instance.audioClips.emptyMag);
            var noAmmo = Instantiate(noAmmoIcon, firePoint);
            Destroy(noAmmo, 0.5f);
            timer = 0;
            return;
        }

        ApplyRecoil();
        
        if(weaponController.weapon.isPenetrate && !weaponController.weapon.isExplosive)
        {
            sound.clip = weaponController.weapon.fire;
            sound.Play();
        }
        else
        {
            sound.PlayOneShot(weaponController.weapon.fire);
        }

        if (isShotgun)
            FireShotgun();

        else
            FireGun();

        timer = 0;
    }

    void FireGun()
    {
        //Fire bullet
        GameObject newBullet = Instantiate(bulletPrefab, firePoint.position, transform.rotation);
        newBullet.GetComponent<Bullet>().UpdateBulletModifyers(weaponController.weapon);
        newBullet.GetComponent<Bullet>().bulletOwner = ownerCollider;


        //Bullet Spread
        float spread = maxMissDegAngle * (1 - accuracy / 100);
        newBullet.transform.Rotate(new Vector3(0, 0, Random.Range(-spread, spread)));

        //Play fire animation
        fireAnimation.SetTrigger("fireGun");

        LoseAmmo();
    }

    private void ChargeSuperMicro()
    {
        if(laserSound == null)
            laserSound = gameObject.AddComponent<AudioSource>();
        laserSound.volume = AudioManager.instance.audioClips.sfxVolume;
        laserSound.clip = AudioManager.instance.audioClips.laserCharge;
        laserSound.Play();

        laserScript.PowerUpLaser();
    }

    private void FireSuperMicro()
    {
        laserSound.volume = AudioManager.instance.audioClips.sfxVolume;
        laserSound.clip = weaponController.weapon.ultimateFire;
        laserSound.Play();

        laser.SetActive(true);
        laserScript.FireLaser();
    }

    public void DeActivateLaser()
    {
        laser.SetActive(false);
        LoseAmmo();
    }

    private void FireShotgun()
    {
        
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
            newBullet.GetComponent<Bullet>().bulletOwner = ownerCollider;
            newBullet.transform.Rotate(new Vector3(0, 0, -shotgunSpreadBetween + i * 5));
        }

        //Play shotgun fire animation
        fireAnimation.SetTrigger("fireShotgun");
        LoseAmmo();
    }


    private void LoseAmmo()
    {
        LoseAmmo(1);
    }
    private void LoseAmmo(int shots)
    {
        float ammoLost = shots * ammoWeight;
        ammo -= (ammoLost * weaponController.NumOfItems());
        weaponController.LoseItemAmmo(ammoLost);
        //ammoCounter.SetAmmo(ammo);
    }

    public void UpdateFireModifiers()
    {
        NewItemScriptableObject weapon = weaponController.weapon;
        ammo = weapon.ammo;
        ammoWeight = weapon.ammoWeight;
        fireRate = weapon.baseFireRate;
        accuracy = weapon.accuracy;
        maxMissDegAngle = weapon.maxMissDegAngle;
        isShotgun = weapon.isShotgun;
        isSuperMicro = weapon.isSuperMicro;
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
            GetComponentInChildren<AimLine>().isBlocked = true;
            firePoint.transform.localPosition = new Vector3(0, -startFirePoint.y, 0);
            //isInWall = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        firePoint.transform.localPosition = startFirePoint;
        GetComponentInChildren<AimLine>().isBlocked = false;
        //isInWall = false;
    }

    public void FadeLaserSound()
    {
        StartCoroutine(FadeLaser());
    }
    private IEnumerator FadeLaser()
    {
        float startVolume = laserSound.volume;
        
        while(laserSound.volume != 0)
        {
            laserSound.volume -= startVolume * Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        Debug.Log("Sound Stopped!");
    }
}
