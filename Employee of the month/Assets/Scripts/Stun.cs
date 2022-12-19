using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : MonoBehaviour
{
    public bool isStunnable;
    public bool isStunned;
    private float stunTime;
    private float stunTimer;

    public float StunTime { get { return stunTime; } set { stunTime = value; } }
    public float StunTimer { get { return stunTimer; } set { stunTimer = value; } }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.CompareTag("HardWall") || collision.gameObject.CompareTag("SoftWall")) && isStunnable)
        {
            Debug.Log("WallCollide");
            GetComponent<Movement>().enabled = false;
            GetComponent<Aim>().enabled = false;
            GetComponentInChildren<Fire>().enabled = false;

            if (!isStunned)
            {
                Debug.Log("Runs Stun");
                StartCoroutine(Stunned(collision));
            }
        }
    }

    public void WallStunChance(float stunTimer, float stunTime)
    {
        this.stunTime = stunTime;
        this.stunTimer = stunTimer;
        isStunnable = true;
        StartCoroutine(StunCountDown());
    }

    private IEnumerator Stunned(Collision2D collision)
    {
        isStunned = true;
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