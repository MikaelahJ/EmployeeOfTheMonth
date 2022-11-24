using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasHealth : MonoBehaviour
{
    public GameObject healthbar;
    

    public int maxHealth = 100;
    public float health;

    
    private bool isDead = false;

    void Start()
    {
        health = maxHealth;

        //HealthRegen(1, 0.2f, 50);
    }

    public void GainHealth(int heal)
    {
        if(heal < 0)
        {
            Debug.LogWarning("Used GainHealth to add Negative HP, use LoseHealth instead");
        }
        ChangeHealth(heal);
        Debug.Log(gameObject.name + " healed " + heal + "HP.");
    }

    public void LoseHealth(float damage)
    {
        if (damage < 0)
        {
            Debug.LogWarning("Used LoseHealth to add Negative damage, use GainHealth instead");
        }
        ChangeHealth(-damage);
        Debug.Log(gameObject.name + " lost " + damage + "HP.");
    }

    private void ChangeHealth(float healthChange)
    {
        health += healthChange;
        
        if (health > maxHealth)
        {
            health = maxHealth;
            Debug.Log(gameObject.name + " has full health!");
        }

        if (health <= 0)
        {
            OnDeath();
            Debug.Log(gameObject.name + " reached 0 health!");
        }

        UpdateHealthbar();
    }

    private void OnDeath()
    {
        isDead = true;
        Debug.Log("Triggered");
        if(GetComponent<Spawner>() != null)
        {
            GetComponent<Spawner>().TriggerRespawn();
        }
    }

    public void HealthRegen(int health, float timeBetweenRegen, float duration)
    {
        StartCoroutine(HealthRegenCouroutine(health, timeBetweenRegen, duration));
    }

    private IEnumerator HealthRegenCouroutine(int health, float timeBetweenRegen, float duration)
    {
        float timer = 0;
        while(timer < duration && !isDead)
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
        healthbar.GetComponent<UIHealthbar>().SetHealthBar(health);
    }
}
