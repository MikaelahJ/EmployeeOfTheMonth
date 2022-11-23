using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponModifyerItem : MonoBehaviour
{
    public NewItemScriptableObject rubber;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(this.gameObject);

            //TODO add modifyer stuff here
        }
    }
}
