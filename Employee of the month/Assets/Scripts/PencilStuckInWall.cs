using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PencilStuckInWall : MonoBehaviour
{
    public GameObject pencil;
    public GameObject crack;

    public void SetPencilPosition(Collision2D wall)
    {
        Vector2 conctactPoint = wall.GetContact(0).point;
        Vector2 wallNormal = wall.GetContact(0).normal;


        float penDistanceFromCenter = Vector2.Dot(conctactPoint, wallNormal);
        float wallDistanceFromCenter = Vector2.Dot(wall.transform.position, wallNormal);

        float distanceBetweenPenCollisionAndMiddlePointOfWallCollider = penDistanceFromCenter - wallDistanceFromCenter;

        //puts the pen 1/4th of the wall up from contact point
        Vector2 offset = wallNormal * 0.5f * distanceBetweenPenCollisionAndMiddlePointOfWallCollider;
        //Debug.Log("Offset " + offset);
        transform.position = conctactPoint - offset;
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

        //The centerpoint where all walls are facing towards this point
        Vector3 wallGraphicsCenterPoint = new Vector3(0, -1, 0);

        //Checks distance from Vector3.zero, the dot product of the normal vector compares only the distance in relation to the walls face
        //a wall along the x-axis will have a normal of (0, 1) and then we compare only the y value distance from center to figure out if the pen is above or below the wall
        float itemDistanceFromCenter = Mathf.Abs(Vector2.Dot((transform.position - wallGraphicsCenterPoint), wallNormal));
        float wallDistanceFromCenter = Mathf.Abs(Vector2.Dot((wall.transform.position - wallGraphicsCenterPoint), wallNormal));

        //Debug.Log("WallNormal: " + wallNormal);
        //Debug.Log("Wall: " + wallDistanceFromCenter);
        //Debug.Log("Item: " + itemDistanceFromCenter);

        if (wallDistanceFromCenter < itemDistanceFromCenter)
        {
            //Item is behind a wall
            pencil.GetComponent<SpriteRenderer>().sortingOrder = -2;
            Destroy(crack);
            return;
        }
        else
        {
            //Debug.Log("Put item on wall");
            //Set the items angle to 70deg relative to the walls face
            crack.transform.localEulerAngles = new Vector3(wallDirection.x * -70, wallDirection.y * 70, transform.localEulerAngles.z);
            crack.GetComponent<SpriteRenderer>().sortingOrder = 1;

            //float wallX = Mathf.Abs(wallNormal.x);
            //float wallY = Mathf.Abs(wallNormal.y);

            //Vector2 conctactPoint = wall.GetContact(0).point;

            //Vector2 getPositionFromWall = new Vector2(wall.transform.position.x * wallX, wall.transform.position.y * wallY);
            //Vector2 getPositionFromItem = new Vector2(transform.position.x * wallY, transform.position.y * wallX);
            //Vector2 offset = wallNormal / 10;
            //Vector2 offset = Vector2.zero;
            //Debug.Log("Offset " + offset);
            //transform.position = getPositionFromWall + getPositionFromItem + offset;
            //transform.position = conctactPoint - offset;
        }
    }
}
