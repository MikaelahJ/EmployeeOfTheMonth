using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : MonoBehaviour
{
    public bool isStunnable;
    public bool isStunned;
    public float stunTimer;
    public float slowdownTimer = 2.0f;

    private float stunTime;
    private bool isSlowed;
    [SerializeField] private GameObject stunAnimation;

    private void Start()
    {
        isStunned = false;
        isSlowed = false;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("HardWall") || collision.gameObject.CompareTag("SoftWall"))
        {
            //Debug.Log("WallCollide");
            if (!isStunned && isStunnable)
            {
                isStunned = true;
                GameObject stun = Instantiate(stunAnimation, transform.position, Quaternion.identity, transform);
                stun.transform.localScale *= 2;
                GameObject stars = stun.transform.GetChild(0).gameObject;
                Material newMaterial = new Material(stars.GetComponent<MeshRenderer>().material);
                newMaterial.color = Color.yellow;
                stars.GetComponent<MeshRenderer>().material = newMaterial;

                Destroy(stun, stunTime);
                GetComponent<Movement>().enabled = false;
                GetComponent<Aim>().enabled = false;
                GetComponentInChildren<Fire>().enabled = false;
                StartCoroutine(Stunned());
            }
        }
    }

    public void WallStunChance(float stunTimer, float stunTime)
    {
        //Debug.Log(stunTime);
        this.stunTime = stunTime;
        this.stunTimer = stunTimer;
        if(!isStunned)
            StartCoroutine(StunCountDown());
    }

    public void OnSlowed(float speedSlowdown)
    {
        Debug.Log("hejhej");
        GameObject stun = Instantiate(stunAnimation, transform.position, Quaternion.identity, transform);
        Destroy(stun, slowdownTimer);

        StartCoroutine(Slowdown(speedSlowdown));
    }

    private IEnumerator Stunned()
    {
        yield return new WaitForSeconds(stunTime);

        if (!GetComponent<HasHealth>().isDead)
        {
            GetComponent<Movement>().enabled = true;
            GetComponent<Aim>().enabled = true;
            GetComponentInChildren<Fire>().enabled = true;
            StartCoroutine(ImmuneToStun());
            isStunned = false;
        }

    }

    private IEnumerator StunCountDown()
    {
        isStunnable = true;
        yield return new WaitForSeconds(stunTimer);
        if (!isStunned)
        {
            isStunnable = false;
        }
    }

    private IEnumerator Slowdown(float speedSlowdown)
    {
        if (!isSlowed)
        {
            isSlowed = true;
            GetComponent<Movement>().walkSpeed -= speedSlowdown;

            yield return new WaitForSeconds(slowdownTimer);

            isSlowed = false;
            GetComponent<Movement>().walkSpeed += speedSlowdown;
        }
    }

    private IEnumerator ImmuneToStun()
    {
        yield return new WaitForSeconds(0.5f);
        isStunnable = false;
    }
}
