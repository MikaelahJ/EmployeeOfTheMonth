using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomMask : MonoBehaviour
{
    public string playerSpriteName;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        Debug.Log("Trigger Entered with " + collision.name);
        if(collision.name != playerSpriteName) { return; }
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<SpriteMask>().enabled = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Trigger Left: " + collision.name);
        if (collision.name != playerSpriteName) { return; }
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<SpriteMask>().enabled = false;
    }
}
