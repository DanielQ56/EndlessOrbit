using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwapToggleSprites : MonoBehaviour
{
    [SerializeField] GameObject MusicVolume;
    [SerializeField] GameObject EffectsVolume;

    [SerializeField] Sprite mvOn;
    [SerializeField] Sprite mvOff;
    [SerializeField] Sprite evOn;
    [SerializeField] Sprite evOff;

    Image mvImage;
    Image evImage;

    Button mvButton;
    Button evButton;

    private void Awake ()
    {
        mvImage = MusicVolume.GetComponent<Image>();
        evImage = EffectsVolume.GetComponent<Image>();
        mvButton = MusicVolume.GetComponent<Button>();
        evButton = EffectsVolume.GetComponent<Button>();
    }

    private void Start ()
    {
        mvButton.onClick.AddListener(ChangeMusicSprite);
        evButton.onClick.AddListener(ChangeEffectsSprite);
    }

    private void OnEnable ()
    {
        SyncSettings();
    }

    //Sync with Audio Manager
    private void SyncSettings ()
    {
        ChangeMusicSprite();
        ChangeEffectsSprite();
    }

    public void ChangeMusicSprite ()
    {
        if (AudioManager.instance.IsMusicOn())
        {
            mvImage.sprite = mvOn;
        }
        else
        {
            mvImage.sprite = mvOff;
        }
    }

    public void ChangeEffectsSprite ()
    {
        if (AudioManager.instance.IsEffectsOn())
        {
            evImage.sprite = evOn;
        }
        else
        {
            evImage.sprite = evOff;
        }
    }

    private void OnDestroy()
    {
        mvButton.onClick.RemoveListener(ChangeMusicSprite);
        evButton.onClick.RemoveListener(ChangeEffectsSprite);
    }
}
