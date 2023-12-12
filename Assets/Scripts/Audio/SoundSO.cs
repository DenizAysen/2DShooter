using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class SoundSO : ScriptableObject
{
    public AudioTypes AudioType;
    public AudioClip Clip;
    public bool Loop = false;
    public bool RandomizePitch = false;
    [Range(0f , 1f)]
    public float RandomPitchRangeModifier = .1f;
    [Range(.1f, 2f)]
    public float Volume = 1f;
    [Range(.1f, 3f)]
    public float Pitch = 1f;
}

public enum AudioTypes
{
    SFX,
    Music
};