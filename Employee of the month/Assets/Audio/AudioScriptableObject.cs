using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Audio", menuName = "ScriptableObjects/AudioScriptableObject")]

public class AudioScriptableObject : ScriptableObject
{
    // Maybe individual sounds
    [Header("Player Sounds")]
    public List<AudioClip> player1Deaths;
    public List<AudioClip> player2Deaths;
    public List<AudioClip> player3Deaths;
    public List<AudioClip> player4Deaths;

    public AudioClip damaged;

    [Header("Weapon Sounds")]
    public AudioClip emptyMag;
    public AudioClip laserCharge;

    [Header("Bullet Sounds")]
    public AudioClip impact_wall;
    public AudioClip impact_wood;
    public AudioClip impact_glass;
    public AudioClip bulletBounce;

    [Header("Volume (Controlled by AudioManager)")]
    [Range(0, 1)] public float musicVolume;
    [Range(0, 1)] public float sfxVolume;
    [Range(0, 1)] public float characterVolume;
}
