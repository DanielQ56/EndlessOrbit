using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;
using System.Linq;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;
    [SerializeField] Sound[] sfx;
    [SerializeField] Sound currentEffect = null;
    public bool muted = false;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        //Initialize SFX Array
        foreach(Sound track in sfx)
        {
            track.source = gameObject.AddComponent<AudioSource>();
            track.source.clip = track.clip;
            track.source.volume = track.volume;
        }
    }


    public void Play(string name)
    {
        Sound s = Array.Find(sfx, sound => sound.name == name);
        currentEffect = s;

        if (s == null)
        {
            Debug.Log("ERROR: Sound not found");
            return;
        }

        if (!muted)
            s.source.Play();
    }

    public void ToggleSound()
    {
        muted = !muted;
    }

}

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    public float volume;
    [HideInInspector]
    public AudioSource source;
}
