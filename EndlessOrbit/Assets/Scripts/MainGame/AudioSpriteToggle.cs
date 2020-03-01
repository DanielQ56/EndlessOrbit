using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSpriteToggle : MonoBehaviour
{
    Image m_img;
    [SerializeField] Sprite mutedSprite;
    [SerializeField] Sprite unmutedSprite;

    void Awake()
    {
        m_img = GetComponent<Image>(); 
    }

    void Update()
    {
        if (AudioManager.instance.muted)
            m_img.sprite = mutedSprite;
        else
            m_img.sprite = unmutedSprite;
    }

}
