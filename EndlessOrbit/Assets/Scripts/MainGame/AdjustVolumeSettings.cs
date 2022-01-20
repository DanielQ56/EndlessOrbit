using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdjustVolumeSettings : MonoBehaviour
{
    [SerializeField] Button MusicVolume;
    [SerializeField] Button EffectsVolume;

    private void Awake()
    {
        MusicVolume.onClick.AddListener(ChangeMusicVolume);
        EffectsVolume.onClick.AddListener(ChangeEffectsVolume);
    }

    public void ChangeMusicVolume()
    {
        AudioManager.instance.ToggleMusicVolume();
    }

    public void ChangeEffectsVolume()
    {
        AudioManager.instance.ToggleEffectsVolume();
    }
    
    private void OnDestroy()
    {
        MusicVolume.onClick.RemoveListener(ChangeMusicVolume);
        EffectsVolume.onClick.RemoveListener(ChangeEffectsVolume);
    }
}
