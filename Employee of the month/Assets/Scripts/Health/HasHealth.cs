using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class HasHealth : MonoBehaviour
{
    public GameObject playerSprite;
    private HudHealthBar hudHealthbar;
    public GameObject bloodPool;
    public List<Sprite> bloodPoolSprites = new List<Sprite>();
    public GameObject BloodAnimation;

    //These 2 are set in ControllerInput
    public Animator animator;
    public Animator healthbarAnimator;

    [SerializeField] private Movement movement;
    [SerializeField] private Aim aim;

    public Team team;

    public int playerIndex;
    public int maxHealth = 100;
    public float health;
    public float healthFlashThreshold = 0.25f;

    public float respawnTime = 3f;
    public bool isDead = false;

    private int sortingLayerID;
    [SerializeField] private LayerMask ignore;

    void Start()
    {
        hudHealthbar = GetComponentInChildren<HudHealthBar>();
        health = maxHealth;

        if (gameObject.CompareTag("Player"))
        {
            SpawnManager.instance.alivePlayers += 1;
        }
    }

    public void GainHealth(int heal)
    {
        if (heal < 0)
        {
            Debug.LogWarning("Used GainHealth to add Negative HP, use LoseHealth instead");
        }
        ChangeHealth(heal);
        Debug.Log(gameObject.name + " healed " + heal + "HP.");
    }

    public void LoseHealth(float damage, GameObject bullet = null)
    {
        if (gameObject.CompareTag("Player"))
        {
            if (animator != null)
                animator.SetTrigger("TookDamage");

            AudioSource.PlayClipAtPoint(AudioManager.instance.audioClips.damaged, transform.position, AudioManager.instance.audioClips.sfxVolume);

            if (damage < 0)
            {
                Debug.LogWarning("Used LoseHealth to add Negative damage, use GainHealth instead");
            }
            ChangeHealth(-damage, bullet);
            Debug.Log(gameObject.name + " lost " + damage + "HP.");

            CheckHealth();

        }
    }

    public void CheckHealth()
    {
        if (health / maxHealth <= 0.5)
        {
            healthbarAnimator.SetBool("HasLowHealth", true);
        }
        else
        {
            healthbarAnimator.SetBool("HasLowHealth", false);
        }
    }

    private void ChangeHealth(float healthChange, GameObject bullet = null)
    {
        if (isDead)
        {
            Debug.Log("Target is already dead!");
            return;
        }
        health += healthChange;

        if (health > maxHealth)
        {
            health = maxHealth;
            Debug.Log(gameObject.name + " has full health!");
        }

        if (health <= 0)
        {
            OnDeath(bullet);
            health = 0;
            Debug.Log(gameObject.name + " reached 0 health!");
        }

        UpdateHealthbar();
    }

    public void AddBlood(GameObject bullet)
    {
        var blood = Instantiate(bloodPool, transform.position, bullet.transform.rotation);
        blood.GetComponent<SpriteRenderer>().sprite = bloodPoolSprites[Random.Range(0, bloodPoolSprites.Count)];

        if (BloodAnimation != null)
        {
            GameObject bloodAnim = Instantiate(BloodAnimation, transform);
            bloodAnim.GetComponent<BloodAnimation>().setRotation = bullet.transform.rotation;
            bloodAnim.GetComponent<BloodAnimation>().isPenetrate = bullet.GetComponent<Bullet>().isPenetrate;
        }
    }

    private void OnDeath(GameObject bullet)
    {
        isDead = true;

        //send raycast to check for wall to play wall death animation
        RaycastHit2D hit = Physics2D.Raycast(transform.position, bullet.transform.up, 1, ~ignore);
        Debug.DrawRay(transform.position, -transform.up, Color.green, 10f);

        if (hit)
        {
            gameObject.transform.position = hit.point;

            transform.up = -hit.normal;

            animator.SetTrigger("WallDeath");
        }
        else
        {
            animator.SetTrigger("OnDeath");
        }

        if (GameModeManager.Instance.currentMode == Gamemodes.DeathMatch)
        {
            Debug.Log("is Deathmatch");
            Debug.Log(bullet.GetComponent<Bullet>().bulletOwner.gameObject.transform.parent);
            Team team = bullet.GetComponent<Bullet>().bulletOwner.gameObject.GetComponentInParent<HasHealth>().team;
            if (team != null)
                team.AddPoints(1);
        }

        if (GetComponent<Spawner>() != null)
        {
            DisablePlayer();
            GetComponent<Spawner>().TriggerRespawn(respawnTime);
        }
        else if (gameObject.CompareTag("Player"))
        {
            SpawnManager.instance.PlayerDied();

            AudioClip death = null;
            int randomSound;
            switch (playerIndex)
            {
                case 0:
                    randomSound = Random.Range(0, AudioManager.instance.audioClips.player1Deaths.Count);
                    death = AudioManager.instance.audioClips.player1Deaths[randomSound];
                    break;
                case 1:
                    randomSound = Random.Range(0, AudioManager.instance.audioClips.player2Deaths.Count);
                    death = AudioManager.instance.audioClips.player2Deaths[randomSound];
                    break;
                case 2:
                    randomSound = Random.Range(0, AudioManager.instance.audioClips.player3Deaths.Count);
                    death = AudioManager.instance.audioClips.player3Deaths[randomSound];
                    break;
                case 3:
                    randomSound = Random.Range(0, AudioManager.instance.audioClips.player4Deaths.Count);
                    death = AudioManager.instance.audioClips.player4Deaths[randomSound];
                    break;
                default:
                    Debug.Log("No Death sound could be found");
                    break;

            }

            AudioSource.PlayClipAtPoint(death, transform.position, AudioManager.instance.audioClips.characterVolume);

            Camera.main.GetComponent<CameraController>().RemoveCameraTracking(gameObject);

            DisablePlayer();
        }
    }

    void DisablePlayer()
    {
        GetComponentInChildren<WeaponController>().RemoveAllItems();
        //To prevent the player from picking up new items while dead.
        GetComponentInChildren<WeaponController>().isDead = true;

        GetComponentInChildren<CircleCollider2D>().enabled = false;

        movement.walksound.Stop();
        movement.enabled = false;
        aim.enabled = false;

        playerSprite.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.None;
        sortingLayerID = playerSprite.GetComponent<SortingGroup>().sortingLayerID;
        playerSprite.GetComponent<SortingGroup>().sortingLayerID = 0;

        GetComponentInChildren<Fire>().enabled = false;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        //GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
    }

    void EnablePlayer()
    {
        Debug.Log("HasHealth: EnablePlayer");
        GetComponentInChildren<WeaponController>().isDead = false;
        GetComponentInChildren<CircleCollider2D>().enabled = true;

        animator.Play("Idle");

        movement.walksound.Play();
        movement.enabled = true;
        aim.enabled = true;

        playerSprite.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        playerSprite.GetComponent<SortingGroup>().sortingLayerID = sortingLayerID;

        GetComponentInChildren<Fire>().enabled = true;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        //GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }

    public void OnRespawn()
    {
        Debug.Log("HasHealth: OnRespawn");
        health = maxHealth;
        isDead = false;
        EnablePlayer();
    }

    public void HealthRegen(int health, float timeBetweenRegen, float duration)
    {
        StartCoroutine(HealthRegenCouroutine(health, timeBetweenRegen, duration));
    }

    private IEnumerator HealthRegenCouroutine(int health, float timeBetweenRegen, float duration)
    {
        float timer = 0;
        while (timer < duration && !isDead)
        {
            timer += timeBetweenRegen;
            switch (health > 0)
            {
                case true:
                    GainHealth(health);
                    break;
                case false:
                    LoseHealth(-health);
                    break;
            }
            yield return new WaitForSeconds(timeBetweenRegen);
        }
        Debug.Log(gameObject.name + "'s " + health + " hp per " + timeBetweenRegen + " seconds wore off!");
    }

    private void UpdateHealthbar()
    {
        if (hudHealthbar == null) { return; }
        hudHealthbar.SetHealth(health, maxHealth);

        //if (healthbar == null) { return; }
        //healthbar.SetHealthBar(health);
    }


}
