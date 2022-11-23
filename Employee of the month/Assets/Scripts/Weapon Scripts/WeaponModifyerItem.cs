using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponModifyerItem : MonoBehaviour
{
    public NewItemScriptableObject itemType;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(this.gameObject);
            collision.gameObject.GetComponentInChildren<WeaponController>().AddItem(itemType);
            Debug.Log(itemType);
        }
    }
}
