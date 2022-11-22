using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "ScriptableObjects/NewItemScriptableObject", order = 1)]
public class NewItemScriptableObject : ScriptableObject
{
    [Header("Item Icon")]
    public Sprite itemIcon;

    [Header("Weapon modifiers")]
    public float fireRate = 0.5f;
    public float recoilModifier = 0;
    

    [Header("Bullet modifiers")]
    public bool isBouncy = false;
    public int numOfBounces = 1;
    public bool isPenetrate = false;
    public bool isExplosive = false;
    public bool isKnockback = false;

    
}