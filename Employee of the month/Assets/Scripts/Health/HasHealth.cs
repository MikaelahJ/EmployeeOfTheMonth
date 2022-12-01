using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasHealth : MonoBehaviour
{
    public UIHealthbar healthbar;
    private Animator animator;
    public GameObject bloodPool;

    public int playerIndex;
    public int maxHealth = 100;
    public float health;

    public bool isRadiation;
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
        if(heal < 0)
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

        AudioSource.PlayClipAtPoint(AudioManager.instance.audioClips.damaged, transform.position);

        if (damage < 0)
        {
            Debug.LogWarning("Used LoseHealth to add Negative damage, use GainHealth instead");
        }
        ChangeHealth(-damage);
        Debug.Log(gameObject.name + " lost " + damage + "HP.");
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
        }

        UpdateHealthbar();
    }

    public void AddBlood(GameObject bullet)
    {
        Instantiate(bloodPool, transform.position, bullet.transform.rotation );
    }

    private void OnDeath()
    {
        isDead = true;
        Debug.Log("Death Triggered");
        if(GetComponent<Spawner>() != null)
        {
            GetComponent<Spawner>().TriggerRespawn(5f);
        }
        else if(gameObject.CompareTag("Player"))
        {
            SpawnManager.instance.PlayerDied();
            AudioSource.PlayClipAtPoint(AudioManager.instance.audioClips.death, transform.position);

            GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
            GetComponentInChildren<CircleCollider2D>().enabled = false;
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

    public void HealthChange(int healthChange, float tickTime, float duration)
    {
        if (isRadiation == true)
            return;
        StartCoroutine(HealthChangeCouroutine(healthChange, tickTime, duration));
    }

    private IEnumerator HealthChangeCouroutine(int healthChange, float tickTime, float duration)
    {
        float timer = 0;
        while(timer < duration && !isDead)
        {
            timer += tickTime;
            switch (healthChange > 0)
            {
                case true://healthChange > 0 ger regen
                    GainHealth(healthChange);
                    break;
                case false://healthChange < 0 ger bleed/radiation damage
                    LoseHealth(-healthChange);
                    break;
            }
            yield return new WaitForSeconds(tickTime);
        }
        Debug.Log(gameObject.name + "'s " + healthChange + " hp per " + tickTime + " seconds wore off!");
        Debug.Log(health);
        isRadiation = false;
    }

    private void UpdateHealthbar()
    {
        if (healthbar == null) { return; }
        healthbar.SetHealthBar(health);
    }


}
