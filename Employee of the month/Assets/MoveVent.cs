using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveVent : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
       animator = gameObject.GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetTrigger("Open");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Invoke(nameof(CloseVent), 1f);
        }
    }
    private void CloseVent()
    {
        animator.SetTrigger("Close");
    }
}
