using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRemovedFromGun : MonoBehaviour
{
    public Sprite sprite;
    public Sprite brokenSprite;
    public Vector3 moveDirection;

    public bool sticksToWalls = false;

    public float throwDistance = 0.5f;
    public float throwHeight = 1;
    public float animationSpeed = 4;
    public float breakSize = 1f;

    private Vector3 startLocalScale;
    private Rigidbody2D rb2d;

    private float timer = 0;
    private bool isBroken = false;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        GetComponent<SpriteRenderer>().sprite = sprite;
        startLocalScale = transform.localScale;
        //For Dynamic mode
        //Debug.Log("Distance: " + moveDirection * throwDistance);
        float offset = 0.1f;
        float randomDistance = throwDistance + throwDistance * Random.Range(-throwDistance * offset, throwDistance * offset);
        rb2d.AddForce(moveDirection * randomDistance, ForceMode2D.Impulse);
    }

    void FixedUpdate()
    {
        if (isBroken) { return; }

        //For Kinematic Mode
        //Vector3 moveDistance = throwDistance * moveDirection * Time.deltaTime;
        //rb2d.MovePosition(transform.position + moveDistance);

        float size = Mathf.Sin(timer);

        transform.localScale = startLocalScale + startLocalScale * throwHeight * size;

        timer += (Time.deltaTime * animationSpeed);

        if(breakSize - 1 > size) 
        {
            BreakItem(null);
        }
    }

    void BreakItem(Collision2D wall)
    {
        isBroken = true;


        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().sortingOrder = -1;
        GetComponent<SpriteRenderer>().sprite = brokenSprite;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        transform.localScale = new Vector3(breakSize, breakSize, breakSize);

        //If not broken by wall
        if (wall == null)
        {
            return;
        }

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
            transform.localEulerAngles = new Vector3(wallDirection.x * -70, wallDirection.y * 70, transform.localEulerAngles.z);
            GetComponent<SpriteRenderer>().sortingOrder = 1;

            float wallX = Mathf.Abs(wallNormal.x);
            float wallY = Mathf.Abs(wallNormal.y);


            //Vector2 getPositionFromWall = new Vector2(wall.transform.position.x * wallX, wall.transform.position.y * wallY);
            //Vector2 getPositionFromItem = new Vector2(transform.position.x * wallY, transform.position.y * wallX);
            Vector2 offset = wallNormal / 8;
            //Debug.Log("Offset " + offset);
            //transform.position = getPositionFromWall + getPositionFromItem + offset;
            transform.position -= (Vector3)offset;
        }
        else
        {
            //Item is behind a wall
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(!sticksToWalls) { return; }

        Debug.Log("Collided with " + other.transform.name);
        if (other.gameObject.CompareTag("HardWall") || other.gameObject.CompareTag("SoftWall"))
        {
            Debug.Log("Item Collided with wall");
            BreakItem(other);
        }
    }

}
