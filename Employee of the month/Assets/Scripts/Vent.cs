using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vent : MonoBehaviour
{
    //[SerializeField] private Sprite openVent;
    //private Sprite closedSprite;
    //private Sprite sprite;

    private VentManager ventManager;
    private int spawnVentPos;
    private Transform spawnPos;

    private void Start()
    {
        //closedSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        ventManager = GameObject.Find("VentManager").GetComponent<VentManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.GetComponentInParent<Movement>().justTeleported == false)
            {
                WhereToGo(collision);
            }
        }
    }

    public void WhereToGo(Collider2D playerToMove)
    {
       TrailRenderer trail =  playerToMove.GetComponentInChildren<TrailRenderer>();

        trail.emitting = true;
        playerToMove.gameObject.GetComponentInParent<Movement>().justTeleported = true;

        //which vent to spawn player at
        spawnVentPos = ventManager.ventConnection[gameObject];
        char ventNumber = this.gameObject.name[name.Length - 1];
        foreach (var pos in ventManager.ventConnection)
        {
            //if (pos.Key.GetComponent<SpriteRenderer>().sprite == ventManager.ventSprites[spawnVentPos])
            //{
            //    spawnPos = pos.Key.transform;
            //}
            //Debug.Log("ventnumber" + ventNumber);
            //Debug.Log("posvalue" + pos.Value.ToString());
            //Debug.Log("poskey" + pos.Key);
            if (pos.Value.ToString() == ventNumber.ToString())
            {
                spawnPos = pos.Key.transform;
            }
        }

        playerToMove.gameObject.GetComponentInParent<Rigidbody2D>().position = spawnPos.position;

        StartCoroutine(JustTeleported(playerToMove));
    }

    IEnumerator JustTeleported(Collider2D playerToMove)
    {
        yield return new WaitForSeconds(0.3f);
        TrailRenderer trail = playerToMove.GetComponentInChildren<TrailRenderer>();
        trail.emitting = false;

        playerToMove.gameObject.GetComponentInParent<Movement>().justTeleported = false;
    }
}
