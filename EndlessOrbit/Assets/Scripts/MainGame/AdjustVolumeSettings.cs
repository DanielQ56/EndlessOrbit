using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdjustVolumeSettings : MonoBehaviour
{
    [SerializeField] Slider MusicVolume;
    [SerializeField] Slider EffectsVolume;
    [SerializeField] Button Mute;
    [SerializeField] Button RestoreDefaults;

    private void Awake()
    {
        MusicVolume.onValueChanged.AddListener(ChangeMusicVolume);
        EffectsVolume.onValueChanged.AddListener(ChangeEffectsVolume);
        Mute.onClick.AddListener(MuteAll);
        RestoreDefaults.onClick.AddListener(RestoreDefaultVolume);
    }

    private void Start()
    {
        MusicVolume.value = AudioManager.instance.GetMusicVolume();
        EffectsVolume.value = AudioManager.instance.GetEffectsVolume();
        //this.gameObject.SetActive(false);
    }

    public void ChangeMusicVolume(float value)
    {
        AudioManager.instance.AdjustMusicVolume(value);
    }

    public void ChangeEffectsVolume(float value)
    {
        AudioManager.instance.AdjustEffectsVolume(value);
    }

    public void MuteAll()
    {
        MusicVolume.value = 0f;
        EffectsVolume.value = 0f;
    }

    public void RestoreDefaultVolume()
    {
        MusicVolume.value = 0.5f;
        EffectsVolume.value = 0.5f;
    }

    private void OnDestroy()
    {
        MusicVolume.onValueChanged.RemoveListener(ChangeMusicVolume);
        EffectsVolume.onValueChanged.RemoveListener(ChangeEffectsVolume);
        Mute.onClick.RemoveListener(MuteAll);
        RestoreDefaults.onClick.RemoveListener(RestoreDefaultVolume);
    }
}
