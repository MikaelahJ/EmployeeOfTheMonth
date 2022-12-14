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
    public AudioClip ultimateFire;
    public AudioClip fire;
    public int fireSoundPriority = 0;
    public AudioClip bulletImpactSound;
    public int bulletImpactPriority = 0;

    [Header("Weapon modifiers")]
    public int ammo = 0;
    public float weaponDamage = 0;
    public float recoilModifier = 0;
    [Range(-0.15f, 0.5f)]public float fireRate = 0;
    [Range(0, 100)]public float accuracy = 100;
    [Range(0, 90)] public float maxMissDegAngle = 0;
    public bool isShotgun = false;
    public int shotgunAmount = 0;

    [Header("Bullet modifiers")]
    public Sprite bulletSprite;
    public int bulletSpritePriority = 0;

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

    [Header("Animations")]
    public bool hasAnimations = false;
}