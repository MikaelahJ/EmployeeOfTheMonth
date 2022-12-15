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

    public AudioClip ding;

    [Header("Volume (Controlled by AudioManager)")]
    [Range(0, 1)] public float musicVolume;
    [Range(0, 1)] public float sfxVolume;
    [Range(0, 1)] public float characterVolume;
}
