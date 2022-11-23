using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoCounter : MonoBehaviour
{
    int currentAmmo = 0;
    string textAmmo = "000";
    TextMeshProUGUI ammoCounter;

    private void Start()
    {
        ammoCounter = GetComponentInChildren<TextMeshProUGUI>();

        SetAmmo(30);
    }

    public void SetAmmo(int ammo)
    {
        string temp = "000" + ammo;
        textAmmo = temp.Substring(temp.Length - 3);
        ammoCounter.text = textAmmo;
        currentAmmo = ammo;
    }

    public void LoseAmmo()
    {
        currentAmmo--;
        SetAmmo(currentAmmo);
    }

    public void LoseAmmo(int ammo)
    {
        currentAmmo -= ammo;
        SetAmmo(currentAmmo);
    }
}
