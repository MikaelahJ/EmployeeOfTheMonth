using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoCounter : MonoBehaviour
{
    public int currentAmmo = 0;
    string textAmmo = "000";

    [SerializeField] private GameObject ammoCounter;
    TextMeshProUGUI ammoCount;

    private void Start()
    {
        ammoCount = ammoCounter.GetComponent<TextMeshProUGUI>();

        SetAmmo(999);
    }

    public void SetAmmo(int ammo)
    {
        if (ammo < 0)
            ammo = 0;

        string temp = "000" + ammo;
        textAmmo = temp.Substring(temp.Length - 3);
        ammoCount.text = textAmmo;
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
