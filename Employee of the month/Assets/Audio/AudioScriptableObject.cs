using UnityEngine;

[CreateAssetMenu(fileName = "Audio", menuName = "ScriptableObjects/AudioScriptableObject")]

public class AudioScriptableObject : ScriptableObject
{
    [Header("Player Sounds")]
    public AudioClip walking; 



    [Header("Gun Sounds")]
    public AudioClip fire;
    public AudioClip emptyMag;

    [Header("Item Sounds")]
    public AudioClip itemPickup;
    public AudioClip itemDestroyed;
}
