using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb2d;
    public float bulletSpeed = 10;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.velocity = transform.up * bulletSpeed;
        Destroy(this.gameObject, 10);
    }

    void Update()
    {
        
    }
}
