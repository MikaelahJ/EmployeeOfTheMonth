using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBreak : MonoBehaviour
{
    [Header("Sprites in order first to last")]
    [SerializeField] private List<Sprite> spritesBeforeBreak = new List<Sprite>();
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    public int damageThreshold = 5;
    private int maxHealth;
    private int health;

    private float timer;
    private float breakRate = 0.5f;

    private void Start()
    {
        animator.enabled = false;
        spritesBeforeBreak.Reverse(); //känns enklast att kunna lägga in dom i rätt ordning i editorn så vi reversar den här ba
        maxHealth = spritesBeforeBreak.Count;
        health = maxHealth;
    }
    private void Update()
    {
        timer += Time.deltaTime;
    }
    public void TakeDamage(float damage)
    {
        Debug.Log("damage" + damage);
        if (!(damage >= damageThreshold))
        {
            Debug.Log("damage threshold not reached");
            return;
        }
        if (damage >= 25)
            health = 0;

        if (timer >= breakRate)
        {
            health--;
            timer = 0;
        }

        if (health <= 0)
        {
            animator.enabled = true;
            animator.SetTrigger("Break");
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            return;
        }

        spriteRenderer.sprite = spritesBeforeBreak[health];
        Debug.Log("sprite " + spriteRenderer.sprite);
    }
}

