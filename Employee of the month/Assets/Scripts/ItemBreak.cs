using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBreak : MonoBehaviour
{
    [Header("Sprites in order first to last")]
    [SerializeField] private List<Sprite> spritesBeforeBreak = new List<Sprite>();
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    //public int damageThreshold = 5;
    private int healthMultiplier;
    public int health;
    private int startHealth;

    private float timer;
    private float breakRate = 0.5f;
    private int damageDealt = 0;

    private void Start()
    {
        animator.enabled = false;
        spritesBeforeBreak.Reverse(); //känns enklast att kunna lägga in dom i rätt ordning i editorn så vi reversar den här ba
        healthMultiplier = spritesBeforeBreak.Count;
        startHealth = health;
        if (healthMultiplier != 0)
            health *= healthMultiplier + 1;
    }

    private void Update()
    {
        timer += Time.deltaTime;
    }
    public void TakeDamage(int damage)//damage ska skickas in som int
    {
        //if (timer >= breakRate)
        //{
        //    health -= damage;
        //    timer = 0;
        //}
        damageDealt += damage;

        health -= damage;
        Debug.Log("health " + health);

        if (health <= 0)
        {
            animator.enabled = true;
            animator.SetTrigger("Break");
            Invoke(nameof(DisableCollider), 0.001f);
            return;
        }

        //damagedealt kolla om över starthealth
        if (damageDealt >= startHealth)
        {
            healthMultiplier--;
            spriteRenderer.sprite = spritesBeforeBreak[healthMultiplier];

            damageDealt -= startHealth;
            Debug.Log("damageDealt " + damageDealt);
        }

    }

    //Added this because sometimes the collider disable before pencil stuck on wall script uses it
    private void DisableCollider()
    {
        gameObject.GetComponent<Collider2D>().enabled = false;
    }
}

