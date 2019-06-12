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
        StartCoroutine(ToggleSliders());
    }

    IEnumerator ToggleSliders()
    {
        yield return new WaitForEndOfFrame();
        if(panel.activeInHierarchy)
        {
            sliders[0].ActivatePanel(previewPlayer.color.r);
            sliders[1].ActivatePanel(previewPlayer.color.g);
            sliders[2].ActivatePanel(previewPlayer.color.b);
        }
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

    void UpdateColorListener(int index)
    {
        previewPlayer.DisableSpriteOptimizations();
        //StartCoroutine(ChangeColor(index));
        switch (index)
        {
            case 0:
                previewPlayer.color = new Color(sliders[0].getValue(), previewPlayer.color.g, previewPlayer.color.b);
                break;
            case 1:
                previewPlayer.color = new Color(previewPlayer.color.r, sliders[1].getValue(), previewPlayer.color.b);
                break;
            case 2:
                previewPlayer.color = new Color(previewPlayer.color.r, previewPlayer.color.g, sliders[2].getValue());
                break;
        }
    }

    IEnumerator ChangeColor(int index)
    {
        yield return new WaitForEndOfFrame();
        switch (index)
        {
            case 0:
                previewPlayer.color = new Color(sliders[0].getValue(), previewPlayer.color.g, previewPlayer.color.b);
                break;
            case 1:
                previewPlayer.color = new Color(previewPlayer.color.r, sliders[1].getValue(), previewPlayer.color.b);
                break;
            case 2:
                previewPlayer.color = new Color(previewPlayer.color.r, previewPlayer.color.g, sliders[2].getValue());
                break;
        }
    }
}
