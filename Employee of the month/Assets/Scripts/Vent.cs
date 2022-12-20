using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vent : MonoBehaviour
{
    //[SerializeField] private Sprite openVent;
    //private Sprite closedSprite;
    //private Sprite sprite;
    private TrailRenderer ventTrail;
    [SerializeField] private Animator animator;

    private VentManager ventManager;
    private int spawnVentPos;
    private Transform spawnPos;

    private void Start()
    {
        //closedSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        ventManager = GameObject.Find("VentManager").GetComponent<VentManager>();
        ventTrail = GetComponentInChildren<TrailRenderer>();
        
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
        animator.SetTrigger("Vent");

        
        var player = playerToMove.gameObject.GetComponentInParent<Transform>().parent.gameObject;//get player prefab

        foreach (Transform child in player.transform)//get circleHighlight
        {
            Debug.Log(child.name);
            if (child.name == "PlayerCircleHighlight(Clone)")
            {
                ventTrail.startColor = child.GetComponent<SpriteRenderer>().color;//set trail to player colour
                ventTrail.endColor = child.GetComponent<SpriteRenderer>().color;//set trail to player colour

                Debug.Log("startcolour" + ventTrail.GetComponent<TrailRenderer>().startColor);

            }
        }



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
        playerToMove.gameObject.GetComponentInParent<Movement>().justTeleported = false;
    }
}
