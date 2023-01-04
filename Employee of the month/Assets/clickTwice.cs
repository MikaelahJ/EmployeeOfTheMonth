using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class clickTwice : MonoBehaviour
{
    [SerializeField] private Image buttonImage;
    [SerializeField] private Sprite clickedOnce;
    int clicks = 0;

    public bool OnClick()
    {
        if(clicks == 0)
        {
            buttonImage.sprite = clickedOnce;
            clicks++;
            return false;
        }
        return true;
    }
}
