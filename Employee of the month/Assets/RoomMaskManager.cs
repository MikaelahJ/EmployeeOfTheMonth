using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomMaskManager : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private List<SpriteMask> rooms;
    public string layerName;

    void Start()
    {
        foreach (var spriteMask in rooms)
        {
            spriteMask.frontSortingLayerID = SortingLayer.NameToID(layerName);
            spriteMask.backSortingLayerID = SortingLayer.NameToID(layerName);
            spriteMask.GetComponent<RoomMask>().playerSpriteName = layerName;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
