using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponModifyerItem : MonoBehaviour
{
    public GameObject ItemGoingToWeaponSlot;

    public GameObject pickupTextPrefab;

    private GameObject pickupTextObject;

    public NewItemScriptableObject itemType;
    [SerializeField] [Range(1, 30)] float respawnTime;

    private Vector3 startScale;
    [SerializeField] [Range(0, 1)] float scaleOccilation = 0.05f;
    [SerializeField] float speedOccilation = 3f;

    private float timer = 0f;

    private Transform parentPos;

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = itemType.sprite;
        startScale = transform.localScale;

        timer = Random.Range(0, 360f);
    }

    private void Update()
    {
        transform.localScale = startScale + scaleOccilation * startScale * Mathf.Sin(timer * speedOccilation);

        timer += Time.deltaTime;
        timer %= 360f;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.transform.CompareTag("Player"))
        {
            GameObject weapon = FindWeapon(collision.transform);
            WeaponController weaponController = weapon.GetComponent<WeaponController>();
            (bool, int) emptyWeaponSlot = weaponController.CanAddItem();
            if (emptyWeaponSlot.Item1)
            {
                //weaponController.AddItem(itemType);

                //Adds pickuptext over player
                pickupTextObject = Instantiate(pickupTextPrefab, collision.transform.parent.transform);
                pickupTextObject.GetComponent<PickupText>().ActivateText(itemType.name);

                SpawnItemGoingToWeaponSlot(weapon, emptyWeaponSlot.Item2);
                //Disable();
                //Invoke(nameof(Enable), respawnTime);

                Disable();
                Invoke(nameof(RespawnItem), respawnTime);

            }
        }
    }

    private void RespawnItem()
    {
        itemType = GetComponentInParent<ItemSpawner>().RespawnItem();
        Start();
        Enable();
    }

    public GameObject FindWeapon(Transform parent)
    {
        foreach (Transform child in parent)
        {
            if (child.gameObject.TryGetComponent<WeaponController>(out WeaponController weaponController))
            {
                Debug.Log("Found Weapon " + child.gameObject.name);
                return child.gameObject;
            }
        }
        Debug.Log("No weapon could be found!, this will create an error lol");
        return null;
    }

    public void SpawnItemGoingToWeaponSlot(GameObject targetWeapon, int index)
    {
        GameObject newItem = Instantiate(ItemGoingToWeaponSlot, transform.position, transform.rotation);
        newItem.GetComponent<ItemGoingToWeaponSlot>().itemIndex = index;
        newItem.GetComponent<ItemGoingToWeaponSlot>().item = itemType;
        newItem.GetComponent<ItemGoingToWeaponSlot>().targetWeapon = targetWeapon;
    }

    private void Enable()
    {
        if (itemType.onRespawn != null)
            AudioSource.PlayClipAtPoint(itemType.onRespawn, transform.position, AudioManager.instance.audioClips.sfxVolume);

        GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<SpriteRenderer>().enabled = true;
    }
    private void Disable()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
    }
}
