using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIfPenetrate : MonoBehaviour
{
    [SerializeField] private Bullet bulletScript;
    private Bullet bullet;

    private void Start()
    {
        bullet = GetComponentInParent<Bullet>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Untagged"))
        {
            bulletScript.SendDamage(collider);
            AudioSource.PlayClipAtPoint(AudioManager.instance.audioClips.impact_wood, transform.position, AudioManager.instance.audioClips.sfxVolume);
        }

        if (collider.gameObject.CompareTag("Player"))
        {
            bulletScript.SendDamage(collider);
            bullet.DisableTracking();
        }

        if (collider.gameObject.CompareTag("SoftWall"))
        {
            AudioSource.PlayClipAtPoint(AudioManager.instance.audioClips.impact_glass, transform.position, AudioManager.instance.audioClips.sfxVolume);
            bulletScript.SendDamage(collider);
        }
    }
}
