using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class AimLine : MonoBehaviour
{
    private LineRenderer aimLine;
    public float laserWidth = 0.1f;
    public float laserMaxLength = 5f;
    //BoxCollider2D collider;
    public bool isBlocked = false;

    void Start()
    {
        aimLine = GetComponent<LineRenderer>();
        Vector3[] initLaserPositions = new Vector3[2] { Vector3.zero, Vector3.zero };
        aimLine.SetPositions(initLaserPositions);
        //aimLine.SetWidth(laserWidth, laserWidth);
    }

    void Update()
    {
        if (!isBlocked)
        {
            aimLine.enabled = true;
            ShootLaserFromTargetPosition(transform.position, transform.up, laserMaxLength);
        }
        else
        {
            aimLine.enabled = false;
        }
    }

    void ShootLaserFromTargetPosition(Vector3 targetPosition, Vector3 direction, float length)
    {
        Vector3 endPosition = targetPosition + (length * direction);

        RaycastHit2D raycastHit = Physics2D.Raycast(targetPosition, direction, length, LayerMask.GetMask("HardWall", "SoftWall"));
        
        if(raycastHit)
            endPosition = raycastHit.point;

        aimLine.SetPosition(0, targetPosition);
        aimLine.SetPosition(1, endPosition);
    }
}