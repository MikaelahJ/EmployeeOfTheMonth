using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudHealthBar : MonoBehaviour
{
    public SpriteRenderer healthSprite;

    public float offsetY = 0.5f;

    private void Start()
    {
        healthSprite = GetComponent<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        transform.position = transform.parent.position + Vector3.up * offsetY; //Make sure the bar is over player
        transform.LookAt(transform.position + Camera.main.transform.forward);
    }

    public void SetHealth(float health, float maxHealth)
    {
        float remaining = health/maxHealth;
        
        healthSprite.size = new Vector2(remaining, healthSprite.size.y);
    }
}
