using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound 
{
    public string name;
    public AudioClip clip;

    [Range(0f,1.5f)]
    public float volume;
    [Range(0.1f, 3f)]
    public float pitch = 1f;

    public bool loop;

    public AudioMixerGroup group;

    [HideInInspector]
    public AudioSource source;
}
