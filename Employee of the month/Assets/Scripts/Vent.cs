using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vent : MonoBehaviour
{
    [SerializeField] private Sprite openVent;
    private Sprite closedSprite;
    private Sprite sprite;

    private VentManager ventManager;
    private int spawnVentPos;
    private Transform spawnPos;

    private void Start()
    {
        closedSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
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
        playerToMove.gameObject.GetComponentInParent<Movement>().justTeleported = true;

        gameObject.GetComponent<SpriteRenderer>().sprite = openVent;

        //which vent to spawn player at
        spawnVentPos = ventManager.ventConnection[gameObject];
        foreach (var pos in ventManager.ventConnection)
        {
            if (pos.Key.GetComponent<SpriteRenderer>().sprite == ventManager.ventSprites[spawnVentPos])
            {
                spawnPos = pos.Key.transform;
            }
        }

        playerToMove.gameObject.GetComponentInParent<Rigidbody2D>().position = spawnPos.position;

        StartCoroutine(CloseVent(playerToMove));
    }
    IEnumerator CloseVent(Collider2D collision)
    {
        yield return new WaitForSeconds(0.5f);
        collision.gameObject.GetComponentInParent<Movement>().justTeleported = false;
        gameObject.GetComponent<SpriteRenderer>().sprite = closedSprite;
    }
}