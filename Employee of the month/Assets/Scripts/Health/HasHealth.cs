using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasHealth : MonoBehaviour
{
    private HudHealthBar hudHealthbar;
    public GameObject bloodPool;
    private Animator animator;

    public int playerIndex;
    public int maxHealth = 100;
    public float health;

    private bool isDead = false;

    void Start()
    {
        hudHealthbar = GetComponentInChildren<HudHealthBar>();
        health = maxHealth;
        animator = transform.GetComponentInChildren<Animator>();
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

            AudioSource.PlayClipAtPoint(AudioManager.instance.audioClips.damaged, transform.position);

            if (damage < 0)
            {
                Debug.LogWarning("Used LoseHealth to add Negative damage, use GainHealth instead");
            }
            ChangeHealth(-damage);
            Debug.Log(gameObject.name + " lost " + damage + "HP.");

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
    }

    private void OnDeath()
    {
        isDead = true;
        Debug.Log("Death Triggered");
        animator.SetTrigger("OnDeath");
        if (GetComponent<Spawner>() != null)
        {
            GetComponent<Spawner>().TriggerRespawn(5f);
        }
        else if (gameObject.CompareTag("Player"))
        {
            SpawnManager.instance.PlayerDied();
            AudioSource.PlayClipAtPoint(AudioManager.instance.audioClips.death, transform.position);

            Camera.main.GetComponent<CameraController>().RemoveCameraTracking(gameObject);


            GetComponentInChildren<WeaponController>().RemoveAllItems();
            GetComponentInChildren<CircleCollider2D>().enabled = false;
            GetComponent<Movement>().walksound.Stop();
            GetComponent<Movement>().enabled = false;
            GetComponent<Aim>().enabled = false;
            GetComponentInChildren<Fire>().enabled = false;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
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
