using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIAmmoCounter : MonoBehaviour
{

    [SerializeField] private GameObject ammoCounter;
    TextMeshProUGUI ammoCount;

    void Awake()
    {
        ammoCount = ammoCounter.GetComponent<TextMeshProUGUI>();
        SetAmmo(0);
    }

    public void SetAmmo(int ammo)
    {
        if (ammo < 0)
            ammo = 0;
        ammoCount.text = ammo.ToString();
    }
}
