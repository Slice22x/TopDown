using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public enum SoundType
    {
        SFX,
        Music,
        Misc,
    }

    public SoundType ThisSound;

    public AudioClip Clip;
    public string Name;

    [Range(0f,1f)]
    public float Volume;
    [Range(.1f,3f)]
    public float Pitch;
    public bool Loop;

    
    public AudioSource Source;
}
