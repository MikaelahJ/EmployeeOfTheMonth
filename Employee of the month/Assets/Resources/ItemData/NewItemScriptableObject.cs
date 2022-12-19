using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "ScriptableObjects/NewItemScriptableObject", order = 1)]

//Add variables in the list below, please set the value to 0 or equivalent
//After you added a variable, update the UpdateWeaponStats Function in WeaponController
public class NewItemScriptableObject : ScriptableObject
{
    [Header("Item On Weapon Sprite")]
    public Sprite itemIcon;
    public Sprite itemBrokenOnGround;
    public bool brokenItemSticksToWall = false;

    [Header("In Game Sprite")]
    public Sprite sprite;

    [Header("Sounds")]
    public AudioClip onRespawn;
    public AudioClip onPickup;
    public AudioClip onDestroy;
    [Tooltip("Will always play if you have three of this item")]
    public AudioClip ultimateFire;
    public AudioClip fire;
    [Tooltip("Highest priority overrides the current fire sound")]
    public int fireSoundPriority = 0;
    public AudioClip bulletImpactSound;
    [Tooltip("Highest priority overrides the current bullet impact sound")]
    public int bulletImpactPriority = 0;

    [Header("Weapon modifiers")]
    [Tooltip("Total Ammo before item breaks")]
    public float ammo = 0;
    [Tooltip("ammo consumed per shot, ammoweight of 0.5 fires 2 bullets per 1 ammo. Three ammoweight 0.5 mods gives 0.125 ammoweight, 8 shots per 1 ammo. Three ammoweight 1.5 mods gives 3.4 ammoweight, 1 shot per 3.4 ammo")]
    public float ammoWeight = 1;
    [Tooltip("modifier is additive")]
    public float weaponDamage = 0;
    [Tooltip("modifier is additive")]
    public float recoilModifier = 0;
    [Tooltip("Base rate of Bullets fired per second, can only be set on base weapon")]
    public float baseFireRate = 0;
    [Tooltip("% Increase of fire rate, use negative values for a decrease in fire rate")]
    public float fireRatePercentage = 0;
    [Tooltip("3 items with 100% accuracy gives weapon 100% accuracy, 3 items with 80% accuracy gives 0.8*0.8*0.8 = 51% accuracy")]
    [Range(0, 100)]public float accuracy = 100;
    [Tooltip("The spread of missed bullets, 0% accuracy with a 45 degree miss angle allows you to miss in a 90 degree cone")]
    [Range(0, 90)] public float maxMissDegAngle = 0;
    public bool isShotgun = false;
    public int shotgunAmount = 0;

    [Header("Bullet modifiers")]
    public Sprite bulletSprite;
    [Tooltip("Highest priority overrides the current bullet sprite")]
    public int bulletSpritePriority = 0;

    [Tooltip("modifier is additive")]
    public float bulletVelocity = 0f;
    [Header("Bouncy")]
    public bool isBouncy = false;
    public int numOfBounces = 0;
    [Header("Penetration")]
    public bool isPenetrate = false;
    public int numOfPenetrations = 0;
    [Header("Explosive")]
    public bool isSuperMicro = false;
    public bool isExplosive = false;
    public float explosionRadius = 0f;
    public float explosionDamage = 0f;
    [Header("Knockback")]
    public bool isKnockback = false;
    public float knockbackModifier = 0f;
    [Header("Homing")]
    public bool isHoming = false;
    public float turnSpeed = 0f;
    public float scanBounds = 0f;
    [Header("Stapler")]
    public bool isStapler = false;
    public float stunTime = 0f;
    [Header("Animations")]
    public bool hasAnimations = false;
}