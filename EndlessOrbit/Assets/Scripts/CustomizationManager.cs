using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizationManager : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] List<SliderScript> sliders;
    [SerializeField] GameObject player;
    [SerializeField] Image previewPlayer;

    SpriteRenderer playerSprite;

    private void Start()
    {
        playerSprite = player.GetComponent<SpriteRenderer>();
        previewPlayer.color = playerSprite.color;
        previewPlayer.sprite = playerSprite.sprite;
        
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
        if (panel.activeInHierarchy)
        {
            sliders[0].ActivatePanel(playerSprite.color.r);
            sliders[1].ActivatePanel(playerSprite.color.g);
            sliders[2].ActivatePanel(playerSprite.color.b);
        }
        else
        {
            playerSprite.color = previewPlayer.color;
            playerSprite.sprite = previewPlayer.sprite;
        }

    }

    public void ChangeImage(Sprite s)
    {
        previewPlayer.sprite = s;
    }

    void UpdateColorListener()
    {
        previewPlayer.color = new Color(sliders[0].getValue(), sliders[1].getValue(), sliders[2].getValue());
    }
}
