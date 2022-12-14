using UnityEngine;

[CreateAssetMenu(fileName = "Audio", menuName = "ScriptableObjects/AudioScriptableObject")]

public class AudioScriptableObject : ScriptableObject
{
    // Maybe individual sounds
    [Header("Player Sounds")]
    public AudioClip death; 
    public AudioClip damaged;

    [Header("Weapon Sounds")]
    public AudioClip emptyMag;

    [Header("Bullet Sounds")]
    public AudioClip impact_wall;
    public AudioClip impact_wood;
    public AudioClip impact_glass;
    public AudioClip bulletBounce;

    [Header("Soundtrack volume (Controlled by AudioManager)")]
    public float volume;
}
