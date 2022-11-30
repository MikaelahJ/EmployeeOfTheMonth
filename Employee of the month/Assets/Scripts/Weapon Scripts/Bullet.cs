using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private float bulletSpeed = 5;
    private Vector3 direction;
    private float damage = 5;
    private Vector3 previousDirection;

    public bool isBouncy = false;
    private int maxBounce = 2;
    private int bounces = 0;

    public bool isPenetrate = false;
    private int maxObjectPass = 1;
    private int objectsPassed = 0;

    [SerializeField] private GameObject explosionPrefab;
    public bool isExplode = false;
    private float explodeRadius = 1;
    private float explosionDamage;

    public bool isHoming = false;
    public float turnSpeed = 1;

    public float knockBackModifier = 10;


    void Start()
    {
        direction = transform.up;
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.velocity = direction * bulletSpeed;
        previousDirection = transform.up;
    }

    private void Update()
    {
        transform.up = rb2d.velocity;
    }

    public void UpdateBulletModifyers(NewItemScriptableObject weapon)
    {
        bulletSpeed = weapon.bulletVelocity;
        damage = weapon.weaponDamage;
        isBouncy = weapon.isBouncy;
        maxBounce = weapon.numOfBounces;
        isPenetrate = weapon.isPenetrate;
        maxObjectPass = weapon.numOfPenetrations;
        isExplode = weapon.isExplosive;
        explodeRadius = weapon.explosionRadius;
        knockBackModifier = weapon.knockbackModifier;
        explosionDamage = weapon.explosionDamage;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        SendDamage(collision.collider);

        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("AppliedForce");
            ApplyKnockBack(collision);
        }

        if (isExplode)
        {
            Explode(transform.position);
            return;
        }

        if (isBouncy && bounces < maxBounce && !collision.gameObject.transform.parent.CompareTag("Player"))
        {
            if (isPenetrate && objectsPassed < maxObjectPass && !collision.gameObject.CompareTag("HardWall"))
            {
                Penetrate();

                return;
            }
            else
            {
                Bounce(collision.GetContact(0));

                if (collision.gameObject.transform.parent.CompareTag("Player"))
                {
                    Debug.Log("AppliedForce");
                    ApplyKnockBack(collision);
                }
                return;
            }
        }

        if (isPenetrate && objectsPassed < maxObjectPass && !collision.gameObject.CompareTag("HardWall"))
        {
            Penetrate();
            AudioSource.PlayClipAtPoint(AudioManager.instance.audioClips.impact_glass, transform.position);
            return;
        }
        if (isExplode)
        {
            Explode(transform.position);
            Destroy(gameObject);
        }
        else
        {
            AudioSource.PlayClipAtPoint(AudioManager.instance.audioClips.impact_wall, transform.position);
            Destroy(gameObject);
        }
    }

    private void ApplyKnockBack(Collision2D playerCollider)
    {
        Rigidbody2D playerRb = playerCollider.gameObject.GetComponent<Rigidbody2D>();
        playerRb.AddForce(rb2d.velocity.normalized * knockBackModifier, ForceMode2D.Impulse);
    }


    private void SendDamage(Collider2D collider)
    {
        Debug.Log(damage);
        if (collider.transform.transform.parent.GetComponent<HasHealth>() != null)
        {
            collider.transform.transform.parent.GetComponent<HasHealth>().LoseHealth(damage);
        }
        if(collider.transform.GetComponent<HasHealth>() != null)
        {
            collider.transform.GetComponent<HasHealth>().LoseHealth(damage);
            collider.gameObject.GetComponent<HasHealth>().AddBlood(gameObject);
        }
    }

    private void Explode(Vector2 collisionPoint)
    {
        damage += explosionDamage;
        Collider2D[] targetsInRadius = Physics2D.OverlapCircleAll(collisionPoint, explodeRadius);
        var explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(explosion, 1f);

        foreach (var target in targetsInRadius)
        {
            SendDamage(target);
        }
    }

    private void Bounce(ContactPoint2D collisionPoint)
    {
        bounces++;

        rb2d.sharedMaterial.bounciness = 1;

        //direction = Vector3.Reflect(direction, collisionPoint.normal);

        //rb2d.velocity = (direction).normalized * bulletSpeed;

        //float angle = Mathf.Atan2(rb2d.velocity.y, rb2d.velocity.x) * Mathf.Rad2Deg + 90;
        //Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);
        //transform.rotation = rot;
    }

    private void Penetrate()
    {
        objectsPassed++;

        int bulletPenetrateLayer = LayerMask.NameToLayer("BulletPenetrate");
        gameObject.layer = bulletPenetrateLayer;

        rb2d.velocity = direction * bulletSpeed;

        if (objectsPassed == maxObjectPass)
        {
            Invoke(nameof(ChangeLayer), 0.5f);
        }
    }

    private void ChangeLayer()
    {
        int defaultLayer = LayerMask.NameToLayer("Default");
        gameObject.layer = defaultLayer;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        
        previousDirection = transform.up;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isHoming && collision.gameObject.CompareTag("Player"))
        {
            Vector3 newDirection = collision.transform.position - transform.position;
            newDirection.Normalize();
            newDirection.z = 0;
            transform.up = Vector2.Lerp(previousDirection, newDirection, turnSpeed * Time.deltaTime);
            previousDirection = transform.up;
            rb2d.velocity = transform.up * rb2d.velocity.magnitude;
        }
    }

}
