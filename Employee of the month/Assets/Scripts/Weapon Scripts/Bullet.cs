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
    public bool isExplode = false;
    private float explodeRadius = 1;
    private float explosionDamage;

    public float knockBackModifier = 10;

    public bool isHoming = false;
    public float turnSpeed = 1;
    private Vector2 previousDirection;
    public Vector3 closest; //The position of the closest Player
    public  float range; //Current range to closest Player
    public List<Collider2D> targetsInRange;


    void Start()
    {
        direction = transform.up;
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.velocity = direction * bulletSpeed;
        previousDirection = transform.up;
        targetsInRange = new List<Collider2D>();
        range = 0;
    }

    private void Update()
    {
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
        explosionDamage = weapon.explosionDamage;
        knockBackModifier = weapon.knockbackModifier;
        isHoming = weapon.isHoming;
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

                if (isHoming)
                {
                    transform.up = rb2d.velocity;
                    previousDirection = transform.up;
                    isHoming = false;
                    StartCoroutine(ToggleSeeking()); //Turn off homing on player to get new bullet direction
                }

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

        if (isExplode)
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
            collider.transform.parent.transform.GetComponent<HasHealth>().LoseHealth(damage);
            collider.transform.parent.transform.GetComponent<HasHealth>().AddBlood(gameObject);
        }

        if(collider.transform.GetComponent<HasHealth>() != null)
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

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (isHoming && collision.gameObject.CompareTag("Player"))
    //    {
    //        float bulletBoundsY = GetComponent<CapsuleCollider2D>().bounds.extents.y + 0.1f;
    //        Vector2 startPos = new Vector2(transform.position.x, transform.position.y + bulletBoundsY);
    //        Vector2 deltaPos = collision.transform.position - transform.position;
    //        RaycastHit2D hit;
    //        hit = Physics2D.Raycast(startPos, deltaPos);

    //        if (hit.collider.CompareTag("Player"))
    //        {
    //            Debug.Log("Homing Triggered");
    //            Vector3 newDirection = collision.transform.position - transform.position;
    //            newDirection.z = 0;
    //            transform.up = Vector2.Lerp(previousDirection, newDirection, turnSpeed * Time.deltaTime);
    //            previousDirection = transform.up;
    //            rb2d.velocity = transform.up * rb2d.velocity.magnitude;
    //        }
    //    }
    //}

    private IEnumerator ToggleSeeking()
    {
        yield return new WaitForSeconds(1);
        transform.up = rb2d.velocity;
        previousDirection = transform.up;
        isHoming = true;
    }

    private void FixedUpdate()
    {
        //Runs if there is a player in range of the bullet
        if (targetsInRange.Count > 0)
        {
            FindClosest(); //Finds closest player
            float bulletBoundsY = GetComponent<CapsuleCollider2D>().bounds.extents.y + 0.1f;
            Vector2 startPos = new Vector2(transform.position.x, transform.position.y + bulletBoundsY);
            Vector2 deltaPos = closest - transform.position;
            
            RaycastHit2D hit;
            hit = Physics2D.Raycast(startPos, deltaPos); //Raycast to check if player is in line of sight of the bullet
            //Debug.Log(hit.collider.name);
            if (hit.collider != null && hit.collider.gameObject.CompareTag("Player"))
            {
                Debug.Log("Homing Triggered");
                Vector3 newDirection = hit.collider.transform.position - transform.position;
                newDirection.z = 0;
                transform.up = Vector2.Lerp(previousDirection, newDirection, turnSpeed * Time.deltaTime);
                previousDirection = transform.up;
                rb2d.velocity = transform.up * rb2d.velocity.magnitude;
            }
        }
    }


    //Finds the player that are closest to the bullet
    private void FindClosest()
    {
        foreach (Collider2D col in targetsInRange)
        {
            float objectRange = (col.gameObject.transform.position - transform.position).magnitude;
            //Debug.Log(col.name);

            if (range == 0)
            {
                range = objectRange;
                closest = col.gameObject.transform.position;
            }
            else if (objectRange < range)
            {
                range = objectRange;
                closest = col.gameObject.transform.position;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Add players that are in range of the bullet
        if (isHoming && collision.gameObject.CompareTag("Player"))
        {
            if (!targetsInRange.Contains(collision))
            {
                //Debug.Log(targetsInRange.Count);
                targetsInRange.Add(collision);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Remove players that are not in range of the bullet
        if (isHoming && collision.CompareTag("Player"))
        {
            if (targetsInRange.Contains(collision))
            {
                //Debug.Log("Removed");
                targetsInRange.Remove(collision);
            }
        }
    }

}
