using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponModifyerItem : MonoBehaviour
{
    public NewItemScriptableObject itemType;

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = itemType.sprite;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.transform.parent.transform.CompareTag("Player"))
        {
            if (collision.gameObject.transform.parent.transform.GetComponentInChildren<WeaponController>().AddItem(itemType))
            {
                Destroy(this.gameObject);
            }
            Debug.Log(itemType);
        }
    }
}
