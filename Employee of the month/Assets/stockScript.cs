using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stockScript : MonoBehaviour
{
    [SerializeField] private Sprite[] playerStockSprites;
    public int playerIndex;
    private int stocks;
    public float offset = 0.5f;
    public GameObject stockHolder;

    // Start is called before the first frame update
    void Start()
    {

    }
    
    public void AddStocks(int stocks)
    {
        for (int i = 0; i < stocks; i++)
        {
            GameObject newStock = new GameObject("Stock " + (i + 1), typeof(SpriteRenderer));
            newStock.transform.parent = stockHolder.transform;
            newStock.transform.localScale = Vector3.one;
            SpriteRenderer spriteRenderer = newStock.GetComponent<SpriteRenderer>();
            int random = Random.Range(0, playerStockSprites.Length);
            spriteRenderer.sprite = playerStockSprites[random];
            spriteRenderer.sortingOrder = 50;
            spriteRenderer.sortingLayerID = SortingLayer.NameToID("Foreground");

            newStock.transform.localPosition = new Vector3(offset * i, 0, 0);
        }
    }

    public void LoseStock()
    {
        if(stockHolder.transform.childCount > 0)
        {
            Destroy(stockHolder.transform.GetChild(stockHolder.transform.childCount - 1).gameObject);
        }
    }



}
