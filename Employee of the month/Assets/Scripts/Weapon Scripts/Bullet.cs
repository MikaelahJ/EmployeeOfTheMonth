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

    public AnimationCurve animSpeed;

    public bool isPenetrate = false;
    //private int maxObjectPass = 1;
    //private int objectsPassed = 0;

    [SerializeField] private GameObject explosionPrefab;
    public bool isExplode = false;
    private float explodeRadius = 1;
    private float explosionDamage;

    public float knockBackModifier = 10;

    public bool isHoming = true;
    public float turnSpeed = 50;
    private Vector2 previousDirection;
    public Vector3 closest; //The position of the closest Player
    public float range; //Current range to closest Player
    public List<Collider2D> targetsInRange;
    public LayerMask bulletMask;

    private bool canTakeDamage = false;
    private bool hasExploded = false;

    public AudioClip bulletImpactSound;

    void Start()
    {
        direction = transform.up;
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.velocity = direction * bulletSpeed;
        Invoke(nameof(CanTakeDamage), 0.05f);
        previousDirection = transform.up;
        range = 0;
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
        isHoming = weapon.isHoming;
        turnSpeed = weapon.turnSpeed;

        if (isPenetrate)
        {
            pencil.GetComponent<CapsuleCollider2D>().enabled = true;
            Physics2D.IgnoreLayerCollision(3, 9); //Ignore Player + Bullet
            Physics2D.IgnoreLayerCollision(11, 9); //Ignore Soft Wall + Bullet
            Physics2D.IgnoreLayerCollision(0, 9); //Ignore Default + Bullet
        }
        else
        {
            pencil.GetComponent<CapsuleCollider2D>().enabled = false;
            Physics2D.IgnoreLayerCollision(3, 9, false);
            Physics2D.IgnoreLayerCollision(11, 9, false);
            Physics2D.IgnoreLayerCollision(0, 9, false);
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

        //if (isPenetrate && !collision.gameObject.CompareTag("HardWall"))
        //{
        //    Debug.Log("Penetrate through object: " + collision.gameObject.name);
        //    if (collision.gameObject.CompareTag("SoftWall"))
        //    {
        //        AudioSource.PlayClipAtPoint(AudioManager.instance.audioClips.impact_glass, transform.position);
        //    }
        //    else
        //    {
        //        AudioSource.PlayClipAtPoint(AudioManager.instance.audioClips.impact_wood, transform.position);
        //    }
        //    //return;
        //}

        //Play bullet hit sound
        AudioSource.PlayClipAtPoint(bulletImpactSound, transform.position, AudioManager.instance.audioClips.sfxVolume);

        if (isExplode)
        {
            Explode(transform.position, collision);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if (isHoming)
        {
            transform.up = rb2d.velocity;
            previousDirection = transform.up;
            isHoming = false;
            StartCoroutine(ToggleSeeking()); //Turn off homing on player to get new bullet direction
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
        if (collider.gameObject.GetComponent<ItemBreak>() != null)
        {
            collider.gameObject.GetComponent<ItemBreak>().TakeDamage(damage);
        }
    }

    private void Explode(Vector2 collisionPoint, Collision2D collision)
    {
        //Sometimes the player gets hit multiple times by same explosion
        if (hasExploded) { return; }
        hasExploded = true;
        //damage += explosionDamage;
        Collider2D[] targetsInRadius = Physics2D.OverlapCircleAll(collisionPoint, explodeRadius);
        var explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);
        explosion.transform.localScale = new Vector3(explodeRadius, explodeRadius, explodeRadius);
        Destroy(explosion, 1f);
        //Debug.Log("Explosion Happened!");

        foreach (var target in targetsInRadius)
        {
            SendDamage(explosionDamage, target, collision);
            //Debug.Log("Tried to deal explosiondamage to: " + target.name);
        }
    }

    private void ApplyKnockBack(Collider2D playerCollider)
    {
        Rigidbody2D playerRb = playerCollider.gameObject.GetComponentInParent<Rigidbody2D>();
        if (playerRb == null) { return; }
        playerRb.AddForce(transform.up.normalized * knockBackModifier, ForceMode2D.Impulse);
        Debug.Log("Applied " + rb2d.velocity.normalized * knockBackModifier + " Knockback to " + playerCollider.name);
    }

    private void Bounce()
    {
        bounces++;
        rb2d.sharedMaterial.bounciness = 1;

        AudioSource.PlayClipAtPoint(AudioManager.instance.audioClips.bulletBounce, transform.position, AudioManager.instance.audioClips.sfxVolume);
    }

    //private void OnTriggerStay2D(Collider2D collider)
    //{
    //    if (isHoming && collider.gameObject.CompareTag("Player"))
    //    {
    //        Debug.Log("Homing Triggered");
    //        Vector3 newDirection = collider.transform.position - transform.position;
    //        newDirection.Normalize();
    //        newDirection.z = 0;
    //        transform.up = Vector2.Lerp(direction, newDirection, turnSpeed * Time.deltaTime);
    //        direction = transform.up;
    //        rb2d.velocity = transform.up * bulletSpeed;
    //    }
    //}

    private IEnumerator ToggleSeeking()
    {
        yield return new WaitForSeconds(0.1f);
        transform.up = rb2d.velocity;
        previousDirection = transform.up;
        isHoming = true;
    }

    private void FixedUpdate()
    {
        BulletPlayerTracking();

        //Runs if there is a player in range of the bullet
        //if (targetsInRange.Count > 0 && isHoming)
        //{
        //    Debug.Log("Player in Circle");
        //    FindClosest(); //Finds closest player
        //    float bulletBoundsY = GetComponent<CapsuleCollider2D>().bounds.extents.y + 0.1f;
        //    Vector2 startPos = new Vector2(transform.position.x, transform.position.y + bulletBoundsY);
        //    Vector2 deltaPos = closest - transform.position;

        //    RaycastHit2D hit;
        //    hit = Physics2D.Raycast(startPos, deltaPos, Mathf.Infinity, bulletMask); //Raycast to check if player is in line of sight of the bullet
        //    //Debug.Log(hit.collider.name);
        //    if (hit.collider != null && hit.collider.gameObject.CompareTag("Player"))
        //    {
        //        Debug.Log("Homing Triggered");
        //        Vector3 newDirection = hit.collider.transform.position - transform.position;
        //        newDirection.z = 0;
        //        transform.up = Vector2.Lerp(previousDirection, newDirection, (turnSpeed * Time.deltaTime) / newDirection.magnitude);
        //        previousDirection = transform.up;
        //        rb2d.velocity = transform.up * rb2d.velocity.magnitude;
        //    }
        //}
    }

    private void BulletPlayerTracking()
    {
        if (targetsInRange.Count > 0)
        {
            Debug.Log("Player in Circle");
            FindClosest(); //Finds closest player
            //float bulletBoundsY = GetComponent<CapsuleCollider2D>().bounds.extents.y + 0.2f;
            Vector2 startPos = new Vector2(transform.position.x, transform.position.y);
            Vector2 deltaPos = closest - transform.position;

            RaycastHit2D hit;
            hit = Physics2D.Raycast(startPos, deltaPos, Mathf.Infinity, bulletMask); //Raycast to check if player is in line of sight of the bullet
            //Debug.Log(hit.collider.name);
            if (hit.collider != null && hit.collider.gameObject.CompareTag("Player"))
            {
                Debug.Log("Homing Triggered");
                Vector3 newDirection = hit.collider.transform.position - transform.position;
                newDirection.z = 0;
                if (isHoming)
                {
                    Debug.Log("Homing");
                    transform.up = Vector2.Lerp(previousDirection, newDirection, (turnSpeed * Time.deltaTime) / newDirection.magnitude);
                }
                else
                {
                    Debug.Log("No Homing");
                    transform.up = Vector2.Lerp(previousDirection, newDirection, (turnSpeed * Time.deltaTime));
                }
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
        if (collision.gameObject.CompareTag("Player"))
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
        if (collision.CompareTag("Player"))
        {
            if (targetsInRange.Contains(collision))
            {
                //Debug.Log("Removed");
                targetsInRange.Remove(collision);
            }
        }
    }
}
