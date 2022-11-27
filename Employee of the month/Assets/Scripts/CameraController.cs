using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public GameObject player;
    public GameObject cube;

    [SerializeField] [Range(0, 10)] float speed = 1;
    [SerializeField] [Range(0, 100)] float offset = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveCamera(player.transform.position, cube.transform.position);
    }


    void MoveCamera(Vector3 player, Vector3 cube)
    {
        Vector3 middle = ((player - transform.position) + (cube - transform.position))/2;

        transform.position += middle*Time.deltaTime*speed;
        transform.position = Vector3.Lerp(transform.position, new (transform.position.x, transform.position.y, -(offset + Vector2.Distance(player, cube))), Time.deltaTime);
    }
}
