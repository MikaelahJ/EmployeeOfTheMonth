using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponModifyerItem : MonoBehaviour
{
    public NewItemScriptableObject itemType;
    [SerializeField] [Range(1, 30)] float respawnTime;

    private Vector3 startScale;
    [SerializeField] [Range(0, 1)] float scaleOccilation = 0.05f;
    [SerializeField] float speedOccilation = 3f;

    private float timer = 0f;

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = itemType.sprite;
        startScale = transform.localScale;

        timer = Random.Range(0, 360f);
}

    private void Update()
    {
        transform.localScale = startScale + scaleOccilation * startScale * Mathf.Sin(timer* speedOccilation);

        timer += Time.deltaTime;
        timer %= 360f;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if (collision.gameObject.transform.CompareTag("Player"))
        {
            WeaponController weaponController = collision.gameObject.transform.parent.transform.GetComponentInChildren<WeaponController>();
            if (weaponController.CanAddItem())
            {
                weaponController.AddItem(itemType);
                Disable();
                Invoke(nameof(Enable), respawnTime);
            }
        }
    }

    private void Enable()
    {
        if(itemType.onRespawn != null)
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
