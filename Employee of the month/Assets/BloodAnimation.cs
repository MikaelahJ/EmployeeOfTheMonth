using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodAnimation : MonoBehaviour
{
    public Quaternion setRotation;
    public bool isPenetrate;

    //Sprites are wrong rotation so we correct it with this
    private float rotate = -90;

    // Start is called before the first frame update
    void Start()
    {
        if (isPenetrate)
        {
            //Play blood animation behind
            rotate = -rotate;
        }
        else
        {
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }

        Destroy(gameObject, 0.5f);
    }

    void LateUpdate()
    {
        transform.rotation = setRotation;
        transform.eulerAngles += new Vector3(0, 0, rotate); 
    }
}
