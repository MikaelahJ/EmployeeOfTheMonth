using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomMask : MonoBehaviour
{
    public string playerSpriteName;
    bool startCollision;
    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(DisableAllMasks), 0.1f);
    }

    void DisableAllMasks()
    {
        if(!startCollision)
        GetComponent<SpriteMask>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        startCollision = true;
        Debug.Log("Trigger Entered with " + collision.name);
        if(collision.name != playerSpriteName) { return; }
        Debug.Log("Enabled mask: " + collision.name);
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<SpriteMask>().enabled = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Trigger Left: " + collision.name);
        if (collision.name != playerSpriteName) { return; }

        if (collision.transform.parent.TryGetComponent<HasHealth>(out HasHealth health))
        {
            if (health.isDead)
            {
                Debug.Log("RoomMask: Player died");
                return;
            }
        }

        Debug.Log("Disabled mask: " + collision.name);
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<SpriteMask>().enabled = false;
    }
}
