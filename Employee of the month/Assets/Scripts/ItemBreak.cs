using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBreak : MonoBehaviour
{
    [Header("Sprites in order first to last")]
    [SerializeField] private List<Sprite> spritesBeforeBreak = new List<Sprite>();
    [SerializeField] private Animator animator;
    private SpriteRenderer spriteRenderer;

    private int maxHealth;
    private int health;

    private void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spritesBeforeBreak.Reverse(); //k�nns enklast att kunna l�gga in dom i r�tt ordning i editorn s� vi reversar den h�r ba
        maxHealth = spritesBeforeBreak.Count;
        health = maxHealth;

        Debug.Log("maxhealth" + maxHealth);
    }
    public void TakeDamage()
    {

        health--;
        Debug.Log("itemhealth" + health);
        if (health <= 0)
        {
            animator.SetTrigger("Break");
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            return;
        }

        spriteRenderer.sprite = spritesBeforeBreak[health];
        Debug.Log("sprite " + spriteRenderer.sprite);
    }
}
