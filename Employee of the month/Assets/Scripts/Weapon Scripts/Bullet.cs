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

    public bool isStapler;
    public bool isHoming = false;
    public float turnSpeed;
    public float percentageTurned;
    public Vector2 aimAssistRightBounds;
    public Vector2 aimAssistLeftBounds;
    private Vector2 previousDirection;
    public Vector3 closest; //The position of the closest Player
    public float range; //Current range to closest Player
    public List<Collider2D> targetsInRange;
    public LayerMask bulletMask;
    private Vector2[] aimAssistCollider;
    [SerializeField] private float scanBounds;

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
        targetsInRange = new List<Collider2D>();
        aimAssistCollider = GetComponentInChildren<EdgeCollider2D>().points;
        List<EdgeCollider2D> results = new List<EdgeCollider2D>();

        if (!isHoming)
        {
            //Debug.Log("Magnetic");
            aimAssistCollider[2] = new Vector2(aimAssistRightBounds.x, aimAssistRightBounds.y -10);
            aimAssistCollider[3] = aimAssistRightBounds;
            aimAssistCollider[4] = aimAssistLeftBounds;
            aimAssistCollider[5] = new Vector2(aimAssistLeftBounds.x, aimAssistLeftBounds.y - 10);
            GetComponentInChildren<EdgeCollider2D>().points = aimAssistCollider;


            //ContactFilter2D filter = new ContactFilter2D();//.NoFilter();
            //filter.SetLayerMask(bulletMask);
            //int hits;
            //hits = Physics2D.OverlapCollider(GetComponentInChildren<EdgeCollider2D>(), filter, targetsInRange);
            //if(results != null)
            //{
            //    foreach (Collider2D col in results)
            //    {
            //        if(col.CompareTag("Player"))
            //        {
            //            if (!targetsInRange.Contains(col))
            //            {
            //                //Debug.Log(targetsInRange.Count);
            //                targetsInRange.Add(col);
            //            }
            //        }
            //    }
            //}
        }
        else
        {
            //Debug.Log("Homing");
            aimAssistCollider[2] = new Vector2(aimAssistCollider[2].x, aimAssistCollider[2].y - scanBounds);
            aimAssistCollider[5] = new Vector2(aimAssistCollider[5].x, aimAssistCollider[5].y - scanBounds);
            GetComponentInChildren<EdgeCollider2D>().points = aimAssistCollider;

            //ContactFilter2D filter = new ContactFilter2D();
            //filter.SetLayerMask(bulletMask);
            //int hits;
            //hits = Physics2D.OverlapCollider(GetComponentInChildren<EdgeCollider2D>(), filter, targetsInRange);
            //if (results != null)
            //{
            //    foreach (Collider2D col in results)
            //    {
            //        if (col.CompareTag("Player"))
            //        {
            //            Debug.Log(col.name);
            //            if (!targetsInRange.Contains(col))
            //            {
            //                //Debug.Log(targetsInRange.Count);
            //                targetsInRange.Add(col);
            //            }
            //        }
            //    }
            //}
        }
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
        scanBounds = weapon.scanBounds;
        isStapler = weapon.isStapler;

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

        if(isStapler && collision.gameObject.CompareTag("Player"))
        {
            ApplyKnockBack(collision.collider);
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
            collider.gameObject.GetComponent<ItemBreak>().TakeDamage((int)damage);
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

        AudioSource.PlayClipAtPoint(AudioManager.instance.audioClips.bulletBounce, transform.position);

        //Turn off tracking to calculate new trajectory

        if (isHoming)
        {
            DisableTracking();
        }
    }


    private void FixedUpdate()
    {
        if (isHoming)
        {
            BulletPlayerTracking();
        }
    }


    #region Homing/Aimassist
    //turns off tracking and sets a new previousDirection
    public void DisableTracking()
    {
        if (isHoming)
        {
            transform.up = rb2d.velocity;
            previousDirection = transform.up;
            isHoming = false;
            StartCoroutine(ToggleSeeking()); //Turn off homing on player to get new bullet direction
        }
    }


    //To turn off seeking when bouncing etc...
    private IEnumerator ToggleSeeking()
    {
        yield return new WaitForSeconds(0.1f);
        transform.up = rb2d.velocity;
        previousDirection = transform.up;
        isHoming = true;
    }

    private void BulletPlayerTracking()
    {
        //only runs when there are players inside collider
        //if (targetsInRange.Count > 0)
        //{
            Debug.Log("InCollider");
            FindClosest(); //Finds closest player
            Vector2 startPos = new Vector2(transform.position.x, transform.position.y);
            Vector2 deltaPos = closest - transform.position;

            //raycast to see if any of the players are in line of sight of bullet
            RaycastHit2D hit;
            hit = Physics2D.Raycast(startPos, deltaPos, Mathf.Infinity, bulletMask);
            if (hit.collider != null && hit.collider.gameObject.CompareTag("Player"))
            {

                Debug.DrawRay(transform.position, hit.collider.transform.position - transform.position, Color.red, 5);;
                Vector3 newDirection = hit.collider.transform.position - transform.position;
                newDirection.z = 0;

                if (isHoming) //homing
                {
                    //Debug.Log("Homing");
                    
                    transform.up = Vector2.MoveTowards(previousDirection, newDirection, turnSpeed * Time.deltaTime);
                }
                else //Aim assist
                {
                    //Debug.Log("Magnetism");
                    transform.up = Vector2.Lerp(previousDirection, newDirection, (turnSpeed * Time.deltaTime));
                }

                previousDirection = transform.up;
                rb2d.velocity = transform.up * rb2d.velocity.magnitude;
            }
        //}
    }


    //Finds the player that are closest to the bullet
    private void FindClosest()
    {
        foreach (Collider2D col in targetsInRange)
        {
            float objectRange = (col.gameObject.transform.position - transform.position).magnitude;
            Debug.Log(col.name);

            if (range == 0)
            {
                range = objectRange;
                closest = col.gameObject.transform.position;
            }
            else if (objectRange < range)
            {
                Debug.Log("Updates");
                range = objectRange;
                closest = col.gameObject.transform.position;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //Add players that are in range of the bullet
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!targetsInRange.Contains(collision))
            {
                Debug.Log("Added");
                Debug.Log(targetsInRange.Count);
                targetsInRange.Add(collision);
                //Debug.Log(targetsInRange.Count);
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

    #endregion; 
}
