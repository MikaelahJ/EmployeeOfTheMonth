using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject pencil;
    private Rigidbody2D rb2d;
    private float bulletSpeed = 5;
    private Vector3 direction;
    private float damage = 5;

    public bool isBouncy = false;
    private int maxBounce = 2;
    private int bounces = 0;

    public bool isPenetrate = false;
    //private int maxObjectPass = 1;
    //private int objectsPassed = 0;

    [SerializeField] private GameObject explosionPrefab;
    public bool isExplode = false;
    private float explodeRadius = 1;
    private float explosionDamage;

    public float knockBackModifier = 10;

    public bool isHoming = false;
    public float turnSpeed = 1;

    private bool canTakeDamage = false;

    public AudioClip bulletImpactSound;

    void Start()
    {
        direction = transform.up;
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.velocity = direction * bulletSpeed;
        Invoke(nameof(CanTakeDamage), 0.05f);
    }

    private void CanTakeDamage()
    {
        canTakeDamage = true;
    }

    public void UpdateBulletModifyers(NewItemScriptableObject weapon)
    {
        pencil.GetComponent<SpriteRenderer>().sprite = weapon.bulletSprite;
        bulletSpeed = weapon.bulletVelocity;
        damage = weapon.weaponDamage;
        isBouncy = weapon.isBouncy;
        maxBounce = weapon.numOfBounces;
        isPenetrate = weapon.isPenetrate;
        //maxObjectPass = weapon.numOfPenetrations;
        isExplode = weapon.isExplosive;
        explodeRadius = weapon.explosionRadius;
        explosionDamage = weapon.explosionDamage;
        knockBackModifier = weapon.knockbackModifier;
        bulletImpactSound = weapon.bulletImpactSound;

        if (isPenetrate)
        {
            pencil.GetComponent<CapsuleCollider2D>().enabled = true;
            Physics2D.IgnoreLayerCollision(3, 9); //köra igenom player och softwall layer
            Physics2D.IgnoreLayerCollision(11, 9);
        }
        else
        {
            pencil.GetComponent<CapsuleCollider2D>().enabled = false;
            Physics2D.IgnoreLayerCollision(3, 9, false);
            Physics2D.IgnoreLayerCollision(11, 9, false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        SendDamage(collision.collider, collision);

        //Bounce
        if (isBouncy && bounces < maxBounce)
        {
            Bounce();

            if (collision.gameObject.CompareTag("Player"))
            {
                Debug.Log("AppliedForce");
                ApplyKnockBack(collision.collider);
            }
            return;

        }

        if (isPenetrate && !collision.gameObject.CompareTag("HardWall"))
        {
            if (collision.gameObject.CompareTag("SoftWall"))
            {
                AudioSource.PlayClipAtPoint(AudioManager.instance.audioClips.impact_glass, transform.position);
            }
            else
            {
                AudioSource.PlayClipAtPoint(AudioManager.instance.audioClips.impact_wood, transform.position);
            }
            return;
        }

        //Play bullet hit sound
        AudioSource.PlayClipAtPoint(bulletImpactSound, transform.position);

        if (isExplode)
        {
            Explode(transform.position, collision);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        transform.up = rb2d.velocity;
    }

    public void SendDamage(Collider2D collider, Collision2D collision = null)
    {
        if (!canTakeDamage) { return; }
        SendDamage(damage, collider, collision);
    }

    public void SendDamage(float damage, Collider2D collider, Collision2D collision = null)
    {
        if (collider.gameObject.transform.CompareTag("Player"))
        {
            if(collider.transform.parent.transform.TryGetComponent<HasHealth>(out HasHealth health))
            {
                health.LoseHealth(damage);
                health.AddBlood(gameObject);
            }

            ApplyKnockBack(collider);
        }

        if (collider.transform.GetComponent<HasHealth>() != null)
        {
            collider.transform.GetComponent<HasHealth>().LoseHealth(damage);
        }
        if (collider.gameObject.GetComponent<ItemBreak>() != null && damage >= 10)
        {
            collider.gameObject.GetComponent<ItemBreak>().TakeDamage();
        }
    }

    private void Explode(Vector2 collisionPoint, Collision2D collision)
    {
        //damage += explosionDamage;
        Collider2D[] targetsInRadius = Physics2D.OverlapCircleAll(collisionPoint, explodeRadius);
        var explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);
        explosion.transform.localScale = new Vector3(explodeRadius, explodeRadius, explodeRadius);
        Destroy(explosion, 1f);

        foreach (var target in targetsInRadius)
        {
            SendDamage(explosionDamage, target, collision);
        }
    }

    private void ApplyKnockBack(Collider2D playerCollider)
    {
        Rigidbody2D playerRb = playerCollider.gameObject.GetComponent<Rigidbody2D>();
        if (playerRb == null) { return; }
        playerRb.AddForce(rb2d.velocity.normalized * knockBackModifier, ForceMode2D.Impulse);
    }

    private void Bounce()
    {
        bounces++;
        rb2d.sharedMaterial.bounciness = 1;

        AudioSource.PlayClipAtPoint(AudioManager.instance.audioClips.bulletBounce, transform.position);
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (isHoming && collider.gameObject.CompareTag("Player"))
        {
            Debug.Log("Homing Triggered");
            Vector3 newDirection = collider.transform.position - transform.position;
            newDirection.Normalize();
            newDirection.z = 0;
            transform.up = Vector2.Lerp(direction, newDirection, turnSpeed * Time.deltaTime);
            direction = transform.up;
            rb2d.velocity = transform.up * bulletSpeed;
        }
    }
}
