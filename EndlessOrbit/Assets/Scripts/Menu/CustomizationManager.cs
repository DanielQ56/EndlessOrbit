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
        foreach (SliderScript s in sliders)
        {
            s.UpdatedColor.AddListener(UpdateColorListener);
        }
        this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        previewPlayer.color = PlayerCustomization.instance.playerColor;
        previewPlayer.sprite = PlayerCustomization.instance.playerSprite;

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
        this.gameObject.SetActive(false);
    }

    void UpdateColorListener(int index)
    {
        previewPlayer.color = new Color(sliders[0].getValue(), sliders[1].getValue(), sliders[2].getValue());
    }

}
