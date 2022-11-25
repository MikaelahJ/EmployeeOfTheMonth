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

    public bool isExplode = false;
    private float explodeRadius = 1;

    void Start()
    {
        direction = transform.up;
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.velocity = direction * bulletSpeed;
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
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        SendDamage(collision.collider);

        if (isExplode)
        {
            Explode(transform.position);
            return;
        }

        if (isBouncy && bounces < maxBounce && !collision.gameObject.CompareTag("Player"))
        {
            if (isPenetrate && objectsPassed < maxObjectPass && !collision.gameObject.CompareTag("HardWall"))
            {   
                Penetrate();

                return;
            }
            else
            {
                Bounce(collision.GetContact(0));

                if (collision.gameObject.CompareTag("Player"))
                {
                    //send knockback
                }

                return;
            }
        }

        if (isPenetrate && objectsPassed < maxObjectPass && !collision.gameObject.CompareTag("HardWall"))
        {
            Penetrate();
            return;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void SendDamage(Collider2D collider)
    {
        Debug.Log(damage);
        if (collider.transform.GetComponent<HasHealth>() != null)
        {
            collider.transform.GetComponent<HasHealth>().LoseHealth(damage);
        }
    }

    private void Explode(Vector2 collisionPoint)
    {
        Collider2D[] targetsInRadius = Physics2D.OverlapCircleAll(collisionPoint, explodeRadius);
        foreach (var target in targetsInRadius)
        {
            SendDamage(target);
        }
    }

    private void Bounce(ContactPoint2D collisionPoint)
    {
        bounces++;

        direction = Vector3.Reflect(direction, collisionPoint.normal);
        rb2d.velocity = (direction).normalized * bulletSpeed;

        float angle = Mathf.Atan2(rb2d.velocity.y, rb2d.velocity.x) * Mathf.Rad2Deg + 90;
        Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = rot;
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
}
