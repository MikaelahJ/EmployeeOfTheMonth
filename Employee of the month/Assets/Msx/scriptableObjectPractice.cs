using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "testSO", menuName = "ScriptableObjects/Max/Test")]
public class scriptableObjectPractice : ScriptableObject
{
    public float topSpeed;
    public bool isStickShift;
    public float acceleration;
}
