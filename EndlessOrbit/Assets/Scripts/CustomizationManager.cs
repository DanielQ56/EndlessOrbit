using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizationManager : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] List<SliderScript> sliders;
    [SerializeField] Image previewPlayer;

    SpriteRenderer playerSprite;

    private void Start()
    {
        previewPlayer.color = PlayerCustomization.instance.playerColor;
        previewPlayer.sprite = PlayerCustomization.instance.playerSprite;
        
        foreach(SliderScript s in sliders)
        {
            s.UpdatedColor.AddListener(UpdateColorListener);
        }
        panel.SetActive(false);
    }

    public void TogglePanel()
    {
        panel.SetActive(!panel.activeInHierarchy);
    }
    

    public void ChangeImage(Sprite s)
    {
        previewPlayer.sprite = s;
    }

    public void SaveChanges()
    {
        PlayerCustomization.instance.playerSprite = previewPlayer.sprite;
        PlayerCustomization.instance.playerColor = previewPlayer.color;
    }

    void UpdateColorListener()
    {
        previewPlayer.color = new Color(sliders[0].getValue(), sliders[1].getValue(), sliders[2].getValue());
    }
}
