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

    private void Start()
    {
        animator.enabled = false;
        spritesBeforeBreak.Reverse(); //känns enklast att kunna lägga in dom i rätt ordning i editorn så vi reversar den här ba
        healthMultiplier = spritesBeforeBreak.Count + 1;
        startHealth = health;
        if (healthMultiplier != 0)
            health *= healthMultiplier;
    }

    private void Update()
    {
        timer += Time.deltaTime;
    }
    public void TakeDamage(int damage)//damage ska skickas som int
    {
        //if (timer >= breakRate)
        //{
        //    health -= damage;
        //    timer = 0;
        //}

        health -= damage;
        Debug.Log("health " + health);

        if (health <= 0)
        {
            animator.enabled = true;
            animator.SetTrigger("Break");
            gameObject.GetComponent<Collider2D>().enabled = false;
            return;
        }

        Debug.Log("health Modulus " + health % healthMultiplier);
        //health = 150
        if (health % healthMultiplier == startHealth)
        {
            spriteRenderer.sprite = spritesBeforeBreak[health];

            healthMultiplier--;
            Debug.Log("healthMultiplier " + healthMultiplier);
        }

    }
}

