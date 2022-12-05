using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIfPenetrate : MonoBehaviour
{
    [SerializeField] private Bullet bulletScript;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        //if collision is not Player or SoftWall and is not bouncy
        if (!collider.gameObject.CompareTag("Player") && !collider.gameObject.CompareTag("SoftWall") && !bulletScript.isBouncy)
        {
            Destroy(bulletScript.gameObject);
        }
        else //go through and send damage to Player or SoftWall
        {
            bulletScript.SendDamage(collider);
        }
    }
}
