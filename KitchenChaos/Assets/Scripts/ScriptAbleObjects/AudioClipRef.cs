using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AudioClipRef : ScriptableObject
{
    public AudioClip[] chop;
    public AudioClip[] deliveryFail;
    public AudioClip[] deliverySuccess;
    public AudioClip[] footStep;
    public AudioClip[] objectDrop;
    public AudioClip[] objectPickup;
    public AudioClip panSizzle;
    public AudioClip[] trash;
    public AudioClip[] warning;

}
