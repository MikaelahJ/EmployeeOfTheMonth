using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public scriptableObjectPractice engine;


    private void Update()
    {
        if (engine == null) return;

        this.transform.position += Vector3.up * engine.topSpeed * Time.deltaTime;
        engine.isStickShift = false;
    }


    public void AddEngine(scriptableObjectPractice newEngine)
    {
        engine = Instantiate(newEngine);
    }
}
