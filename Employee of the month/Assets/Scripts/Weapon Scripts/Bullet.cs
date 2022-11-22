using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private float bulletSpeed = 5;
    private Vector3 direction;
    private int damage;

    public bool isBouncy = false;
    public int maxBounce = 2;
    private int bounces = 0;

    public bool isPenetrate = false;
    public int maxObjectPass = 1;
    private int objectsPassed = 0;

    public bool isExplode = true;
    private float explodeRadius = 1;


    void Start()
    {
        direction = transform.up;
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.velocity = direction * bulletSpeed;
    }
    private void UpdateBulletModifyers(NewItemScriptableObject weapon)
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isExplode)
        {
            Explode();
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

    private void Explode()
    {
        throw new NotImplementedException();
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

    }
}
