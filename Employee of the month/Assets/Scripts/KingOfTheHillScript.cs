using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class KingOfTheHillScript : MonoBehaviour
{
    BoxCollider2D myCollider;
    LineRenderer myLine;
    // Start is called before the first frame update
    void Start()
    {
        myCollider = GetComponent<BoxCollider2D>();
        myLine = GetComponent<LineRenderer>();

        Mesh mesh = myCollider.CreateMesh(false, false);
        mesh.Optimize();

        Vector3[] positions = mesh.vertices;
        positions = positions.OrderBy(pos => Vector3.SignedAngle(pos.normalized, Vector3.up, Vector3.forward)).ToArray();

        myLine.positionCount = positions.Length;
        myLine.SetPositions(positions);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponentInParent<HasHealth>().team.AddPoints(Time.deltaTime);
        }
    }



}
