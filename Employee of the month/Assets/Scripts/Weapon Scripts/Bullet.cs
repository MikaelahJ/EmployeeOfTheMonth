using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject pencil;
    [SerializeField] private GameObject PencilStuckInWall;
    [SerializeField] private GameObject particles;
    [SerializeField] private GameObject trail;
    [SerializeField] private GameObject impactParticles;
    [SerializeField] private Sprite burningPaper;
    private bool haveSpawnedPencil = false;

    public float selfDamageModifier = 0.5f;
    public float teamDamageModifier = 0.25f;

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
    public Collider2D bulletOwner;

    public bool isStapler;
    public bool isSuperStapler;
    public float stunTime;
    public float stunTimer;
    public float speedSlowdown;

    public bool isShotgun;
    public bool isSuperShredder;


    [Header("Homing parameters")]
    public bool isHoming = false;
    public float turnSpeed;
    public Vector2 aimAssistRightBounds;
    public Vector2 aimAssistLeftBounds;
    public Vector2 homingShotgunRightBoundsFar;
    public Vector2 homingShotgunLeftBoundsFar;
    public Vector2 homingShotgunRightBoundsClose;
    public Vector2 homingShotgunLeftBoundsClose;
    public Vector2 homingTrackingRightBounds;
    public Vector2 homingTrackingLeftBounds;
    private Vector2 previousDirection;
    public GameObject closest; //The position of the closest Player
    public float range; //Current range to closest Player
    public List<GameObject> targetsInRange;
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
        targetsInRange = new List<GameObject>();
        aimAssistCollider = GetComponentInChildren<PolygonCollider2D>().points;

        rb2d = GetComponent<Rigidbody2D>();

        var physicsMat = new PhysicsMaterial2D();

        // ? operator is a fancy way to do if/else
        physicsMat.bounciness = isBouncy ? 1 : 0;
        physicsMat.friction = isBouncy ? 0 : 1000;

        rb2d.sharedMaterial = physicsMat;

        if (!isHoming)
        {
            //Debug.Log("Magnetic");
            aimAssistCollider[1] = new Vector2(aimAssistRightBounds.x, aimAssistRightBounds.y - 10);
            aimAssistCollider[2] = aimAssistRightBounds;
            aimAssistCollider[3] = aimAssistLeftBounds;
            aimAssistCollider[4] = new Vector2(aimAssistLeftBounds.x, aimAssistLeftBounds.y - 10);
            GetComponentInChildren<PolygonCollider2D>().points = aimAssistCollider;
        }
        else
        {
            if (isShotgun)
            {
                Debug.Log("HomingShotgunTriggered");
                aimAssistCollider[1] = new Vector2(homingShotgunRightBoundsClose.x, homingShotgunRightBoundsClose.y - scanBounds);
                aimAssistCollider[2] = homingShotgunRightBoundsFar;
                aimAssistCollider[3] = homingShotgunLeftBoundsFar;
                aimAssistCollider[4] = new Vector2(homingShotgunLeftBoundsClose.x, homingShotgunLeftBoundsClose.y - scanBounds);
                GetComponentInChildren<PolygonCollider2D>().points = aimAssistCollider;
            }
            else
            {
                //Debug.Log("Homing");
                aimAssistCollider[1] = new Vector2(aimAssistCollider[1].x, aimAssistCollider[1].y - scanBounds);
                aimAssistCollider[4] = new Vector2(aimAssistCollider[4].x, aimAssistCollider[4].y - scanBounds);
                GetComponentInChildren<PolygonCollider2D>().points = aimAssistCollider;
            }
        }
    }

    private void CanTakeDamage()
    {
        canTakeDamage = true;
    }

    public void UpdateBulletModifyers(NewItemScriptableObject weapon)
    {
        pencil.GetComponent<SpriteRenderer>().sprite = weapon.bulletSprite;
        if(weapon.bulletSprite.name == "Staple")
        {
            GetComponent<Animator>().SetTrigger("StapleShot");
        }
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
        isShotgun = weapon.isShotgun;
        isSuperShredder = weapon.isSuperShredder;
        isHoming = weapon.isHoming;
        turnSpeed = weapon.turnSpeed;
        scanBounds = weapon.scanBounds;
        isStapler = weapon.isStapler;
        isSuperStapler = weapon.isSuperStapler;

        stunTime = weapon.stunTime;
        speedSlowdown = weapon.speedSlowdown;

        if (isSuperShredder)
        {
            pencil.GetComponent<SpriteRenderer>().sprite = burningPaper;
            return;
        }

        if (isPenetrate)
        {
            pencil.GetComponent<CapsuleCollider2D>().enabled = true;
            Physics2D.IgnoreLayerCollision(3, 9); //Ignore Player + Bullet
            Physics2D.IgnoreLayerCollision(11, 9); //Ignore Soft Wall + Bullet
            Physics2D.IgnoreLayerCollision(0, 9); //Ignore Default + Bullet
            Physics2D.IgnoreLayerCollision(15, 9); //Ignore MapEffects + Bullet
        }
        else
        {
            pencil.GetComponent<CapsuleCollider2D>().enabled = false;
            Physics2D.IgnoreLayerCollision(3, 9, false);
            Physics2D.IgnoreLayerCollision(11, 9, false);
            Physics2D.IgnoreLayerCollision(0, 9, false);
            Physics2D.IgnoreLayerCollision(15, 9, false);
        }

        if (isExplode)
        {
            //Activate laser bullets if its exploding
            pencil.transform.localScale += Vector3.one * (explodeRadius - 1f);
            GetComponent<Animator>().enabled = true;
            GetComponent<Animator>().SetTrigger("isLaser");
        }

        if (!isExplode && isPenetrate)
        {
            particles.SetActive(true);
            var emission = particles.GetComponent<ParticleSystem>().emission;
            float startVal = emission.rateOverTime.constant;
            emission.rateOverTime = startVal * bulletSpeed;

            trail.SetActive(true);
            trail.GetComponent<TrailRenderer>().time = bulletSpeed / 30f;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("HardWall") || collision.gameObject.CompareTag("Untagged") && collision.gameObject.name != "Map")
        {
            var impact = Instantiate(impactParticles, transform.position, transform.rotation);
            Destroy(impact, 2);
        }
        SendDamage(collision.collider, collision);
        //Debug.Log("Collided with " + collision.gameObject.name);
        if (isStapler && collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log("isStapler triggered");
            //ApplyKnockBack(collision.collider); //Knockback is added in SendDamage -Thomas
            bool selfDamage = collision.collider.name == bulletOwner.name;

            if (!selfDamage || canTakeDamage)
            {
                collision.gameObject.GetComponent<Stun>().OnSlowed(speedSlowdown);
                collision.gameObject.GetComponent<Stun>().WallStunChance(0.5f, stunTime);
                //Debug.Log(collision.gameObject.name);
            }
        }

        //Bounce
        if (isBouncy && bounces < maxBounce)
        {
            Bounce();
            if (collision.gameObject.CompareTag("Player"))
            {
                //Debug.Log("AppliedForce");
                //Knockback l???ggs p??? i SendDamage
                //ApplyKnockBack(collision.collider);
            }
            return;
        }


        //Play bullet hit sound
        AudioSource.PlayClipAtPoint(bulletImpactSound, transform.position, AudioManager.instance.audioClips.sfxVolume);

        if (isExplode)
        {
            Explode(transform.position, collision);
            Destroy(gameObject);

            Camera.main.GetComponent<ScreenShakeBehavior>().TriggerShake(0.1f, 0.1f);
        }
        else
        {
            if (isPenetrate)
            {
                SpawnPencilStuckInWall(collision);
            }
            Destroy(gameObject);
        }
    }

    private void SpawnPencilStuckInWall(Collision2D collision)
    {
        if (haveSpawnedPencil) { return; }
        haveSpawnedPencil = true;
        GameObject pencilStuck = Instantiate(PencilStuckInWall, transform.position, Quaternion.identity);
        pencilStuck.transform.localScale = transform.lossyScale;
        pencilStuck.GetComponent<PencilStuckInWall>().SetPencilRotation(transform.rotation);
        pencilStuck.GetComponent<PencilStuckInWall>().SetCrackTransform(collision);
        pencilStuck.GetComponent<PencilStuckInWall>().SetPencilPosition(collision);

        Destroy(particles, 5);
        Destroy(trail, 5);
        particles.GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
        particles.transform.SetParent(null);
        particles.transform.localScale = Vector3.one;
        trail.transform.SetParent(null);

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        transform.up = rb2d.velocity;
    }

    public void SendDamage(Collider2D collider, Collision2D collision = null)
    {
        //Debug.Log(collider.name + " collided with " + bulletOwner.name);
        if (!canTakeDamage && collider.name == bulletOwner.name)
        {
            return;
        }
        SendDamage(damage, collider, collision);
    }

    public void SendDamage(float damage, Collider2D collider, Collision2D collision = null)
    {
        bool selfDamage = collider.name == bulletOwner.name;
        if (selfDamage)
        {
            damage *= selfDamageModifier;
        }
        else
        {
            Team self = bulletOwner.gameObject.GetComponentInParent<HasHealth>().team;
            Team otherTeam = null;
            var other = collider.gameObject.GetComponentInParent<HasHealth>();
            if (other != null)
            {
                otherTeam = other.team;
                if (otherTeam != null && otherTeam.GetTeamName() != Teams.NoTeam)
                {
                    if (self.GetTeamName() == otherTeam.GetTeamName())
                    {
                        damage *= teamDamageModifier;
                    }
                }
            }
        }

        if (collider.gameObject.transform.CompareTag("Player"))
        {
            ApplyKnockBack(collider);

            if (damage == 0) { return; }
            if (collider.transform.parent.transform.TryGetComponent<HasHealth>(out HasHealth health))
            {
                health.LoseHealth(damage, gameObject);
                health.AddBlood(gameObject);
            }

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
        Destroy(explosion, 1.5f);
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
        //Debug.Log("Applied " + rb2d.velocity.normalized * knockBackModifier + " Knockback to " + playerCollider.name);
        float extraKnockback = 0;
        Vector3 forceDirection = transform.up.normalized;
        if (hasExploded)
        {
            forceDirection *= -1;
            extraKnockback = explosionDamage;
        }
        playerRb.AddForce(forceDirection * (knockBackModifier + extraKnockback), ForceMode2D.Impulse);
        Debug.Log("Applied " + rb2d.velocity.normalized * (knockBackModifier + extraKnockback) + " Knockback to " + playerCollider.name);
    }

    private void Bounce()
    {
        bounces++;
        rb2d.velocity *= 1.02f;
        AudioSource.PlayClipAtPoint(AudioManager.instance.audioClips.bulletBounce, transform.position, AudioManager.instance.audioClips.sfxVolume);

        //Turn off tracking to calculate new trajectory

        if (isHoming)
        {
            DisableTracking();
        }
    }


    private void FixedUpdate()
    {
        if (isHoming)
            BulletPlayerTracking();
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
        if (targetsInRange.Count > 0)
        {
            //Debug.Log("InCollider");
            FindClosest(); //Finds closest player
            Vector2 startPos = transform.position;// new Vector2(transform.position.x, transform.position.y);
            Vector2 deltaPos = closest.transform.position - transform.position;

            //raycast to see if any of the players are in line of sight of bullet
            RaycastHit2D hit;
            hit = Physics2D.Raycast(startPos, deltaPos, Mathf.Infinity, bulletMask);
            if (hit.collider != null && hit.collider.gameObject.CompareTag("Player"))
            {

                Debug.DrawRay(transform.position, hit.collider.transform.position - transform.position, Color.red, 5);
                Vector3 newDirection = hit.collider.transform.position - transform.position;
                newDirection.z = 0;

                if (isHoming) //homing
                {
                    transform.up = Vector2.MoveTowards(previousDirection, newDirection, turnSpeed * Time.deltaTime);
                }
                else //Aim assist
                {
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
        foreach (GameObject enemy in targetsInRange)
        {
            float objectRange = (enemy.transform.position - transform.position).magnitude;
            //Debug.Log(col.name);

            if (range == 0)
            {
                range = objectRange;
                closest = enemy.gameObject;
            }
            else if (objectRange < range)
            {
                //Debug.Log("Updates");
                range = objectRange;
                closest = enemy.gameObject;
            }
            else
            {
                range = (closest.transform.position - transform.position).magnitude;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Add players that are in range of the bullet
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!targetsInRange.Contains(collision.gameObject))
            {
                targetsInRange.Add(collision.gameObject);
            }
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        //Remove players that are not in range of the bullet
        if (collision.CompareTag("Player"))
        {
            if (targetsInRange.Contains(collision.gameObject))
            {
                targetsInRange.Remove(collision.gameObject);
            }
        }
    }

    #endregion; 
}
