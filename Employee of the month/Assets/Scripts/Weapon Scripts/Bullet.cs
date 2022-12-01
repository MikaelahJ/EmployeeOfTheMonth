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

    public bool isBouncy = false;
    private int maxBounce = 2;
    private int bounces = 0;

    public bool isPenetrate = false;
    private int maxObjectPass = 1;
    private int objectsPassed = 0;

    [SerializeField] private GameObject explosionPrefab;
    public bool isMicrowave = false;
    private float explodeRadius = 1;
    private float explosionDamage;

    private float trailLength = 5;
    private int radiationDamage = 2;

    public float knockBackModifier = 10;

    public bool isHoming = false;
    public float turnSpeed = 1;


    void Start()
    {
        direction = transform.up;
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.velocity = direction * bulletSpeed;
    }

    private void FixedUpdate()
    {
        var hit = Physics2D.Raycast(transform.position, -transform.up, trailLength);
        Debug.DrawRay(transform.position, -transform.up, Color.red);

        if (hit.collider != null)
        {
            Debug.Log("hit: " + hit.collider);
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                Debug.Log("Radiation");
                SendRadiationDamage(hit.collider);
            }
        }
    }
    private void SendRadiationDamage(Collider2D collider)
    {
        collider.transform.parent.transform.GetComponent<HasHealth>().HealthChange(radiationDamage, 0.5f, 3f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        SendDamage(collision.collider, collision);

        //Bounce
        if (isBouncy && bounces < maxBounce)
        {
            //Penetrate soft walls
            if (isPenetrate && objectsPassed < maxObjectPass && !collision.gameObject.CompareTag("HardWall"))
            {
                Penetrate();
                return;
            }
            //Bounce on other
            else
            {
                Bounce();

                if (collision.gameObject.CompareTag("Player"))
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

        if (isMicrowave)
        {
            Explode(transform.position, collision);
            Destroy(gameObject);
        }
        else
        {
            AudioSource.PlayClipAtPoint(AudioManager.instance.audioClips.impact_wall, transform.position);
            Destroy(gameObject);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        transform.up = rb2d.velocity;
    }

    private void SendDamage(Collider2D collider, Collision2D collision)
    {
        if (collider.gameObject.transform.CompareTag("Player"))
        {
            if (collider.transform.GetComponent<HasHealth>() != null)
            {
                collider.transform.GetComponent<HasHealth>().LoseHealth(damage);

            }
            else
            {
                collider.transform.parent.transform.GetComponent<HasHealth>().LoseHealth(damage);
                ApplyKnockBack(collision);
            }

        }

        if (collider.transform.GetComponent<HasHealth>() != null)
        {
            collider.transform.GetComponent<HasHealth>().LoseHealth(damage);
        }
    }

    private void Explode(Vector2 collisionPoint, Collision2D collision)
    {
        damage += explosionDamage;
        Collider2D[] targetsInRadius = Physics2D.OverlapCircleAll(collisionPoint, explodeRadius);
        var explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(explosion, 1f);
        AudioSource.PlayClipAtPoint(AudioManager.instance.audioClips.bulletExplode, transform.position);

        foreach (var target in targetsInRadius)
        {
            SendDamage(target, collision);
        }

    }

    private void ApplyKnockBack(Collision2D playerCollider)
    {
        Rigidbody2D playerRb = playerCollider.gameObject.GetComponent<Rigidbody2D>();
        playerRb.AddForce(rb2d.velocity.normalized * knockBackModifier, ForceMode2D.Impulse);
    }

    private void Bounce()
    {
        bounces++;

        rb2d.sharedMaterial.bounciness = 1;

        AudioSource.PlayClipAtPoint(AudioManager.instance.audioClips.bulletBounce, transform.position);
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isHoming && collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Homing Triggered");
            Vector3 newDirection = collision.transform.position - transform.position;
            newDirection.Normalize();
            newDirection.z = 0;
            transform.up = Vector2.Lerp(direction, newDirection, turnSpeed * Time.deltaTime);
            direction = transform.up;
            rb2d.velocity = transform.up * bulletSpeed;
        }
    }
    public void UpdateBulletModifyers(NewItemScriptableObject weapon)
    {
        bulletSpeed = weapon.bulletVelocity;
        damage = weapon.weaponDamage;
        isBouncy = weapon.isBouncy;
        maxBounce = weapon.numOfBounces;
        isPenetrate = weapon.isPenetrate;
        maxObjectPass = weapon.numOfPenetrations;
        isMicrowave = weapon.isMicrowave;
        explodeRadius = weapon.explosionRadius;
        explosionDamage = weapon.explosionDamage;
        knockBackModifier = weapon.knockbackModifier;
    }
}
