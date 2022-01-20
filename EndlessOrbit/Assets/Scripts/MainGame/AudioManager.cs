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
        MusicSource.volume = mv;
        EffectsSource.volume = ev;
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

    public void ToggleMusicVolume()
    {
        if (IsMusicOn()) MuteMusic();
        else UnmuteMusic();
    }

    public void ToggleEffectsVolume()
    {
        if (IsEffectsOn()) MuteEffects();
        else UnmuteEffects();
    }


    #region Mute/Unmute
    public void MuteMusic()
    {
        MusicSource.volume = 0f;
    }

    public void MuteEffects()
    {
        EffectsSource.volume = 0f;
    }

    public void UnmuteMusic(float mv = 0.5f)
    {
        MusicSource.volume = mv;
    }

    public void UnmuteEffects(float ev = 0.5f)
    {
        EffectsSource.volume = ev;
    }

    #endregion

    #region Get Functions
    public float GetMusicVolume()
    {
        return MusicSource.volume;
    }

    public bool IsMusicOn()
    {
        return MusicSource.volume == 0.5f;
    }

    public float GetEffectsVolume()
    {
        return EffectsSource.volume;
    }

    public bool IsEffectsOn()
    {
        return EffectsSource.volume == 0.5f;
    }
    #endregion
}
