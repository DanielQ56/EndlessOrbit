using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;

    [SerializeField] AudioSource EffectsSource;
    [SerializeField] AudioSource MusicSource;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            
        }
        else
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void SetStartingVolume(float mv = 0.5f, float ev = 0.5f)
    {
        EffectsSource.volume = ev;
        MusicSource.volume = mv;
    }

    public void PlayMusic(AudioClip clip)
    {
        MusicSource.clip = clip;
        MusicSource.Play();
    }

    public void PlayEffect(AudioClip clip)
    {
        EffectsSource.clip = clip;
        EffectsSource.Play();
    }

    public void PauseSounds()
    {
        EffectsSource.Pause();
        MusicSource.Pause();
    }

    public void UnpauseSounds()
    {
        EffectsSource.UnPause();
        MusicSource.UnPause();
    }

    public void AdjustMusicVolume(float value)
    {
        MusicSource.volume = value;
    }

    public void AdjustEffectsVolume(float value)
    {
        EffectsSource.volume = value;
    }

    public void MuteMusic()
    {
        MusicSource.volume = 0f;
    }

    public void MuteEffects()
    {
        EffectsSource.volume = 0f;
    }

    public float GetMusicVolume()
    {
        return MusicSource.volume;
    }

    public float GetEffectsVolume()
    {
        return EffectsSource.volume;
    }
}
