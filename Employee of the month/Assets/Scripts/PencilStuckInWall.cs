using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PencilStuckInWall : MonoBehaviour
{
    public GameObject pencil;
    public GameObject crack;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPencilRotation(Quaternion rotation)
    {
        pencil.transform.rotation = rotation;
    }

    public void SetCrackTransform(Collision2D wall)
    {
        //Vector perpendicular to wall
        Vector2 wallNormal = wall.GetContact(0).normal;
        //Vector towards wall from collision point
        Vector2 wallDirection = new Vector2(wallNormal.y, wallNormal.x);

        float itemDistanceFromCenter = Mathf.Abs(Vector2.Dot((Vector2)transform.position, wallNormal));
        float wallDistanceFromCenter = Mathf.Abs(Vector2.Dot((Vector2)wall.transform.position, wallNormal));

        //Debug.Log("Wall: " + wallDistanceFromCenter);
        //Debug.Log("Item: " + itemDistanceFromCenter);

        if (wallDistanceFromCenter > itemDistanceFromCenter)
        {
            Debug.Log("Put item on wall");
            //Set the items angle to 70deg relative to the walls face
            crack.transform.localEulerAngles = new Vector3(wallDirection.x * -70, wallDirection.y * 70, transform.localEulerAngles.z);
            crack.GetComponent<SpriteRenderer>().sortingOrder = 1;

            float wallX = Mathf.Abs(wallNormal.x);
            float wallY = Mathf.Abs(wallNormal.y);

            Vector2 conctactPoint = wall.GetContact(0).point;

            //Vector2 getPositionFromWall = new Vector2(wall.transform.position.x * wallX, wall.transform.position.y * wallY);
            //Vector2 getPositionFromItem = new Vector2(transform.position.x * wallY, transform.position.y * wallX);
            Vector2 offset = wallNormal / 8;
            //Debug.Log("Offset " + offset);
            //transform.position = getPositionFromWall + getPositionFromItem + offset;
            transform.position = conctactPoint - offset;
        }
        else
        {
            //Item is behind a wall
            Destroy(gameObject);
        }
    }
}
