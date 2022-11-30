using UnityEngine;

[CreateAssetMenu(fileName = "Audio", menuName = "ScriptableObjects/AudioScriptableObject")]

public class AudioScriptableObject : ScriptableObject
{
    [Header("Player Sounds")]
    public AudioClip walking;
    public AudioClip death;
    public AudioClip damaged;


    [Header("Gun Sounds")]
    public AudioClip fire;
    public AudioClip shotgun;
    public AudioClip emptyMag;

    [Header("Item Sounds")]
    public AudioClip itemPickup;
    public AudioClip itemDestroyed;

    [Header("Bullet Sounds")]
    public AudioClip impact_wall;
    public AudioClip impact_glass;
    public AudioClip bulletBounce;
    public AudioClip bulletExplode;
}
