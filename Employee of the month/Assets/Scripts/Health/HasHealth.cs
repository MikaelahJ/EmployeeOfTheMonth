using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int healthbar;

    private bool isDead = false;

    void Start()
    {
        healthbar = maxHealth;

        HealthRegen(-10, 1, 7);
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

    public void LoseHealth(int damage)
    {
        if (damage < 0)
        {
            Debug.LogWarning("Used LoseHealth to add Negative damage, use GainHealth instead");
        }
        ChangeHealth(-damage);
        Debug.Log(gameObject.name + " lost " + damage + "HP.");
    }

    private void ChangeHealth(int healthChange)
    {
        healthbar += healthChange;
        
        if (healthbar > maxHealth)
        {
            healthbar = maxHealth;
            Debug.Log(gameObject.name + " has full health!");
        }

        if (healthbar <= 0)
        {
            OnDeath();
            Debug.Log(gameObject.name + " reached 0 health!");
        }
    }

    private void OnDeath()
    {
        isDead = true;
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
}
