using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] Transform superMicroPos;
    [SerializeField] GameObject superMicro;
    [SerializeField] GameObject superMicroPull;

    private Aim aim;
    private Movement movement;
    private Fire fire;

    public LineRenderer lineRenderer;
    public Transform firePoint;

    public GameObject startVFX;
    public GameObject endVFX;
    private float damage = 100;
    public bool isCharged = false;
    public bool isCharging = false;
    public bool isShooting = false;

    private float defaultWalkSpeed;

    private Quaternion rotation;
    private List<ParticleSystem> particles = new List<ParticleSystem>();


    void Start()
    {
        aim = GetComponentInParent<Aim>();
        movement = GetComponentInParent<Movement>();
        fire = GetComponentInParent<Fire>();
        FillLists();
        DisableLaser();
    }

    void Update()
    {
        RotateToAim();
        UpdateLaser();
    }

    public void PowerUpLaser()
    {
        isCharging = true;
        if (superMicroPos.childCount == 0)
        {
            var superMicroSprite = Instantiate(superMicro, superMicroPos);
            var superMicroSpritePull = Instantiate(superMicroPull, superMicroPos);
        }
        superMicroPos.GetChild(1).GetComponent<Animator>().SetTrigger("Charging");
        superMicroPos.GetChild(0).GetComponent<Animator>().SetTrigger("Charging");

        Invoke(nameof(SetCharged), 1);
    }

    private void SetCharged()
    {
        isCharged = true;
        isCharging = false;
    }

    public void FireLaser()
    {
        isShooting = true;

        if (aim == null)
            aim = GetComponentInParent<Aim>();
        aim.rotationSpeed = 0.7f;

        EnableLaser();
        UpdateLaser();
        StartCoroutine(DisableLaser());
    }

    public void EnableLaser()
    {
        lineRenderer.enabled = true;


        if (movement == null)
            movement = GetComponentInParent<Movement>();

        //movement.enabled = false;
        defaultWalkSpeed = movement.walkSpeed;
        movement.walkSpeed *= 0.1f;

        for (int i = 0; i < particles.Count; i++)
            particles[i].Play();
    }

    public void UpdateLaser()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, firePoint.transform.up, 20, ~(1 << 8));//send raycast and ignore modifyers

        if (hit)
        {
            SendDamage(hit.collider, damage * Time.deltaTime);
            lineRenderer.SetPosition(1, hit.point);
        }

        lineRenderer.SetPosition(0, (Vector2)firePoint.position);
        startVFX.transform.position = (Vector2)firePoint.position;

        endVFX.transform.position = lineRenderer.GetPosition(1);
    }

    private void SendDamage(Collider2D collider, float damage)
    {
        if (collider.gameObject.transform.CompareTag("Player"))
        {
            if (collider.transform.parent.transform.TryGetComponent<HasHealth>(out HasHealth health))
            {
                health.LoseHealth(damage);
            }
        }

        if (collider.transform.GetComponent<HasHealth>() != null)
        {
            collider.transform.GetComponent<HasHealth>().LoseHealth(damage);
        }
        if (collider.gameObject.GetComponent<ItemBreak>() != null)
        {
            collider.gameObject.GetComponent<ItemBreak>().TakeDamage((int)damage);
        }
    }

    IEnumerator DisableLaser()
    {
        yield return new WaitForSeconds(3);

        superMicroPos.GetChild(0).GetComponent<Animator>().SetTrigger("Fired");

        isShooting = false;
        isCharged = false;
        lineRenderer.enabled = false;
        //movement.enabled = true;
        aim.rotationSpeed = 30;
        movement.walkSpeed = defaultWalkSpeed;


        for (int i = 0; i < particles.Count; i++)
            particles[i].Stop();

        fire.DeActivateLaser();
    }

    void RotateToAim()
    {
        Vector2 aimDirection;
        if (aim.hasGamePad)
        {
            aimDirection = aim.aimDirection - (Vector2)transform.position;
        }
        else
        {
            aimDirection = aim.mousePosition - transform.position;
        }

        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        rotation.eulerAngles = new Vector3(0, 0, angle);
        transform.rotation = rotation;
    }
    void FillLists()
    {
        for (int i = 0; i < startVFX.transform.childCount; i++)
        {
            var ps = startVFX.transform.GetChild(i).GetComponent<ParticleSystem>();
            if (ps != null)
                particles.Add(ps);
        }

        for (int i = 0; i < endVFX.transform.childCount; i++)
        {
            var ps = endVFX.transform.GetChild(i).GetComponent<ParticleSystem>();
            if (ps != null)
                particles.Add(ps);
        }
    }
}