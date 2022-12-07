using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBreak : MonoBehaviour
{
    [Header("Sprites in order first to last")]
    [SerializeField] private List<Sprite> spritesBeforeBreak = new List<Sprite>();
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private int maxHealth;
    private int health;

    private void Start()
    {
        animator.enabled = false;
        spritesBeforeBreak.Reverse(); //känns enklast att kunna lägga in dom i rätt ordning i editorn så vi reversar den här ba
        maxHealth = spritesBeforeBreak.Count;
        health = maxHealth;
    }
    public void TakeDamage()
    {
        health--;
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

//private int maxDamage;
//private int damageTaken = 0;

//private void Start()
//{
//    animator.enabled = false;
//    maxDamage = spritesBeforeBreak.Count + 1;
//}
//public void TakeDamage()
//{

//    damageTaken++;
//    if (damageTaken <= 0)
//    {
//        animator.enabled = true;
//        animator.SetTrigger("Break");
//        gameObject.GetComponent<BoxCollider2D>().enabled = false;
//        return;
//    }

//    spriteRenderer.sprite = spritesBeforeBreak[health];
//    Debug.Log("sprite " + spriteRenderer.sprite);
//}

