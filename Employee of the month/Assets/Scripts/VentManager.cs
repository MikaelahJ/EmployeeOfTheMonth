using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentManager : MonoBehaviour
{
    [SerializeField] public List<Sprite> ventSprites = new List<Sprite>();
    [SerializeField] private Sprite defaultVent;
    [SerializeField] private GameObject ventPrefab;

    private List<Transform> ventSpawnPoints = new List<Transform>();
    public Dictionary<GameObject, int> ventConnection = new Dictionary<GameObject, int>();

    private void Start()
    {
        GetSpawnPoints();
        SpawnItem();
    }

    private void GetSpawnPoints()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            ventSpawnPoints.Add(gameObject.transform.GetChild(i));
        }
    }

    private void SpawnItem()
    {
        int k = ventSpawnPoints.Count-1;
        for (int i = 0; i < ventSpawnPoints.Count; i++)
        {
            var ventSprite = ventSprites[i];

            var vent = Instantiate(ventPrefab, ventSpawnPoints[i]);
            vent.GetComponent<SpriteRenderer>().sprite = ventSprite;

            ventConnection.Add(vent, k);
            k--;
        }
    }
}
