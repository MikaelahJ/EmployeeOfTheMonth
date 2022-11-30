using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasHealth : MonoBehaviour
{
    public UIHealthbar healthbar;
    private Animator animator;

    public int playerIndex;
    public int maxHealth = 100;
    public float health;
    public GameObject bloodPool;

    private bool isDead = false;

    void Start()
    {
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
        if(animator != null)
            animator.SetTrigger("TookDamage");

        if (damage < 0)
        {
            Debug.LogWarning("Used LoseHealth to add Negative damage, use GainHealth instead");
        }
        ChangeHealth(-damage);
        Debug.Log(gameObject.name + " lost " + damage + "HP.");

        Instantiate(bloodPool, transform.position, transform.rotation);
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
        Destroy(gameObject);
        isDead = true;

        Debug.Log("Death Triggered");
        if(GetComponent<Spawner>() != null)
        {
            GetComponent<Spawner>().TriggerRespawn(5f);
        }
        else if(gameObject.CompareTag("Player"))
        {
            SpawnManager.instance.PlayerDied();

            GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
            GetComponent<CircleCollider2D>().enabled = false;
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
        if (healthbar == null) { return; }
        healthbar.SetHealthBar(health);
    }


}
