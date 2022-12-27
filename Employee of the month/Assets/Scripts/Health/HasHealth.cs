using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class HasHealth : MonoBehaviour
{
    public GameObject playerSprite;
    private HudHealthBar hudHealthbar;
    public GameObject bloodPool;
    public GameObject BloodAnimation;

    //These 2 are set in ControllerInput
    public Animator animator;
    public Animator healthbarAnimator;

    [SerializeField] private Movement movement;
    [SerializeField] private Aim aim;

    public int playerIndex;
    public int maxHealth = 100;
    public float health;
    public float healthFlashThreshold = 0.25f;

    public bool isDead = false;

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

    public void LoseHealth(float damage)
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
            ChangeHealth(-damage);
            Debug.Log(gameObject.name + " lost " + damage + "HP.");

            CheckHealth();

        }
    }

    public void CheckHealth()
    {
        if(health / maxHealth <= 0.5)
        {
            healthbarAnimator.SetBool("HasLowHealth", true);
        }
        else
        {
            healthbarAnimator.SetBool("HasLowHealth", false); 
        }
    }

    private void ChangeHealth(float healthChange)
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
            OnDeath();
            health = 0;
            Debug.Log(gameObject.name + " reached 0 health!");
        }

        UpdateHealthbar();
    }

    public void AddBlood(GameObject bullet)
    {
        Instantiate(bloodPool, transform.position, bullet.transform.rotation);
        if (BloodAnimation != null)
        {
            GameObject bloodAnim = Instantiate(BloodAnimation, transform);
            bloodAnim.GetComponent<BloodAnimation>().setRotation = bullet.transform.rotation;
            bloodAnim.GetComponent<BloodAnimation>().isPenetrate = bullet.GetComponent<Bullet>().isPenetrate;
        }
    }

    private void OnDeath()
    {
        isDead = true;
        animator.SetTrigger("OnDeath");
        if (GetComponent<Spawner>() != null)
        {
            GetComponent<Spawner>().TriggerRespawn(5f);
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


            GetComponentInChildren<WeaponController>().RemoveAllItems();
            //To prevent the player from picking up new items while dead.
            GetComponentInChildren<WeaponController>().isDead = true;

            GetComponentInChildren<CircleCollider2D>().enabled = false;

            movement.walksound.Stop();
            movement.enabled = false;
            aim.enabled = false;

            playerSprite.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.None;
            playerSprite.GetComponent<SortingGroup>().sortingLayerID = 0;

            GetComponentInChildren<Fire>().enabled = false;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        }
    }

    public void OnRespawn()
    {
        health = maxHealth;
        isDead = false;
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
