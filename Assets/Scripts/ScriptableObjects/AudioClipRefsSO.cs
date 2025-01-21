using UnityEngine;

[CreateAssetMenu(fileName = "AudioClipRefsSO", menuName = "Scriptable Objects/AudioClipRefsSO")]
public class AudioClipRefsSO : ScriptableObject
{
    public AudioClip[] Chop;
    public AudioClip[] DeliveryFailed;
    public AudioClip[] DeliverySuccess;
    public AudioClip[] Footsteps;
    public AudioClip[] ObjectDrop;
    public AudioClip[] ObjectPickup;
    public AudioClip[] StoveSizzle;
    public AudioClip[] Trash;
    public AudioClip[] Warning;
}
