﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;
using System.Linq;
using UnityEngine.UI;

public class OldAudioManager : MonoBehaviour
{
    public static OldAudioManager instance = null;
    [SerializeField] Sound[] sfx;
    [SerializeField] Sound currentEffect = null;
    [SerializeField] Sprite Enabled;
    [SerializeField] Sprite Disabled;
    [SerializeField] Image AudioImage;

    public bool muted = false;

    Dictionary<bool, Sprite> AudioActive;

    void Awake()
    {
        if (instance == null)
            instance = this;

        //Initialize SFX Array
        foreach(Sound track in sfx)
        {
            track.source = gameObject.AddComponent<AudioSource>();
            track.source.clip = track.clip;
            track.source.volume = track.volume;
        }
        AudioActive = new Dictionary<bool, Sprite>();
        AudioActive.Add(true, Enabled);
        AudioActive.Add(false, Disabled);

        if(PlayerPrefs.GetInt("mute", 0) == 1)
        {
            ToggleSound();   
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

        if (!muted && s.source != null)
            s.source.Play();
    }

    public void ToggleSound()
    {
        muted = !muted;
        Debug.Log("Muted is: " + muted);
        AudioImage.sprite = AudioActive[muted];
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetInt("mute", muted ? 1 : 0);
    }

}

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    public float volume;
    public AudioSource source;
}
