using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentManager : MonoBehaviour
{
    //[SerializeField] public List<Sprite> ventSprites = new List<Sprite>();
    //[SerializeField] private Sprite defaultVent;
    //[SerializeField] private GameObject ventPrefab;

    private List<Transform> ventPoints = new List<Transform>();
    public Dictionary<GameObject, int> ventConnection = new Dictionary<GameObject, int>();

    private void Start()
    {
        GetVentPoints();
        ConnectVents();
    }

    private void GetVentPoints()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            ventPoints.Add(gameObject.transform.GetChild(i));
        }
    }

    private void ConnectVents()
    {
        int k = ventPoints.Count - 1;
        for (int i = 0; i < ventPoints.Count; i++)
        {
            //var ventSprite = ventSprites[i];
            //var vent = Instantiate(ventPrefab, ventSpawnPoints[i]);
            //vent.GetComponent<SpriteRenderer>().sprite = ventSprite;
            //ventConnection.Add(vent, k);

            ventConnection.Add(ventPoints[i].gameObject, k);
            k--;
        }
        //foreach (KeyValuePair<GameObject, int> kvp in ventConnection)
        //{
        //    Debug.LogFormat("ventconnection: {0} - {1}", kvp.Key, kvp.Value);
        //}
    }
}
