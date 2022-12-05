using UnityEngine;

[CreateAssetMenu(fileName = "Audio", menuName = "ScriptableObjects/AudioScriptableObject")]

public class AudioScriptableObject : ScriptableObject
{
    [Header("Player Sounds")]
    public AudioClip walking;
    public AudioClip death;
    public AudioClip damaged;

    public AudioClip emptyMag;

    [Header("Bullet Sounds")]
    public AudioClip impact_wall;
    public AudioClip impact_wood;
    public AudioClip impact_glass;
    public AudioClip bulletBounce;
}
