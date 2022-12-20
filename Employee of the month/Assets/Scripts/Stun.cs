using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : MonoBehaviour
{
    public bool isStunnable;
    public bool isStunned;
    private float stunTime;
    public float stunTimer;

    private void Start()
    {
        isStunned = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.CompareTag("HardWall")))
        {
            Debug.Log("WallCollide");
           

            if (!isStunned && isStunnable)
            {
                isStunned = true;
                GetComponent<Movement>().enabled = false;
                GetComponent<Aim>().enabled = false;
                GetComponentInChildren<Fire>().enabled = false;
                Debug.Log("Runs Stun");
                StartCoroutine(Stunned());
            }
        }
    }

    public void WallStunChance(float stunTimer, float stunTime)
    {
        Debug.Log(stunTime);
        this.stunTime = stunTime;
        this.stunTimer = stunTimer;
        isStunnable = true;
        StartCoroutine(StunCountDown());
    }

    private IEnumerator Stunned()
    {
        yield return new WaitForSeconds(stunTime);
        GetComponent<Movement>().enabled = true;
        GetComponent<Aim>().enabled = true;
        GetComponentInChildren<Fire>().enabled = true;
        isStunned = false;
    }

    private IEnumerator StunCountDown()
    {
        yield return new WaitForSeconds(stunTimer);
        isStunnable = false;
    }
}
