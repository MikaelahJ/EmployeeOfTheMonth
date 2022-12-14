using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGoingToWeaponSlot : MonoBehaviour
{
    public NewItemScriptableObject item;
    public GameObject targetWeapon;
    public int itemIndex;
    public float baseSpeed = 0.1f;
    public float acceleration = 1;

    private GameObject itemSlot;
    GameObject itemHolder;
    Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = item.sprite;
        rb2d = GetComponent<Rigidbody2D>();
        itemHolder = FindItemHolder(targetWeapon.transform);
        itemSlot = itemHolder.transform.GetChild(itemIndex).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        MoveTowardsTarget();
    }

    public void MoveTowardsTarget()
    {
        Vector3 itemSlotPosition = itemSlot.transform.position;
        Vector3 moveDirection = itemSlotPosition - transform.position;

        if(moveDirection.magnitude < 0.2f)
        {
            targetWeapon.GetComponent<WeaponController>().AddItem(item);
            Destroy(gameObject);
        }

        transform.position += baseSpeed * Time.deltaTime * moveDirection.normalized;
        baseSpeed += Time.deltaTime * acceleration;

        //rb2d.AddForce(speed * Time.deltaTime * moveDirection.normalized, ForceMode2D.Impulse);
        //transform.up = moveDirection;
    }

    public GameObject FindItemHolder(Transform parent)
    {
        foreach (Transform child in parent)
        {
            if (child.gameObject.TryGetComponent<UIItemHolder>(out _))
            {
                Debug.Log("Found ItemHolder " + child.gameObject.name);
                return child.gameObject;
            }
        }
        Debug.Log("No ItemHolder could be found!, this will create an error lol");
        return null;
    }
}
