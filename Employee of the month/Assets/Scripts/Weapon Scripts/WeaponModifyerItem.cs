using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponModifyerItem : MonoBehaviour
{
    public NewItemScriptableObject itemType;
    [SerializeField] [Range(1, 30)] float respawnTime;

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = itemType.sprite;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.transform.CompareTag("Player"))
        {
            if (collision.gameObject.transform.parent.transform.GetComponentInChildren<WeaponController>().AddItem(itemType))
            {
                Disable();
                Invoke(nameof(Enable), respawnTime);
            }
            Debug.Log(itemType);
        }
    }

    private void Enable()
    {
        if(itemType.onRespawn != null)
            AudioSource.PlayClipAtPoint(itemType.onRespawn, transform.position);
        GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<SpriteRenderer>().enabled = true;
    }
    private void Disable()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
    }
}
