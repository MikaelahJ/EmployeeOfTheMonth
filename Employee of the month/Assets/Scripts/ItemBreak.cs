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
    [SerializeField] private GameObject spriteMask;

    //public int damageThreshold = 5;
    private int healthMultiplier;
    public float health;
    private float startHealth;

    private float timer;
    //private float breakRate = 0.5f;
    private float damageDealt = 0;

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
    public void TakeDamage(float damage)//damage ska skickas in som int
    {
        //if (timer >= breakRate)
        //{
        //    health -= damage;
        //    timer = 0;
        //}
        damageDealt += damage;

        health -= damage;
        //Debug.Log(gameObject.name + " has " + health + " health");

        if (health <= 0)
        {
            animator.enabled = true;
            animator.SetTrigger("Break");
            Invoke(nameof(DisableCollider), 0.001f);
            //Om en ny passage mellan rum öppnas så behövs det en transition mask
            if (spriteMask != null)
                spriteMask.SetActive(true);
            return;
        }

        //damagedealt kolla om över starthealth
        if (damageDealt >= startHealth)
        {
            healthMultiplier--;
            spriteRenderer.sprite = spritesBeforeBreak[healthMultiplier];

            damageDealt -= startHealth;
            //Debug.Log("damageDealt " + damageDealt);
        }
    }

    //Added this because sometimes the collider disable before pencil stuck on wall script uses it
    private void DisableCollider()
    {
        if (gameObject.GetComponent<Rigidbody2D>() != null)
            Destroy(gameObject.GetComponent<Rigidbody2D>());
        var colliders = gameObject.GetComponents<Collider2D>();

        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = false;
        }
    }
}

