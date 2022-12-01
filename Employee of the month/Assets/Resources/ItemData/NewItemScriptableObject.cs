using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "ScriptableObjects/NewItemScriptableObject", order = 1)]

//Add variables in the list below, please set the value to 0 or equivalent
//After you added a variable, update the UpdateWeaponStats Function in WeaponController
public class NewItemScriptableObject : ScriptableObject
{
    [Header("Item Icon")]
    public Sprite itemIcon;

    [Header("In Game Sprite")]
    public Sprite sprite;

    [Header("Weapon modifiers")]
    public int ammo = 0;
    public float weaponDamage = 0;
    public float fireRate = 0;
    public float recoilModifier = 0;
    public float accuracyPercentage = 1;
    public float bulletSpreadPercentage = 0;
    public bool isShotgun = false;
    public int shotgunAmmount = 0;


    [Header("Bullet modifiers")]
    public float bulletVelocity = 0f;
    [Header("Bouncy")]
    public bool isBouncy = false;
    public int numOfBounces = 0;
    [Header("Penetration")]
    public bool isPenetrate = false;
    public int numOfPenetrations = 0;
    [Header("Explosive")]
    public bool isMicrowave = false;
    public float explosionRadius = 0f;
    public float explosionDamage = 0f;
    [Header("Knockback")]
    public bool isKnockback = false;
    public float knockbackModifier = 0f;
    [Header("Homing")]
    public bool isHoming = false;
    public float turnSpeed = 0f;


}